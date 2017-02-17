using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Chip45ProgrammerLib
{
    public static class SerialPortExtension
    {
        public const char XON = '\x11';
        public const char XOFF = '\x13';
        public static string ReadUntil(this SerialPort port, char terminator, long maxSize)
        {
            var data = new StringBuilder();
            
            long bytesRead = 0;
            port.ReadTimeout = 2000;
            try
            {
                while (bytesRead < maxSize)
                {
                    //                if (port.BytesToRead <= 0)
                    //                    break;


                    var c = (byte)port.ReadChar();
                    bytesRead++;
                    //unsigned char c = 0;
                    //if (!getChar(reinterpret_cast<char*>(&c)))
                    // Timeout
                    //    break;

                    if (c == terminator)
                        break;
                    var s = Encoding.ASCII.GetString(new byte[] { c });
                    data.Append(s);
                }
            }
            catch (TimeoutException ee)
            {
            }
            return data.ToString();
        }

        public static void MyWait(this SerialPort port)
        {
            var readStartTime = DateTime.Now;
            var timeout = TimeSpan.FromMilliseconds(port.ReadTimeout);
            while (port.BytesToRead == 0)
            {
                Thread.Sleep(10);
                if (DateTime.Now.Subtract(readStartTime) > timeout)
                    throw new Exception("Read timeout!");
            }
        }
        public static bool DownloadLine(this SerialPort port, string s, Action<string> log = null, bool verbose = false)
        {
            // Send the hex record
            // if (m_verbose)
            //     cout << "Sending '" << s.trimmed().toAscii().data() << "'" << endl;
            port.Write(s + "\n");
            //write(s.toAscii());

            // read until XON, 10 characters or timeout
            Thread.Sleep(8);
            port.MyWait();
            var r = port.ReadUntil(XON, 256); //!!!!!10
            //cout << "REPLY " << QString(r).toLatin1().data() << endl;
            // The bootloader replies with '.' on success...
            if (r.Contains("-"))
            {
                log?.Invoke($"Something went wrong during programming ({Chip45.FormatControlChars(r)})");
                return false;
            }
            if (!r.Contains(".") && !r.Contains("*"))
            {
                if (verbose)
                {
                    if (string.IsNullOrEmpty(r))
                        log?.Invoke("Timeout");
                    else
                        log?.Invoke("Reply: " + Chip45.FormatControlChars(r));
                }
                return false;
            }
            // ...and with '*' on page write
//            if (verbose && r.Contains("*"))
//                log?.Invoke("+");
            return true;
        }
    }
}
