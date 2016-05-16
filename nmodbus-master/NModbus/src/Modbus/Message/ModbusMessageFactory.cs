using System;
using System.Globalization;
using Modbus.Device;

namespace Modbus.Message
{
	internal static class ModbusMessageFactory
	{
		private const int MinRequestFrameLength = 3;

		public static T CreateModbusMessage<T>(byte[] frame, Func<int, byte[]> read) where T : IModbusMessage, new()
		{
			IModbusMessage message = new T();
			message.Initialize(frame, read);

			return (T) message;
		}

		public static IModbusMessage CreateModbusRequest(ModbusSlave slave, byte[] frame)
		{
			if (slave == null)
				throw new ArgumentNullException("slave");
			if (frame.Length < MinRequestFrameLength)
				throw new FormatException(String.Format(CultureInfo.InvariantCulture, "Argument 'frame' must have a length of at least {0} bytes.", MinRequestFrameLength));

			IModbusMessage request;
			byte functionCode = frame[1];

			// allow custom function override
			if (!slave.TryCreateModbusMessageRequest(functionCode, frame, out request))
			{
				// default implementations
				switch (functionCode)
				{
					case Modbus.ReadCoils:
					case Modbus.ReadInputs:
						request = CreateModbusMessage<ReadCoilsInputsRequest>(frame, null);
						break;
					case Modbus.ReadHoldingRegisters:
					case Modbus.ReadInputRegisters:
						request = CreateModbusMessage<ReadHoldingInputRegistersRequest>(frame, null);
						break;
					case Modbus.WriteSingleCoil:
						request = CreateModbusMessage<WriteSingleCoilRequestResponse>(frame, null);
						break;
					case Modbus.WriteSingleRegister:
						request = CreateModbusMessage<WriteSingleRegisterRequestResponse>(frame, null);
						break;
					case Modbus.Diagnostics:
						request = CreateModbusMessage<DiagnosticsRequestResponse>(frame, null);
						break;
					case Modbus.WriteMultipleCoils:
						request = CreateModbusMessage<WriteMultipleCoilsRequest>(frame, null);
						break;
					case Modbus.WriteMultipleRegisters:
						request = CreateModbusMessage<WriteMultipleRegistersRequest>(frame, null);
						break;
					case Modbus.ReadWriteMultipleRegisters:
						request = CreateModbusMessage<ReadWriteMultipleRegistersRequest>(frame, null);
						break;
					default:
						throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Unsupported function code {0}", functionCode), "frame");
				}
			}

			return request;
		}
	}
}
