using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using log4net;
using Modbus.Device;
using Modbus.Message;
using Modbus.Utility;
using Unme.Common;

namespace Modbus.IO
{
	/// <summary>	
	/// Refined Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
	/// </summary>
	internal class ModbusRtuTransport : ModbusSerialTransport
	{
		public const int RequestFrameStartLength = 7;
		public const int ResponseFrameStartLength = 4;

		private static readonly ILog _logger = LogManager.GetLogger(typeof(ModbusRtuTransport));

		private Func<Type, IModbusMessageRtu> _instanceCache = FunctionalUtility.Memoize((Type type) => (IModbusMessageRtu) Activator.CreateInstance(type));

		internal ModbusRtuTransport(IStreamResource streamResource)
			: base(streamResource)
		{
			Debug.Assert(streamResource != null, "Argument streamResource cannot be null.");
		}

	    /// <summary>
	    /// workaround for non-realtime implementation of the RTU protocol
	    /// </summary>
	    public static byte[] RequestNeededBytes(byte[] frameStart, ModbusSlave slave, Func<int, byte[]> read)
	    {
	        byte functionCode = frameStart[1];

	        // allow a custom function registered with the slave to provide the number of bytes left to read
	        CustomMessageInfo messageInfo;
	        if (slave.TryGetCustomMessageInfo(functionCode, out messageInfo) || messageInfo.Instance is IModbusMessageRtuMEI)
	            return ((IModbusMessageRtuMEI)messageInfo.Instance).ReadNeededBytes(frameStart, read);
	        return null;
	    }

	    /// <summary>
        /// workaround for non-realtime implementation of the RTU protocol
        /// </summary>
        public static int RequestBytesToRead(byte[] frameStart, ModbusSlave slave)
		{
			byte functionCode = frameStart[1];
			
			// allow a custom function registered with the slave to provide the number of bytes left to read
			CustomMessageInfo messageInfo;
			if (slave.TryGetCustomMessageInfo(functionCode, out messageInfo))
				return messageInfo.Instance.RtuBytesRemaining(frameStart);
			
			int numBytes;

			switch (functionCode)
			{
				case Modbus.ReadCoils:
				case Modbus.ReadInputs:
				case Modbus.ReadHoldingRegisters:
				case Modbus.ReadInputRegisters:
				case Modbus.WriteSingleCoil:
				case Modbus.WriteSingleRegister:
				case Modbus.Diagnostics:
					numBytes = 1;
					break;
				case Modbus.WriteMultipleCoils:
				case Modbus.WriteMultipleRegisters:
					byte byteCount = frameStart[6];
					numBytes = byteCount + 2;
					break;
				default:
					string errorMessage = String.Format(CultureInfo.InvariantCulture, "Function code {0} not supported.", functionCode);
					_logger.Error(errorMessage);
					throw new NotImplementedException(errorMessage);
			}

			return numBytes;
		}

	    /// <summary>
	    /// workaround for non-realtime implementation of the RTU protocol
	    /// </summary>
	    public static byte[] RequestNeededBytes<T>(byte[] frameStart, Func<Type, IModbusMessageRtu> instanceCache, Func<int, byte[]> read)
	    {
	        // allow a custom message to provide the number of bytes left to read
	        if (typeof (IModbusMessageRtuMEI).IsAssignableFrom(typeof (T)))
	        {
	            var inst = instanceCache.Invoke(typeof (T)) as IModbusMessageRtuMEI;
	            return inst?.ReadNeededBytes(frameStart, read);
	        }
	        return null;

	    }

	    /// <summary>
        /// workaround for non-realtime implementation of the RTU protocol
        /// </summary>
        public static int ResponseBytesToRead<T>(byte[] frameStart, Func<Type, IModbusMessageRtu> instanceCache)
		{
			// allow a custom message to provide the number of bytes left to read
			if (typeof(IModbusMessageRtu).IsAssignableFrom(typeof(T)))
				return instanceCache.Invoke(typeof(T)).RtuBytesRemaining(frameStart);

			byte functionCode = frameStart[1];

			// exception response
			if (functionCode > Modbus.ExceptionOffset)
				return 1;

			int numBytes;
			switch (functionCode)
			{
				case Modbus.ReadCoils:
				case Modbus.ReadInputs:
				case Modbus.ReadHoldingRegisters:
				case Modbus.ReadInputRegisters:
					numBytes = frameStart[2] + 1;
					break;
				case Modbus.WriteSingleCoil:
				case Modbus.WriteSingleRegister:
				case Modbus.WriteMultipleCoils:
				case Modbus.WriteMultipleRegisters:
				case Modbus.Diagnostics:
					numBytes = 4;
					break;
				default:
					string errorMessage = String.Format(CultureInfo.InvariantCulture, "Function code {0} not supported.", functionCode);
					_logger.Error(errorMessage);
					throw new NotImplementedException(errorMessage);
			}

			return numBytes;
		}

		public virtual byte[] Read(int count)
		{
			byte[] frameBytes = new byte[count];
			int numBytesRead = 0;

			while (numBytesRead != count)
				numBytesRead += StreamResource.Read(frameBytes, numBytesRead, count - numBytesRead);

			return frameBytes;
		}

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			List<byte> messageBody = new List<byte>();
			messageBody.Add(message.SlaveAddress);
			messageBody.AddRange(message.ProtocolDataUnit);
			messageBody.AddRange(ModbusUtility.CalculateCrc(message.MessageFrame));

			return messageBody.ToArray();
		}

		internal override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame)
		{
			return BitConverter.ToUInt16(messageFrame, messageFrame.Length - 2) == BitConverter.ToUInt16(ModbusUtility.CalculateCrc(message.MessageFrame), 0);
		}

		internal override IModbusMessage ReadResponse<T>()
		{
			var frameStart = Read(ResponseFrameStartLength);
			var frameEnd = Read(ResponseBytesToRead<T>(frameStart, _instanceCache));
			var frame = frameStart.Concat(frameEnd).ToArray();
		    var additionalData = RequestNeededBytes<T>(frame, _instanceCache, Read);
		    if (additionalData != null)
		        frame = frame.Concat(additionalData).ToArray();
		    _logger.DebugFormat("RX: {0}", frame.Join(", "));

			return CreateResponse<T>(frame, Read);
		}

		internal override byte[] ReadRequest(ModbusSlave slave)
		{
			var frameStart = Read(RequestFrameStartLength);
			var frameEnd = Read(RequestBytesToRead(frameStart, slave));
			var frame = frameStart.Concat(frameEnd).ToArray();
		    var additionalData = RequestNeededBytes(frameStart, slave, Read);
            if(additionalData != null)
                frame = frame.Concat(additionalData).ToArray();
            _logger.DebugFormat("RX: {0}", frame.Join(", "));

			return frame;
		}
	}
}
