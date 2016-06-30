using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Chip45Programmer.Windows;
using Chip45ProgrammerLib;

namespace Chip45Programmer
{
    static class Ds30
    {
        public enum Ds30Commands { ErasePage, WriteInCode, WriteInEeprom, WriteConfig, ReadCodeMem }

        private const byte Ds30Hello = 0xC1;
        public const byte Ds30Ok = 0x4B; //erase/write ok
        private const byte Ds30CheckSumErr = 0x4e; // checksum error
        private const byte Ds30VerFail = 0x56; // verification failed
        private const byte Ds30BlProt = 0x50; // bl protection tripped
        private const byte Ds30UnknownCommand = 0x55;//'U'

        public static SerialPort Port;

        readonly static TimeSpan HelloWaitTime = TimeSpan.FromSeconds(15);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="bitNumber">Номер бита от 0</param>
        /// <returns></returns>
        static bool BitRead(int val, int bitNumber)
        {
            return (val & (1 << bitNumber)) != 0;
        }

        public static Tuple<int, byte, byte, byte, int> Ds30FirstConnect()
        {
            if (Port == null || !Port.IsOpen)
                return null;
            var buf = new byte[1];
            buf[0] = Ds30Hello;
            try
            {
                Port.DiscardInBuffer();
                var startTime = DateTime.Now;
                do
                {
                    Port.Write(buf, 0, buf.Length);
                    Thread.Sleep(5);
                    if (Port.BytesToRead > 0)
                        break;
                    Thread.Sleep(20);
                } while (DateTime.Now.Subtract(startTime) < HelloWaitTime);


                ushort deviceId = 0;
                var deviceLowId = Port.ReadByte();
                deviceId = (ushort)deviceLowId;
                var tmp = Port.ReadByte();
                deviceId |= (ushort)((ushort)(tmp & 0x80) << 1);
                var verMajor = (byte)(tmp & 0x7F);
                tmp = Port.ReadByte();
                deviceId |= (ushort)((ushort)(tmp & 0x80) << 2);
                var verMinor = (byte)((tmp >> 4) & 0x0f);
                var verRev = (byte)(tmp & 0x0f);
                var status = Port.ReadByte();
                Thread.Sleep(20);
                return new Tuple<int, byte, byte, byte, int>(deviceId, verMajor, verMinor, verRev, status);
            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось установить связь с загрузчиком\n" + ee);
            }
            return null;
        }

        public static void Ds30ProgramFlash(Action<string> log, string fileName, int rowSize, int pageSize)
        {//FpFlash
            var curPos = 0;
            try
            {
                const byte emptyByte = 0xff;
                var hexFile = new HexFile(log, true, true, emptyByte);
                hexFile.Load(fileName);


                var writeBuf = new byte[rowSize];
                var allLen = hexFile.Count;
                var needWrite = false;

                var helloResult = Ds30FirstConnect();
                if (helloResult == null || helloResult.Item5 != Ds30Ok)
                {
                    MessageBox.Show("Status is not Ok!");
                    return;
                }

                var lastPageErased = -1;

                do
                {
                    var i = 0;
                    for (; i < rowSize && curPos + i < allLen; i++)
                    {
                        writeBuf[i] = hexFile[curPos + i];
                        if (!needWrite && writeBuf[i] != emptyByte)
                            needWrite = true;
                    }
                    if (needWrite)
                    {
                        var curPage = curPos / pageSize;

                        if (curPage != lastPageErased)// || curPos %pageSize == 0
                        {
                            if (Ds30WriteCommand(Ds30Commands.ErasePage, curPos, null).Item1 != Ds30Ok)
                            {
                                MessageBox.Show($"Error erasing page at address {curPos}!");
                                break;
                            }
                            lastPageErased = curPage;
                        }
                        if (Ds30WriteCommand(Ds30Commands.WriteInCode, curPos, writeBuf).Item1 != Ds30Ok)
                        {
                            MessageBox.Show($"Error writing row at address {curPos}!");
                            break;
                        }
                    }
                    curPos += i;
                } while (curPos < allLen);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        public static byte[] Ds30ReadCodeMem(int startAddress, int length, int rowSize)
        {
            //var rowSize = (int)CbDs30RowSize.SelectionBoxItem;
            var retBuf = new byte[length];
            var rowsCount = length / rowSize + (length % rowSize > 0 ? 1 : 0);
            var curAddress = startAddress;

            for (var i = 0; i < rowsCount; i++, curAddress += rowSize)
            {
                var readResult = Ds30WriteCommand(Ds30Commands.ReadCodeMem, curAddress, null);
                if (readResult.Item1 != Ds30Ok)
                {
                    return null;
                }
                for (var j = 0; j < rowSize; j++)
                {
                    retBuf[i * rowSize + j] = readResult.Item2[j];
                }
            }
            return retBuf;

        }

        public static Tuple<int, byte[]> Ds30WriteCommand(Ds30Commands command, int address, byte[] bufBytes)
        {

            byte[] buf = null;
            byte checkSum = 0;
            switch (command)
            {
                case Ds30Commands.ErasePage:
                    buf = new byte[3 + 1 + 1 + 1];
                    buf[3] = 0x01;
                    checkSum += buf[3];
                    buf[4] = 0x01;
                    checkSum += buf[4];
                    //buf[5] = 0x00;
                    break;
                case Ds30Commands.WriteInCode:
                case Ds30Commands.WriteInEeprom:
                    buf = new byte[3 + 1 + 1 + bufBytes.Length + 1];
                    buf[3] = (byte)(command == Ds30Commands.WriteInCode ? 0x02 : 0x04);
                    checkSum += buf[3];
                    buf[4] = (byte)(bufBytes.Length + 1);
                    checkSum += buf[4];
                    for (var i = 0; i < bufBytes.Length; i++)
                    {
                        buf[5 + i] = bufBytes[i];
                        checkSum += buf[5 + i];
                    }
                    break;
                case Ds30Commands.WriteConfig:
                    buf = new byte[3 + 1 + 1 + 1 + 1];
                    buf[3] = 0x08;
                    checkSum += buf[3];
                    buf[4] = 0x02;
                    checkSum += buf[4];
                    buf[5] = bufBytes[0];
                    checkSum += buf[5];
                    break;
                case Ds30Commands.ReadCodeMem:
                    buf = new byte[3 + 1 + 1 + 1];
                    buf[3] = 0x10;
                    checkSum += buf[3];
                    buf[4] = 0x01;
                    checkSum += buf[4];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);

            }

            buf[0] = (byte)((address >> 16) & 0xff);
            checkSum += buf[0];
            buf[1] = (byte)((address >> 8) & 0xff);
            checkSum += buf[1];
            buf[2] = (byte)(address & 0xff);
            checkSum += buf[2];

            buf[buf.Length - 1] = (byte)(0xFF - checkSum + 1);

            //_port.Write(buf, 0, buf.Length);

            foreach (var b in buf)
            {
                Port.BaseStream.WriteByte(b);
                Thread.Sleep(1);
            }


            //Thread.Sleep(10);
            if (command == Ds30Commands.ReadCodeMem)
            {
                var checkSumForTest = (byte)0;

                var bytesCount = (byte)Port.ReadByte();
                checkSumForTest += bytesCount;
                var readbuf = new byte[bytesCount];

                for (var i = 0; i < readbuf.Length; i++)
                {
                    readbuf[i] = (byte)Port.ReadByte();
                    checkSumForTest += readbuf[i];
                }
                var readCheckSum = (byte)Port.ReadByte();
                if (readCheckSum == checkSumForTest)
                    return new Tuple<int, byte[]>(Ds30Ok, readbuf);
                else
                {
                    return new Tuple<int, byte[]>(Ds30CheckSumErr, null);
                }
            }
            return new Tuple<int, byte[]>(Port.ReadByte(), null);
        }

        public static void Ds30ReadConfigs(int rowSize)
        {
            var helloResult = Ds30.Ds30FirstConnect();
            if (helloResult == null || helloResult.Item5 != Ds30.Ds30Ok)
            {
                MessageBox.Show("Status is not Ok!");
                return;
            }

            var configs = Ds30ReadCodeMem(0x300000, 16, rowSize);
            if (configs == null)
            {
                MessageBox.Show("Error reading config!");
                return;
            }

            var configStr = new StringBuilder();

            var CONFIG1H = configs[1];
            var CONFIG2L = configs[2];
            var CONFIG2H = configs[3];
            var CONFIG3L = configs[4];
            var CONFIG3H = configs[5];
            var CONFIG4L = configs[6];
            var CONFIG4H = configs[7];
            var CONFIG5L = configs[8];
            var CONFIG5H = configs[9];
            var CONFIG6L = configs[0xA];
            var CONFIG6H = configs[0xB];
            var CONFIG7L = configs[0xC];
            var CONFIG7H = configs[0xD];


            configStr.Append("OSCEN = ");
            configStr.Append(BitRead(CONFIG1H, 5) ? "OFF" : "ON");
            configStr.AppendLine(" // Oscillator System Clock");

            var FOSC = CONFIG1H & 0x7;
            configStr.Append("FOSC = ");
            switch (FOSC)
            {
                case 0x00:
                    configStr.AppendLine("LP oscillator");
                    break;
                case 0x01:
                    configStr.AppendLine("XT oscillator");
                    break;
                case 0x02:
                    configStr.AppendLine("HS oscillator");
                    break;
                case 0x03:
                    configStr.AppendLine("RC oscillator");
                    break;
                case 0x04:
                    configStr.AppendLine("EC oscillator w/ OSC2 configured as divide-by-4 clock output");
                    break;
                case 0x05:
                    configStr.AppendLine("EC oscillator w/ OSC2 configured as RA6");
                    break;
                case 0x06:
                    configStr.AppendLine("HS oscillator with PLL enabled/Clock frequency = (4 x FOSC)");
                    break;
                case 0x07:
                    configStr.AppendLine("RC oscillator w/ OSC2 configured as RA6");
                    break;
            }

            //Brown-out Reset Voltage
            var BORV = (CONFIG2L >> 2) & 0x3;
            configStr.Append("BORV = ");
            switch (BORV)
            {
                case 0x00:
                    configStr.Append("4.5");
                    break;
                case 0x01:
                    configStr.Append("4.2");
                    break;
                case 0x02:
                    configStr.Append("2.7");
                    break;
                case 0x03:
                    configStr.Append("2.5");
                    break;
            }
            configStr.AppendLine("V // Brown-out Reset Voltage");


            configStr.Append("BOREN = ");
            configStr.Append(BitRead(CONFIG2L, 1) ? "ON" : "OFF");
            configStr.AppendLine(" // Brown-out Reset");

            configStr.Append("PWRTE = ");
            configStr.Append(BitRead(CONFIG2L, 0) ? "OFF" : "ON");
            configStr.AppendLine(" // Power-up Timer");

            var WDTPS = (CONFIG2H >> 1) & 0x7;
            configStr.Append("WDTPS = 1:");
            configStr.Append(1 << WDTPS);
            configStr.AppendLine(" // Watchdog Timer Postscale Select");

            configStr.Append("WDTEN = ");
            configStr.Append(BitRead(CONFIG2H, 0) ? "ON" : "OFF");
            configStr.AppendLine(" // Watchdog Timer");

            configStr.Append("CCP2MX = ");
            if (BitRead(CONFIG3H, 0))
                configStr.AppendLine("ON // CCP2 input/output is multiplexed with RC1");
            else
                configStr.AppendLine("OFF // CCP2 input/output is multiplexed with RB3");


            configStr.Append("BKBUG = ");
            configStr.Append(BitRead(CONFIG4L, 7) ? "OFF" : "ON");
            configStr.AppendLine(" // Background Debugger");

            configStr.Append("LVP = ");
            configStr.Append(BitRead(CONFIG4L, 2) ? "ON" : "OFF");
            configStr.AppendLine(" // Low Voltage ICSP");

            configStr.Append("STVREN = ");
            configStr.Append(BitRead(CONFIG4L, 0) ? "ON" : "OFF");
            configStr.AppendLine(" // Stack Full/Underflow Reset");

            configStr.Append("CP3 = ");
            configStr.Append(BitRead(CONFIG5L, 3) ? "OFF" : "ON");
            configStr.AppendLine(" // Code Protection Block 3 (006000-007FFFh)");
            configStr.Append("CP2 = ");
            configStr.Append(BitRead(CONFIG5L, 2) ? "OFF" : "ON");
            configStr.AppendLine(" // Code Protection Block 2 (004000-005FFFh)");
            configStr.Append("CP1 = ");
            configStr.Append(BitRead(CONFIG5L, 1) ? "OFF" : "ON");
            configStr.AppendLine(" // Code Protection Block 1 (002000-003FFFh)");
            configStr.Append("CP0 = ");
            configStr.Append(BitRead(CONFIG5L, 0) ? "OFF" : "ON");
            configStr.AppendLine(" // Code Protection Block 0 (000200-001FFFh)");

            configStr.Append("CPD = ");
            configStr.Append(BitRead(CONFIG5H, 7) ? "OFF" : "ON");
            configStr.AppendLine(" // Data EEPROM Code Protection");
            configStr.Append("CPB = ");
            configStr.Append(BitRead(CONFIG5H, 6) ? "OFF" : "ON");
            configStr.AppendLine(" // Boot Block Code Protection (000000-0001FFh)");

            configStr.Append("WRT3 = ");
            configStr.Append(BitRead(CONFIG6L, 3) ? "OFF" : "ON");
            configStr.AppendLine(" // Write Protection Block 3 (006000-007FFFh)");
            configStr.Append("WRT2 = ");
            configStr.Append(BitRead(CONFIG6L, 2) ? "OFF" : "ON");
            configStr.AppendLine(" // Write Protection Block 2 (004000-005FFFh)");
            configStr.Append("WRT1 = ");
            configStr.Append(BitRead(CONFIG6L, 1) ? "OFF" : "ON");
            configStr.AppendLine(" // Write Protection Block 1 (002000-003FFFh)");
            configStr.Append("WRT0 = ");
            configStr.Append(BitRead(CONFIG6L, 0) ? "OFF" : "ON");
            configStr.AppendLine(" // Write Protection Block 0 (000200h-001FFFh)");

            configStr.Append("WRTD = ");
            configStr.Append(BitRead(CONFIG6H, 7) ? "OFF" : "ON");
            configStr.AppendLine(" // Data EEPROM Write Protection");
            configStr.Append("WRTB = ");
            configStr.Append(BitRead(CONFIG6H, 6) ? "OFF" : "ON");
            configStr.AppendLine(" // Boot Block Write Protection (000000-0001FFh)");
            configStr.Append("WRTC = ");
            configStr.Append(BitRead(CONFIG6H, 5) ? "OFF" : "ON");
            configStr.AppendLine(" // Configuration Register Write Protection (300000-3000FFh)");

            configStr.Append("EBTR3 = ");
            configStr.Append(BitRead(CONFIG7L, 3) ? "OFF" : "ON");
            configStr.AppendLine(" // Table Read Protection Block 3 (006000-007FFFh)");
            configStr.Append("EBTR2 = ");
            configStr.Append(BitRead(CONFIG7L, 2) ? "OFF" : "ON");
            configStr.AppendLine(" // Table Read Protection Block 2 (004000-005FFFh)");
            configStr.Append("EBTR1 = ");
            configStr.Append(BitRead(CONFIG7L, 1) ? "OFF" : "ON");
            configStr.AppendLine(" // Table Read Protection Block 1 (002000-003FFFh)");
            configStr.Append("EBTR0 = ");
            configStr.Append(BitRead(CONFIG7L, 0) ? "OFF" : "ON");
            configStr.AppendLine(" // Table Read Protection Block 0 (000200h-001FFFh)");

            configStr.Append("EBTRB = ");
            configStr.Append(BitRead(CONFIG7H, 6) ? "OFF" : "ON");
            configStr.AppendLine(" // Boot Block Table Read Protection (000000-0001FFh)");

            MessageBox.Show(configStr.ToString());
        }

        public static void Ds30SendConfigByte()
        {
            var bfbWin = new ByteFromBitsWindow();
            if (bfbWin.ShowDialog() != true)
                return;
            var configByte = bfbWin.GetResultByte();
            var address = bfbWin.Address;
            if (address == null)
            {
                MessageBox.Show("Please enter valid address");
                return;
            }
            // Временная защита
            if (address.Value < 0x300000)
            {
                MessageBox.Show("Address is less then 0x300000!");
                return;
            }
            if (Ds30WriteCommand(Ds30Commands.WriteConfig, address.Value, new[] { configByte }).Item1 != Ds30Ok)
            {
                MessageBox.Show("Error writing config!");
            }
            else
            {
                MessageBox.Show("Config succesfully writed!");
            }
        }

        public static void Ds30ReadByte(int byteAddress, int rowSize)
        {
            var helloResult = Ds30FirstConnect();
            if (helloResult == null || helloResult.Item5 != Ds30Ok)
            {
                MessageBox.Show("Status is not Ok!");
                return;
            }

            var readed = Ds30ReadCodeMem(byteAddress, 1, rowSize);
            var sb = new StringBuilder();
            foreach (var b in readed)
            {
                sb.Append($"{b:X2} ");
            }
            MessageBox.Show(sb.ToString());
        }
    }
}
