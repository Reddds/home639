/*
Modify Arduino files:

programmers.txt
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
chip45RS485.name=Chip45 via RS485
chip45RS485.communication=serial
chip45RS485.speed=9600
chip45RS485.program.tool=chip45
chip45RS485.program.speed=9600
chip45RS485.program.extra_params=-c "\fb" -a 1000

platform.txt
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
tools.chip45.path=<Workdir!!!!!!!>
tools.chip45.cmd.path={path}/Chip45Programmer.exe
#tools.chip45.config.path={path}/etc/avrdude.conf

tools.chip45.upload.params.verbose=-v
tools.chip45.upload.params.quiet=-q
tools.chip45.upload.pattern="{cmd.path}" -p {serial.port} {upload.verbose} {program.extra_params} -r -b {upload.speed} -f "{build.path}/{build.project_name}.hex"

tools.chip45.program.params.verbose=-v
tools.chip45.program.params.quiet=-q
tools.chip45.program.pattern="{cmd.path}" -p {serial.port} {program.verbose} {program.extra_params} -r -b {program.speed} -f "{build.path}/{build.project_name}.hex"

tools.chip45.erase.params.verbose=-v
tools.chip45.erase.params.quiet=-q -q
#tools.chip45.erase.pattern="{cmd.path}" "-C{config.path}" {erase.verbose} -p{build.mcu} -c{protocol} {program.extra_params} -e -Ulock:w:{bootloader.unlock_bits}:m -Uefuse:w:{bootloader.extended_fuses}:m -Uhfuse:w:{bootloader.high_fuses}:m -Ulfuse:w:{bootloader.low_fuses}:m

tools.chip45.bootloader.params.verbose=-v
tools.chip45.bootloader.params.quiet=-q -q
#tools.chip45.bootloader.pattern="{cmd.path}" "-C{config.path}" {bootloader.verbose} -p{build.mcu} -c{protocol} {program.extra_params} "-Uflash:w:{runtime.platform.path}/bootloaders/{bootloader.file}:i" -Ulock:w:{bootloader.lock_bits}:m

*/
using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Chip45Programmer
{
    class Chip45
    {
        public enum ProgramTypes { Flash, Eeprom }
        public class ProgramOptions
        {
            public ProgramTypes ProgramType { get; set; }
            public string ProgramFile { get; set; }
            public int EepromWriteDelay { get; set; }
        }

        public class ConnectOptions
        {
            public byte[] BytesBeforeConnect { get; set; }
            public int PreStringTimeoutAfterMs { get; set; }
        }

        public class ReadEepromOptions
        {
            public string EepromSaveFileName { get; set; } 
            public int BytesToRead { get; set; }
        }

        private readonly SerialPort _port;
        private readonly bool _debug;
        private readonly bool _verbose;
        private readonly Action<string> _log;

        public bool Connected { get; private set; }

        public Chip45(SerialPort port, bool debug, bool verbose, Action<string> log)
        {
            _port = port;
            _debug = debug;
            _verbose = verbose;
            _log = log;
        }

/*        /// <summary>
        /// Если методы запускаются в одном потоке без BackgroundWorker,
        /// то выводим прогресс в консоль
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="percent"></param>
        /// <param name="msg"></param>
        private void ConsoleProgress(BackgroundWorker worker, int percent, string msg = null)
        {
            if (worker != null && worker.WorkerReportsProgress)
            {
                worker.ReportProgress(percent, msg);
                return;
            }
            Console.Write(".");
/*
            Console.Write("\r                                                                          ");
            Console.Write("\r[");
            var i = 0;
            for (; i < percent; i += 5)
            {
                Console.Write("#");
            }
            for (; i < 100; i += 5)
            {
                Console.Write(".");
            }
            Console.Write($"] {msg}");
#1#

        }*/

        /// <summary>
        /// Получаем количество миллисекунд, прошедших с определённого времени
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        private static double GetElapsedMs(DateTime startTime)
        {
            return (DateTime.Now.Subtract(startTime)).TotalMilliseconds;
        }

        public void ConnectBootloader(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
 
            var opts = (ConnectOptions) e.Argument;


            if (opts.BytesBeforeConnect != null)
            {
                if(worker != null)
                    worker.ReportProgress(0, "Send prestring...");
                else
                    _log("Send prestring...");
                _port.Write(opts.BytesBeforeConnect, 0, opts.BytesBeforeConnect.Length);
                if(opts.PreStringTimeoutAfterMs > 0)
                    Thread.Sleep(opts.PreStringTimeoutAfterMs);
            }

            // How many ms to wait for bootloader prompt
            const int initialTimeOut = 30000; //60000;
            const double progressStep = 100d / (initialTimeOut/1000d);
            
//            QTime t;
//            t.start();
//            QTime t2;
//            t2.start();
            var avail = false;

            var prompt = string.Empty;
            var connected = false;
            var gotActiveBootloader = false;
            var startTime = DateTime.Now;
            var startTime2 = DateTime.Now;
            var curProgress = 0d;

            if (worker != null)
                worker.ReportProgress(0, "Connecting...");
            else
                _log("Connecting...");

            while ((worker == null || !worker.CancellationPending) && !connected && (GetElapsedMs(startTime) < initialTimeOut))
            {
                // "After a reset the bootloader waits for approximately 2 seconds to detect a
                //  transmission at its RXD pin. If so, it will measure the timing of the rising
                //  and falling edges of four consecutive characters 'U' at the host's baud to
                //  determine its correct baud rate prescaler."

//                port->putChar('U');
//                port->putChar('U');
//                port->putChar('U');
//                port->putChar('U');
//                port->putChar('\n');
//
//
//                port->flushOutBuffer();
                _port.Write("UUUU\n");


                Thread.Sleep(100);
                //Msleep(100);
                if (_verbose && (GetElapsedMs(startTime2) > 1000))
                {
                    curProgress += progressStep;

                    if (worker != null)
                        worker.ReportProgress((int) curProgress);
                    else
                        Console.Write(".");// <<  << flush;
                    startTime2 = DateTime.Now;
                }
                //avail = port->bytesAvailable();
                avail = _port.BytesToRead > 0;
                if (avail)
                {
                    //prompt = port->readUntil(C45BSerialPort::XON, 30);
                    prompt = _port.ReadUntil(SerialPortExtension.XON, 30);
                    if (prompt.Contains("c45b2"))
                    {
                        connected = true;
                        Connected = true;
                        if (_debug)
                            //cout << "Found fresh bootloader" << endl;
                            _log("Found fresh bootloader");
                    }
                    //else if (prompt.Contains(QString("%1-\n\r>").arg(QChar(C45BSerialPort::XOFF))))
                    else if (prompt.Contains($"{SerialPortExtension.XOFF}-\n\r>"))
                    {
                        connected = true;
                        Connected = true;
                        gotActiveBootloader = true;
                        if (_debug)
                            //                            cout << "Found already activated bootloader" << endl;
                            _log("Found already activated bootloader");
                    }
                }
            }
            
            if (_debug)
                _log("Read " + prompt.Length + " bytes: " + FormatControlChars(prompt));
            if (connected && _verbose)
                _log("Connected");

            if (string.IsNullOrEmpty(prompt))
            {
                _log("Error: No initial reply from bootloader");
                e.Result = false;
                return;
            }
            if (!gotActiveBootloader && !prompt.Contains("c45b2"))
            {
                _log("Error: Wrong bootloader version: " + prompt);
                e.Result = false;
                return;
            }

            if (gotActiveBootloader)
                _log("Warning: bootloader was already active - could not check for compatible version");
            else if (_verbose)
                _log("Bootloader " + prompt.Substring(5).Trim());

            // Flush
            _port.ReadExisting();
            Thread.Sleep(10);

            _port.Write("\n");
            Thread.Sleep(100);
            _port.ReadExisting();
            e.Result = true;
        }

        public void DisconnectBootloader(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            if (worker != null)
                worker.ReportProgress(0, "Disconnecting...");
            else
                _log("Disconnecting...");
            _port.Write("g\n");
            Thread.Sleep(100);
            var resp = _port.ReadUntil(SerialPortExtension.XON, 3);
            _port.ReadExisting();
            var success = resp.Contains("g+");
            e.Result = success;
            if (success)
                _log("Bootloar disconnected");
            Connected = false;
        }

        public void ReadEeprom(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var opts = (ReadEepromOptions) e.Argument;
            var hexFile = new HexFile(_log, true);

            var readStr = new StringBuilder();
            var consoleWriteCounter = 0;
            for (var i = 0; i < opts.BytesToRead; ++i)
            {
                if (i%16 == 0)
                {
                    if (worker != null)
                        worker.ReportProgress((int) (i*100d/opts.BytesToRead), "Reading Eeprom...");
                    else
                    {
                        if (consoleWriteCounter == 80)
                        {
                            consoleWriteCounter = 0;
                            Console.Write("\n");
                        }
                        Console.Write(".");
                        consoleWriteCounter++;
                    }
                }

                string cmd = $"er{i:X4}";
                _port.Write(cmd);
                _port.Write("\n");
                var reply = _port.ReadUntil('\r', 10);
                reply = reply.Replace(SerialPortExtension.XOFF.ToString(), "").Trim();
                var expected = cmd + "+";
                if (!reply.StartsWith(expected))
                {
                    _log("Error: Bootloader did not respond to 'er' command");
                    if (_verbose)
                        _log("Reply: " + FormatControlChars(reply));
                    e.Result = false;
                    return;
                }
                reply = _port.ReadUntil('\r', 10);
                reply = reply.Trim();
                if (_verbose)
                    readStr.Append(reply + " ");
                hexFile.Add(Convert.ToByte(reply, 16));
                _port.ReadUntil(SerialPortExtension.XON, 10);
            }
            if (_verbose)
                _log(readStr.ToString());
            HexUtils.WriteHexfile(opts.EepromSaveFileName, hexFile);
            e.Result = true;
            return;
        }

        public void Program(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var opts = (ProgramOptions) e.Argument;

            var flashHexFile = new HexFile(_log, _verbose);
                if (!flashHexFile.Load(opts.ProgramFile))
                {
                    _log("Failed to load file '" + opts.ProgramFile + "': " + flashHexFile.ErrorString);
                    e.Result = false;
                    return;
                }

            var writeDelay = 0;
            if (opts.ProgramType == ProgramTypes.Eeprom)  // check hexfiles prior to doing COM stuff
            {
                //                if (opt.isSet("-ed"))
                //                    opt.get("-ed")->getInt(eepromWriteDelay);
                writeDelay = opts.EepromWriteDelay;
            }

            //            string eepromReadFilename;
            //            int eepromReadBytes = 0;

            var cmd = opts.ProgramType == ProgramTypes.Flash ? "pf" : "pe";
            _port.WriteLine(cmd);
            var reply = _port.ReadUntil('\r', 10);
            reply = reply.Replace("\x13", "").Trim();
            var expected = cmd + "+";

            if (!reply.StartsWith(expected))
            {
                _log("Error: Bootloader did not respond to '" + cmd + "' command");
                if (_verbose)
                    _log("Reply: " + FormatControlChars(reply));
                e.Result = false;
                return;
            }

            var kind = opts.ProgramType == ProgramTypes.Flash ? "flash" : "EEPROM";

            // Send to bootloader
            var msg = "Programming " + kind + " memory...";
            if (_verbose)
                _log(msg);
            worker?.ReportProgress(0, msg);

            _port.ReadExisting();
            var hexFileLines = flashHexFile.GetHexFile();
            var lineNr = 0;
            var consoleWriteCounter = 0;
            var aHandredPercentWritedToLog = false;

            foreach (var line in hexFileLines)
            {
                ++lineNr;
                var percent = (int) (lineNr*100d/hexFileLines.Length);
                
                if (worker != null)
                    worker.ReportProgress(percent, "Programming " + kind + $@" {lineNr} / {hexFileLines.Length} lines");
                else
                {
                    if (consoleWriteCounter == 75)
                    {
                        consoleWriteCounter = 0;
                        Console.Write($"{percent:####}%\n");
                        if (percent == 100)
                            aHandredPercentWritedToLog = true;
                    }
                    Console.Write(".");
                    consoleWriteCounter++;
                }
                
                if (!_port.DownloadLine(line, _log, _verbose))
                {
                    _log("Error: Failed to download line " + lineNr);
                    e.Result = false;
                    return;
                }
                if (writeDelay > 0)
                    Thread.Sleep(writeDelay);
            }
            if (!aHandredPercentWritedToLog)
            {
                Console.Write("100%");
            }
            Console.Write("\n");
            var received = _port.ReadExisting();

            if (received.Contains("-"))
            {
                _log("Something went wrong during programming");
                _log("Reply: " + FormatControlChars(received) );
                e.Result = false;
                return;
            }
            if (_verbose)
                _log("Done");
            e.Result = true;
        }




        public static string FormatControlChars(string s)
        {
            var sb = new StringBuilder();
            foreach (var c in s)
            {
                var b = Encoding.ASCII.GetBytes(new[] {c})[0];
                if (b < 0x20)
                {
                    sb.Append($"<{b:X2}>");
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
