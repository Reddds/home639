using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using log4net;
using Modbus.Message;
using Unme.Common;

namespace Modbus.IO
{
	/// <summary>
	/// Transport for Serial protocols.
	/// Refined Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
	/// </summary>
	public abstract class ModbusSerialTransport : ModbusTransport
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(ModbusSerialTransport));
		private bool _checkFrame = true;

		internal ModbusSerialTransport(IStreamResource streamResource)
			: base(streamResource)
		{
			Debug.Assert(streamResource != null, "Argument streamResource cannot be null.");
		}

		/// <summary>
		/// Gets or sets a value indicating whether LRC/CRC frame checking is performed on messages.
		/// </summary>
		public bool CheckFrame
		{
			get { return _checkFrame; }
			set { _checkFrame = value; }
		}

		internal void DiscardInBuffer()
		{
			StreamResource.DiscardInBuffer();
		}

		internal override void Write(IModbusMessage message)
		{
			DiscardInBuffer();

			byte[] frame = BuildMessageFrame(message);
			_logger.DebugFormat("TX: {0}", frame.Join(", "));
			StreamResource.Write(frame, 0, frame.Length);
		}

		internal override IModbusMessage CreateResponse<T>(byte[] frame, Func<int, byte[]> read)
		{
			IModbusMessage response = base.CreateResponse<T>(frame, read);

			// compare checksum
			if (CheckFrame && !ChecksumsMatch(response, frame))
			{
				string errorMessage = String.Format(CultureInfo.InvariantCulture, "Checksums failed to match {0} != {1}", response.MessageFrame.Join(", "), frame.Join(", "));
				_logger.Error(errorMessage);
				throw new IOException(errorMessage);
			}

			return response;
		}

		internal abstract bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame);

		internal override void OnValidateResponse(IModbusMessage request, IModbusMessage response)
		{
			// no-op
		}
	}
}
