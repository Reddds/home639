using System;

namespace Modbus
{
    static class HexFileTester
    {
        public static void Test(string fileName, Action<string> log)
        {
            var hf = new HexFile(log, true);

            // test load
            if (!hf.Load(fileName))
            {
                log("Error opening hexfile '" + fileName + "': " + hf.ErrorString);
                return;
            }

            // test write routine
            if (!HexUtils.WriteHexfile(fileName + "_out2.hex", hf))
                return;

            // test empty file
            hf.Reset();
            if (!HexUtils.WriteHexfile(fileName + "_out3.hex", hf))
                return;

            // test add
            hf.Reset();
            hf.Add('h');
            hf.Add('a');
            hf.Add('l');
            hf.Add('l');
            hf.Add('0');
            hf.Add(0);
            hf.Add(1);
            hf.Add(2);
            hf.Add(3);
            hf.Add(4);
            if (!HexUtils.WriteHexfile(fileName + "_out4.hex", hf))
                return;

            // test set
            hf.SetByte(1, 'e');
            hf.SetByte(8, 255);
            if (!HexUtils.WriteHexfile(fileName + "_out5.hex", hf))
                return;

            // test set after current end
            hf.SetByte(654, 255);
            hf.Add(0);
            hf.Add(1);
            hf.Add(2);
            hf.Add(3);
            hf.Add(4);
            if (!HexUtils.WriteHexfile(fileName + "_out6.hex", hf))
                return;

            // test extended (>64k) range
            hf.SetByte(0x10000, 0x11);
            hf.SetByte(0x105FF, 0x22);
            hf.Add(0);
            hf.Add(1);
            hf.Add(2);
            hf.Add(3);
            hf.Add(4);
            hf.SetByte(0x205FF, 0x33);
            if (!HexUtils.WriteHexfile(fileName + "_out7.hex", hf))
                return; // TODO: Complain?
        }
    }
}
