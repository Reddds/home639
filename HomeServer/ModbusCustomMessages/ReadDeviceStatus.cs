using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Modbus.Data;

namespace HomeServer
{
    class ReadDeviceStatus : Modbus.Message.IModbusMessageRtu//, IModbusMessageDataCollection
    {
        private const byte ReadDeviceStatusFunctionCode = 102;
        /// <summary>
        /// Длина заголовка сообщения
        /// </summary>
        private const int HeaderSize = 3;

        public byte? DeviceStatus { get; set; }

        public bool[] DeviceStatusBits
        {
            get
            {
                if (DeviceStatus == null)
                    return null;
                var bits = new bool[8];
                byte curMask = 1;
                for (var i = 0; i < bits.Length; i++)
                {
                    bits[i] = (DeviceStatus.Value & curMask) > 0;
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
            DeviceStatus = frame[2];
        }

        public byte FunctionCode
        {
            get
            {
                return ReadDeviceStatusFunctionCode;
            }
            set
            {
                if (value != ReadDeviceStatusFunctionCode)
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.InvariantCulture, "Invalid function code in message frame, expected: {0}; actual: {1}",
                        ReadDeviceStatusFunctionCode,
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
                if (DeviceStatus == null)
                    return new[] { FunctionCode };

                return new[] { FunctionCode, DeviceStatus.Value };
            }
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

    class ReadDeviceStatusRequest : ReadDeviceStatus
    {
        public ReadDeviceStatusRequest(byte slaveAddress)
        {
            SlaveAddress = slaveAddress;
        }
    }

}
