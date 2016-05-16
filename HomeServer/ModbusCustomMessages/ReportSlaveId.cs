using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Modbus.Data;

namespace HomeServer
{
    class ReportSlaveId : Modbus.Message.IModbusMessageRtu, IModbusMessageDataCollection
    {
        private const byte ReportSlaveIdFunctionCode = 0x11;
        /// <summary>
        /// Длина заголовка сообщения
        /// </summary>
        private const int HeaderSize = 3;

        public byte[] ReseivedId { get; set; }
        public byte[] NetworkBytes { get; }
        public byte ByteCount { get; private set; }
        public void Initialize(byte[] frame, Func<int, byte[]> read)
        {
            if (frame == null)
                throw new ArgumentNullException(nameof(frame));

            if (frame.Length <= HeaderSize)//
                throw new FormatException("Message frame does not contain enough bytes.");

            ByteCount = frame[2];
            if (ByteCount > (frame.Length - HeaderSize - 2))//
                throw new FormatException("Message frame does not contain enough bytes.");



            SlaveAddress = frame[0];
            FunctionCode = frame[1];

            ReseivedId = new byte[ByteCount];
            Array.Copy(frame, HeaderSize, ReseivedId, 0, ByteCount);

        }

        public byte FunctionCode
        {
            get
            {
                return ReportSlaveIdFunctionCode;
            }
            set
            {
                if (value != ReportSlaveIdFunctionCode)
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.InvariantCulture, "Invalid function code in message frame, expected: {0}; actual: {1}",
                        ReportSlaveIdFunctionCode,
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
                if(ReseivedId == null)
                    return new[] {FunctionCode};

                var pdu = new List<byte>
                {
                    // Function code
                    FunctionCode,
                    (byte) ReseivedId.Length
                };
                pdu.AddRange(ReseivedId);
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
                return frameStart => frameStart[2] + 1;
            }
        }
    }


    class ReportSlaveIdRequest : ReportSlaveId
    {
        public ReportSlaveIdRequest(byte slaveAddress)
        {
            SlaveAddress = slaveAddress;
        }
    }

}
