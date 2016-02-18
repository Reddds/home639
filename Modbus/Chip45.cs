﻿using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Modbus
{
    class Chip45
    {
        public enum ProgramTypes { Flash, Eeprom }
        public class ProgramOptions
        {
            public ProgramTypes ProgramType { get; set; }
            public string ProgramFile { get; set; }
            public int Delay { get; set; }
        }

        public class ConnectOptions
        {
            public byte[] BytesBeforeConnect { get; set; }
            public int PreStringTimeoutAfterMs { get; set; }
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
            var worker = (BackgroundWorker) sender;
            var opts = (ConnectOptions) e.Argument;


            if (opts.BytesBeforeConnect != null)
            {
                worker.ReportProgress(0, "Send prestring...");
                _port.Write(opts.BytesBeforeConnect, 0, opts.BytesBeforeConnect.Length);
                if(opts.PreStringTimeoutAfterMs > 0)
                    Thread.Sleep(opts.PreStringTimeoutAfterMs);
            }

            // How many ms to wait for bootloader prompt
            const int initialTimeOut = 30000; //60000;
            var progressStep = 100d / (initialTimeOut/1000d);
            
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

            worker.ReportProgress(0, "Connecting...");

            while (!worker.CancellationPending && !connected && (GetElapsedMs(startTime) < initialTimeOut))
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
                    worker.ReportProgress((int) curProgress);
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
            var worker = (BackgroundWorker) sender;
            worker.ReportProgress(0, "Connecting...");
            _port.Write("g\n");
            Thread.Sleep(100);
            var resp = _port.ReadUntil(SerialPortExtension.XON, 3);
            _port.ReadExisting();
            e.Result = resp.Contains("g+");
            Connected = false;
        }


        public void Program(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var opts = (ProgramOptions) e.Argument;

            var flashHexFile = new HexFile(_log, _verbose);
                if (!flashHexFile.Load(opts.ProgramFile))
                {
                    _log("Failed to load file '" + opts.ProgramFile + "': " + flashHexFile.ErrorString);
                    e.Result = false;
                    return;
                }

            var eepromWriteDelay = 0;
            if (opts.ProgramType == ProgramTypes.Eeprom)  // check hexfiles prior to doing COM stuff
            {
//                if (opt.isSet("-ed"))
//                    opt.get("-ed")->getInt(eepromWriteDelay);
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

            // Send to bootloader
            var msg = "Programming " + (opts.ProgramType == ProgramTypes.Flash ? "flash" : "EEPROM") + " memory...";
            if (_verbose)
                _log(msg);
            worker.ReportProgress(0, msg);

            _port.ReadExisting();
            var hexFileLines = flashHexFile.GetHexFile();
            var lineNr = 0;
            foreach (var line in hexFileLines)
            {
                ++lineNr;
                worker.ReportProgress((int) (lineNr * 100d / hexFileLines.Length), $@"{lineNr} / {hexFileLines.Length} lines");
                if (!_port.DownloadLine(line))
                {
                    _log("Error: Failed to download line " + lineNr);
                    e.Result = false;
                    return;
                }
                if (opts.Delay > 0)
                    Thread.Sleep(opts.Delay);
            }

            var received = _port.ReadExisting();

            if (received.Contains("-"))
            {
                _log("Something went wrong during programming");
                _log("Reply: " + FormatControlChars(received) );
                e.Result = false;
                return;
            }
            if (_verbose)
                _log("...done");
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
