using System;

namespace Modbus.Message
{
	/// <summary>
	/// Extensions to the IModbusMessage interface to support custom messages over the RTU protocol.
	/// </summary>
	public interface IModbusMessageRtuMEI : IModbusMessageRtu
    {
		/// <summary>
		/// Gets the remaining length of the message given the specified frame start.
		/// </summary>
		Func<byte[], Func<int, byte[]>, byte[]> ReadNeededBytes { get; }

	}
}
