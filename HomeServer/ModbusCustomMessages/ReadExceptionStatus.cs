using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Modbus.Data;

namespace HomeServer
{
    class ReadExceptionStatus : Modbus.Message.IModbusMessageRtu//, IModbusMessageDataCollection
    {
        private const byte ReadExceptionStatusFunctionCode = 0x07;
        /// <summary>
        /// Длина заголовка сообщения
        /// </summary>
        private const int HeaderSize = 3;

        public byte? ExceptionStatus { get; set; }

        public bool[] ExceptionStatusBits
        {
            get
            {
                if (ExceptionStatus == null)
                    return null;
                var bits = new bool[8];
                byte curMask = 1;
                for (var i = 0; i < bits.Length; i++)
                {
                    bits[i] = (ExceptionStatus.Value & curMask) > 0;
                    curMask <<= 1;
                }
                return bits;
            }
        }
        public void Initialize(byte[] frame, Func<int, byte[]> read)
        {
            if (frame == null)
                throw new ArgumentNullException(nameof(frame));

            if (frame.Length != 5)// +2 CRC
                throw new FormatException("Message frame does not contain enough bytes.");

            SlaveAddress = frame[0];
            FunctionCode = frame[1];
            ExceptionStatus = frame[2];
        }

        public byte FunctionCode
        {
            get
            {
                return ReadExceptionStatusFunctionCode;
            }
            set
            {
                if (value != ReadExceptionStatusFunctionCode)
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.InvariantCulture, "Invalid function code in message frame, expected: {0}; actual: {1}",
                        ReadExceptionStatusFunctionCode,
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
                if (ExceptionStatus == null)
                    return new[] { FunctionCode };

                return new[] { FunctionCode, ExceptionStatus.Value };
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
                return frameStart => 1; // 4 Уже прочитано
            }
        }
    }


    class ReadExceptionStatusRequest : ReadExceptionStatus
    {
        public ReadExceptionStatusRequest(byte slaveAddress)
        {
            SlaveAddress = slaveAddress;
        }
    }

}
