using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServer.ModbusCustomMessages
{
    class ReadDeviceIdentification : Modbus.Message.IModbusMessageRtuMEI//, IModbusMessageDataCollection
    {
        public class StringObject
        {
            public byte ObjectId { get; set; }
            public string ObjectValue { get; set; }
        }

        public enum ReadDeviceIdCodes
        {
            Basic = 0x01,
            Regular,
            OneSpecific = 0x04
        }

        private const byte ReadDeviceIdFunctionCode = 0x2B;
        /// <summary>
        /// Длина заголовка сообщения
        /// </summary>
        private const int HeaderSize = 3;

        protected ReadDeviceIdCodes DeviceIdCode;
        protected byte ObjectId;
        private byte _conformityLevel;

        public List<StringObject> StringObjects { get; set; }

        public void Initialize(byte[] frame, Func<int, byte[]> read)
        {
            if (frame == null)
                throw new ArgumentNullException(nameof(frame));

            SlaveAddress = frame[0];
            FunctionCode = frame[1];

            if (frame[2] != 0x0e)
                throw new FormatException($"Wrong MEI Type. ({frame[2]})");

            var tmp = frame[3];
            if (tmp < 1 || tmp > 4)
                throw new FormatException($"Wrong Read Device ID code. ({tmp})");

            DeviceIdCode = (ReadDeviceIdCodes)tmp;
            _conformityLevel = frame[4];
            if(_conformityLevel < 0x01 || _conformityLevel > 0x83 || (_conformityLevel > 0x03 && _conformityLevel < 0x81))
                throw new FormatException($"Wrong Conformity level. ({_conformityLevel:X2})");

            var moreFollows = frame[5];
            var nextObjectId = frame[6];
            if(moreFollows != 0 || nextObjectId != 0)
                throw new FormatException($"More Follows and Next Object Id other then zero are not supported");

            var objectsCount = frame[7];
            StringObjects = new List<StringObject>();
            var curPos = 8;
            for (var i = 0; i < objectsCount; i++)
            {
                if(frame.Length < curPos + 2)
                    throw new FormatException("Message frame does not contain enough bytes.");

                var len = frame[curPos + 1];
                if(frame.Length < curPos + 2 + len)
                    throw new FormatException("Message frame does not contain enough bytes.");

                var newStrObject = new StringObject
                {
                    ObjectId = frame[curPos],
                    ObjectValue = Encoding.ASCII.GetString(frame, curPos + 2, len)
                };


                //ObjectValue = Encoding.ASCII.GetString(read(len))
                curPos += 2 + len;
                StringObjects.Add(newStrObject);
            }
        }

        public byte FunctionCode
        {
            get
            {
                return ReadDeviceIdFunctionCode;
            }
            set
            {
                if (value != ReadDeviceIdFunctionCode)
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.InvariantCulture, "Invalid function code in message frame, expected: {0}; actual: {1}",
                        ReadDeviceIdFunctionCode,
                        value));
                }
            }
        }
        public byte SlaveAddress { get; set; }
        public byte[] MessageFrame
        {
            get
            {
                var frame = new List<byte>
                {
                    SlaveAddress
                };
                frame.AddRange(ProtocolDataUnit);

                return frame.ToArray();
            }
        }
        public byte[] ProtocolDataUnit
        {
            get
            {
                // Request
                if (StringObjects == null)
                    return new[]
                    {
                        FunctionCode,
                        (byte)0x0E,
                        (byte)DeviceIdCode,
                        ObjectId
                    };

                // Response
                var pdu = new List<byte>
                {
                    // Function code
                    FunctionCode,
                    0x0E, // MEI Type
                    (byte)DeviceIdCode,
                    _conformityLevel,
                    0,
                    0,
                    (byte)StringObjects.Count,
                };
                foreach (var stringObject in StringObjects)
                {
                    pdu.Add(stringObject.ObjectId);
                    pdu.Add((byte) stringObject.ObjectValue.Length);
                    pdu.AddRange(Encoding.ASCII.GetBytes(stringObject.ObjectValue));
                }
                return pdu.ToArray();
            }
        }

        private static ushort GetUshort(byte[] bytes, int startAddress)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentException("Массив не содержит элементов", nameof(bytes));
            if (startAddress < 0 || bytes.Length <= startAddress + 1)
                throw new ArgumentException("Начальный адрес вне границ массива", nameof(startAddress));
            return (ushort)((bytes[startAddress] << 8) + bytes[startAddress + 1]);
        }

        private static byte[] GetBytes(ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }

        public ushort TransactionId { get; set; }

        /// <summary>
        /// Максимальная длина Ushort массива
        /// </summary>
        public static uint MaxDataCount => 122;

        /// <summary>
        /// Максимальная длина Ushort массива при заданом размере всего сообщения
        /// </summary>
        /// <param name="maxRxSize"></param>
        /// <returns></returns>
        public static int MaxDataCountByBufferSize(int maxRxSize)
        {
            if (maxRxSize > 256)
                maxRxSize = 256;
            return (maxRxSize - (HeaderSize + 2)) / 2;
        }

        public Func<byte[], int> RtuBytesRemaining
        {
            get
            {
                return frameStart => 4;
            }
        }

        public Func<byte[], Func<int, byte[]>, byte[]> ReadNeededBytes
        {
            get
            {
                return (frame, read) =>
                {
                    var objectsCount = frame[7];
                    var additionalData = new List<byte>();
                    for (var i = 0; i < objectsCount; i++)
                    {
                        var idLen = read(2);
                        additionalData.AddRange(idLen);
                        additionalData.AddRange(read(idLen[1]));
                    }
                    additionalData.AddRange(read(2)); // CRC
                    return additionalData.ToArray();
                };
            }
            
        }
    }


    class ReadDeviceIdentificationRequest : ReadDeviceIdentification
    {

        public ReadDeviceIdentificationRequest(byte slaveAddress, ReadDeviceIdCodes deviceIdCode, byte objectId)
        {
            DeviceIdCode = deviceIdCode;
            ObjectId = objectId;
            SlaveAddress = slaveAddress;
        }
    }
}
