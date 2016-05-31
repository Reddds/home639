using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Modbus.Data;

namespace HomeServer.ModbusCustomMessages
{
    class ModbusWriteFileRecord : Modbus.Message.IModbusMessageRtu, IModbusMessageDataCollection
    {
        private const byte WriteWriteFileRecordFunctionCode = 0x15;
        /// <summary>
        /// Длина заголовка сообщения
        /// </summary>
        private const int HeaderSize = 10;

        protected ushort FileNumber;
        protected ushort RecordNumber;
        protected ushort[] RecordData;
        public byte[] NetworkBytes { get; }
        public byte ByteCount { get; private set; }
        public void Initialize(byte[] frame, Func<int, byte[]> read)
        {
            if (frame == null)
                throw new ArgumentNullException(nameof(frame));

            if (frame.Length < 10)//
                throw new FormatException("Message frame does not contain enough bytes.");

            var dataLen = GetUshort(frame, 8);
            ByteCount = frame[2];
            if (frame.Length < HeaderSize + dataLen * 2 || ByteCount < frame.Length - 3)//
                throw new FormatException("Message frame does not contain enough bytes.");


            SlaveAddress = frame[0];
            FunctionCode = frame[1];


            if (frame[3] != 6)
                throw new FormatException("Wrong Reference Type");

            FileNumber = GetUshort(frame, 4);
            RecordNumber = GetUshort(frame, 6);
            RecordData = new ushort[dataLen];
            for (var i = 0; i < RecordData.Count(); i++)
            {
                RecordData[i] = GetUshort(frame, 10 + i * 2);
            }
        }

        public byte FunctionCode
        {
            get
            {
                return WriteWriteFileRecordFunctionCode;
            }
            set
            {
                if (value != WriteWriteFileRecordFunctionCode)
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.InvariantCulture, "Invalid function code in message frame, expected: {0}; actual: {1}",
                        WriteWriteFileRecordFunctionCode,
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
                var pdu = new List<byte>
                {
                    // Function code
                    FunctionCode,
                    //The quantity of registers to be written, combined with all other fields in the request, must not
                    //exceed the allowable length of the MODBUS PDU : 253bytes.
                    // Request data length
                    // Reference Type
                    (byte) (RecordData.Count()*2 + 9),
                    6
                };

                pdu.AddRange(GetBytes(FileNumber)); //File Number
                pdu.AddRange(GetBytes(RecordNumber)); //Record Number
                pdu.AddRange(GetBytes((ushort)RecordData.Count()));//Record length

                foreach (var rec in RecordData)
                {
                    pdu.AddRange(GetBytes(rec));
                }

                // Временно!!!
                //                pdu.Insert(0, 1);
                //                var retArray = pdu.ToArray();
                //                var crc = Modbus.Utility.ModbusUtility.CalculateCrc(retArray);
                //                pdu.AddRange(crc);
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
            return (maxRxSize - (HeaderSize + 2))/2;
        }

        public Func<byte[], int> RtuBytesRemaining
        {
            get
            {
                return frameStart =>
                {
                    Console.WriteLine("!!!!!!!frameStart[2] = " + frameStart[2]);
                    return frameStart[2] - 1;
                };
            }
        }
    }


    class ModbusWriteFileRecordRequest : ModbusWriteFileRecord
    {
        public ModbusWriteFileRecordRequest(byte slaveAddress, ushort fileNumber, ushort recordNumber,
            ushort[] recordData)
        {
            FileNumber = fileNumber;
            RecordNumber = recordNumber;
            if (recordData.Count() * 2 + 9 > 253)
                throw new ArgumentException("Длина данных больше, чем поместится в запрос", nameof(recordData));
            RecordData = recordData;
            SlaveAddress = slaveAddress;
        }

        public ModbusWriteFileRecordRequest(byte slaveAddress, ushort fileNumber, ushort recordNumber,
            byte[] recordData, int startByte, int length)
        {
            if (startByte < 0)
                throw new ArgumentException("startByte не может быть меньше 0", nameof(recordData));
            if (startByte + length > recordData.Length)
                throw new ArgumentException("Выход за границы массива", nameof(recordData) + ", " + nameof(length));



            FileNumber = fileNumber;
            RecordNumber = recordNumber;
            SlaveAddress = slaveAddress;



            //            if(length % 2 != 0)
            //                throw new ArgumentException("Нечётное число байтов для передачи", nameof(recordData));

            var ushortLen = length / 2 + (length % 2);

            if (ushortLen * 2 + 9 > 253)
                throw new ArgumentException("Длина данных больше, чем поместится в запрос", nameof(recordData));

            RecordData = new ushort[ushortLen];

            for (var i = 0; i < RecordData.Length; i++)
            {
                RecordData[i] = (ushort)(recordData[startByte + i * 2] << 8);
                var nextIndex = startByte + i * 2 + 1;
                if (nextIndex < recordData.Length)
                    RecordData[i] |= recordData[nextIndex];
            }
        }
    }

}
