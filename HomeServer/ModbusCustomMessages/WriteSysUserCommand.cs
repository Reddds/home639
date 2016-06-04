using System;
using System.Collections.Generic;
using System.Globalization;
using Modbus.Message;

namespace HomeServer.ModbusCustomMessages
{
    class WriteSysUserCommand : IModbusMessageRtu
    {
        protected bool IsSystem;
        private const byte SystemCommandFunctionCode = 100;
        private const byte UserCommandFunctionCode = 101;
        /// <summary>
        /// Длина заголовка сообщения
        /// </summary>
        private const int HeaderSize = 10;

        protected byte CommandId { get; set; }
        protected byte CommandData { get; set; }
        protected byte CommandAdd1Hi { get; set; }
        protected byte CommandAdd1Lo { get; set; }
        protected byte CommandAdd2Hi { get; set; }
        protected byte CommandAdd2Lo { get; set; }
        protected byte CommandAdd3Hi { get; set; }
        protected byte CommandAdd3Lo { get; set; }

        public void Initialize(byte[] frame, Func<int, byte[]> read)
        {
            if (frame == null)
                throw new ArgumentNullException(nameof(frame));

            if (frame.Length < 10)//
                throw new FormatException("Message frame does not contain enough bytes.");

            SlaveAddress = frame[0];
            FunctionCode = frame[1];

            CommandId = frame[2];
            CommandData = frame[3];
            CommandAdd1Hi = frame[4];
            CommandAdd1Lo = frame[5];
            CommandAdd2Hi = frame[6];
            CommandAdd2Lo = frame[7];
            CommandAdd3Hi = frame[8];
            CommandAdd3Lo = frame[9];
        }

        public byte FunctionCode
        {
            get
            {
                return IsSystem ? SystemCommandFunctionCode : UserCommandFunctionCode;
            }
            set
            {
                if (value == UserCommandFunctionCode)
                {
                    IsSystem = false;
                    return;
                }
                if (value == SystemCommandFunctionCode)
                {
                    IsSystem = true;
                    return;
                }
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "Invalid function code in message frame, expected: {0}; actual: {1}",
                    IsSystem ? SystemCommandFunctionCode : UserCommandFunctionCode,
                    value));
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
                    CommandId,
                    CommandData,
                    CommandAdd1Hi,
                    CommandAdd1Lo,
                    CommandAdd2Hi,
                    CommandAdd2Lo,
                    CommandAdd3Hi,
                    CommandAdd3Lo
                };

                return pdu.ToArray();
            }
        }

        public ushort TransactionId { get; set; }

        public Func<byte[], int> RtuBytesRemaining
        {
            get
            {
                return frameStart =>
                {
                    var func = frameStart[1];
                    if (func == (0x80 | FunctionCode))
                    {
                        var exc = frameStart[2];
                        switch (exc)
                        {
                            case 1: // EXC_FUNC_CODE
                                throw new ArgumentException("Function not supported!");
                            case 2: //EXC_ADDR_RANGE
                                throw new ArgumentException("Addres range error!");
                            case 3: // EXC_REGS_QUANT
                                throw new ArgumentException("Registers quantity error!");
                            case 4://EXC_EXECUTE
                                throw new ArgumentException("Execute error!");
                        }
                    }
                    return 8;
                };
            }
        }
    }


    class WriteSysUserCommandRequest : WriteSysUserCommand
    {
        public WriteSysUserCommandRequest(byte slaveAddress, bool isSystem,
                        byte commandId,
                        byte commandData,
                        byte commandAdd1Hi,
                        byte commandAdd1Lo,
                        byte commandAdd2Hi,
                        byte commandAdd2Lo,
                        byte commandAdd3Hi,
                        byte commandAdd3Lo)
        {
            SlaveAddress = slaveAddress;
            IsSystem = isSystem;
            CommandId = commandId;
            CommandData = commandData;
            CommandAdd1Hi = commandAdd1Hi;
            CommandAdd1Lo = commandAdd1Lo;
            CommandAdd2Hi = commandAdd2Hi;
            CommandAdd2Lo = commandAdd2Lo;
            CommandAdd3Hi = commandAdd3Hi;
            CommandAdd3Lo = commandAdd3Lo;
        }

        public WriteSysUserCommandRequest(byte slaveAddress, bool isSystem,
                        byte commandId,
                        byte commandData,
                        ushort commandAdd1,
                        ushort commandAdd2,
                        ushort commandAdd3)
        {
            SlaveAddress = slaveAddress;
            IsSystem = isSystem;
            CommandId = commandId;
            CommandData = commandData;
            CommandAdd1Hi = (byte) (commandAdd1 >> 8);
            CommandAdd1Lo = (byte) (commandAdd1 & 0xFF);
            CommandAdd2Hi = (byte) (commandAdd2 >> 8);
            CommandAdd2Lo = (byte) (commandAdd2 & 0xFF);
            CommandAdd3Hi = (byte) (commandAdd3 >> 8);
            CommandAdd3Lo = (byte) (commandAdd3 & 0xFF);
        }

        public WriteSysUserCommandRequest(byte slaveAddress, bool isSystem,
                        byte commandId,
                        byte commandData):this(slaveAddress, isSystem, 
                            commandId, commandData, 
                            0,0,0,0,0,0)
        {
            
        }
    }

}
