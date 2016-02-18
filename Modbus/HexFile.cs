using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modbus
{
    class HexFile : List<byte>
    {
        private readonly Action<string> _log;
        private readonly bool _verbose;
        // Currently the biggest AVR controller has 256k Flash
        const uint MAX_FLASH_BYTES = 262144;

        public string ErrorString { get; private set; }
        


        public HexFile(Action<string> log, bool verbose)
        {
            _log = log;
            _verbose = verbose;
            Reset();
        }

        public bool Load(string fileName)
        {
            if (!File.Exists(fileName))
            {
                ErrorString = "File not found";
                return false;
            }



            Reset();
            ushort lineNr = 0;
            uint extendedSegmentAddress = 0;

            var lines = File.ReadAllLines(fileName);

            foreach (var line in lines)
            {
                ++lineNr;

                // grab hexfile information
                var byteCount = Convert.ToByte(line.Substring(1, 2), 16);  // get number of bytes
                uint checkSum = byteCount;  // start checksum computation
                uint address = Convert.ToByte(line.Substring(3, 2), 16);  // get address high byte
                checkSum += address;  // checksum...
                address = address << 8;
                var low = Convert.ToByte(line.Substring(5, 2), 16);  // get address low byte
                address |= low;
                checkSum += low;  // checksum...
                var recordType = Convert.ToByte(line.Substring(7, 2), 16);  // get record type
                checkSum += recordType;
                var fileCheckSum = Convert.ToByte(line.Substring((byteCount * 2) + 9, 2), 16);
                //unsigned char fileCheckSum = asciiToHex(line[(byteCount * 2) + 9], line[(byteCount * 2) + 10]);  // get the checksum

                switch (recordType)
                {
                    case 2:
                        // extended segment address record
                        var extendedSegmentAddressHigh = Convert.ToByte(line.Substring(9, 2), 16);
                        extendedSegmentAddress = (uint)(extendedSegmentAddressHigh << 8);
                        checkSum += extendedSegmentAddressHigh;  // chechsum...
                        var extendedSegmentAddressLow = Convert.ToByte(line.Substring(11, 2), 16);
                        extendedSegmentAddress += extendedSegmentAddressLow;
                        checkSum += extendedSegmentAddressLow;  // chechsum...
                        if (_verbose)
                        {
                            _log("Got extended adress record");
                        }
                        break;

                    case 1:
                        // end of file record
                        if (_verbose)
                        {
                            _log("Loaded hex file");
                            _log("Read " + Count + " bytes");
                        }
                        break;

                    case 0:
                        {
                            // data record
                            for (var i = 0; i < (2 * ((uint)byteCount)); i += 2)
                            {
                                var dataByte = Convert.ToByte(line.Substring(i + 9, 2), 16); ;
                                checkSum += dataByte;  // compute checksum
                                if (!SetByte((int)(address + (extendedSegmentAddress * 16) + (i >> 1)), dataByte))
                                {
                                    ErrorString = "Maximum size exceeded";
                                    return false;
                                }
                            }

                        }
                        break;
                    default:
                        ErrorString = $@"Found unknown or unsupported record type (0x{recordType:X2})";
                        return false;
                }

                // check if checksum error
                if (((checkSum + fileCheckSum) & 0xff) != 0)
                {
                    ErrorString = $@"Checksum error in line {lineNr}: Expected {fileCheckSum}, computed {checkSum}";
                    return false;
                }
            }

            return true;
        }

        public bool SetByte(int address, byte data)
        {
            if (address >= MAX_FLASH_BYTES)
            {
                ErrorString = $@"Overflow (address {address})";
                return false;
            }
            if (address >= Count)
                AddRange(new byte[address - Count + 1]);
            this[address] = data;
            return true;
        }

        public bool SetByte(int address, char data)
        {
            var b = Encoding.ASCII.GetBytes(new[] { data })[0];
            return SetByte(address, b);
        }

        public void Reset()
        {
            Clear();
        }

        public new void Add(byte item)
        {
            if (Count >= MAX_FLASH_BYTES)
            {
                ErrorString = $@"Overflow (address {Count + 1})";
                return;
            }
            base.Add(item);
        }

        public void Add(char c)
        {
            var b = Encoding.ASCII.GetBytes(new[] { c })[0];
            Add(b);
        }

        public bool Equal(HexFile other)
        {
            if (Count != other.Count)
                return false;

            for (var i = 0; i < Count; ++i)
                if (this[i] != other[i])
                    return false;
            return true;
        }

        public string[] GetHexFile()
        {
            var result = new List<string>();
            //const char zeropad('0');
            const int byteCount = 16;  // byte count is fixed to 16 bytes
            var hexSize = Count;

            var lines = (hexSize + (byteCount - 1)) / byteCount; // round up

            for (var line = 0; line < lines; ++line)
            {
                var address = line * byteCount;
                ushort addressSh = (ushort)address;

                // preamble

                if ((address > 0)                          // new segment
                    && ((address % (0x10000)) == 0))
                {
                    //ushort segAddress = address / 16;
                    var segAddress = (ushort)(address >> 4);
                    var segChksum = (byte)(((2 + 2 + (segAddress >> 8) + (segAddress & 0xFF)) ^ 0xFF) + 1);
                    var segStr = $":02000002{segAddress:X4}{segChksum:X2}";
                    result.Add(segStr);

                }
                var thisLinesBytecount = (address + byteCount) > hexSize
                                                ? hexSize - address
                                                : byteCount;
                var str = $@":{thisLinesBytecount:X2}{addressSh:X4}00";
                var checksum = (byte)((byte)thisLinesBytecount + (byte)(addressSh >> 8) + (byte)(addressSh & 0xFF));
                for (var i = 0; i < thisLinesBytecount; ++i)
                {
                    var b = this[address + i];
                    checksum += b;
                    str += b.ToString("X2");
                }
                checksum = (byte)((checksum ^ 0xFF) + 1);
                str += $"{checksum:X2}";
                result.Add(str);
            }
            result.Add(":00000001FF");

            return result.ToArray();
        }

    }
}
