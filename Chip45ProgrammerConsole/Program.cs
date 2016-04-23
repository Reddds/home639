using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chip45ProgrammerLib;
using CommandLineParser.Exceptions;

namespace Chip45ProgrammerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
//            if (args.Length <= 1)
//            {
//                Console.WriteLine("Need parameters");
//                Environment.ExitCode = 1;
//                return;
//            }


            var options = new Options();
            //ConsoleManager.Show();
            var parser = new CommandLineParser.CommandLineParser { AcceptSlash = false };
            //switch argument is meant for true/false logic

            parser.ExtractArgumentAttributes(options);
            parser.ShowUsageHeader = "Tool for communicating with the Chip45 bootloader\n"
                + "Example: c45b -p COM3 -b 57600 -c \"R!\" -f avrblink.hex -e avrblink.eep -r\n";

            try
            {
                parser.ParseCommandLine(args);
                //                if (options.ShowVersion)
                //                {
                //                    var app = Assembly.GetExecutingAssembly();
                //                    Console.WriteLine("\nChip45Programmer version " + app.GetName().Version);
                //                    Console.WriteLine("Based on https://github.com/bullestock/c45b");
                //                    Console.WriteLine("Copyright " + ((AssemblyCopyrightAttribute)app.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright);
                //                }


                parser.ShowParsedArguments();

                var doFlash = !string.IsNullOrEmpty(options.FlashFile);
                var doEeprom = !string.IsNullOrEmpty(options.EepromFile);
                var doEepromRead = !string.IsNullOrEmpty(options.EepromReadFile);
                var sendAppCmd = !string.IsNullOrEmpty(options.StringCommand);

                if (!doFlash && !doEeprom && !doEepromRead)
                {
                    Console.WriteLine("Neither -f nor -e nor -er specified - nothing to do");
                    if (sendAppCmd) // starting without further options can be used to halt or reset device
                    {
                        // but only if running application device reacts on app command.
                        if (options.RunApp)
                            Console.WriteLine("Just resetting device");
                        else
                            Console.WriteLine("Just entering bootloader");
                    }
                    else
                    {
                        Environment.ExitCode = 1;
                        return;
                    }
                }

                if ((doFlash || doEeprom) && doEepromRead)
                {
                    Console.WriteLine("You may only specify read or write commands.");
                    Environment.ExitCode = 1;
                    return;
                }

                if (doEepromRead && options.EepromReadBytes <= 0)
                {
                    Console.WriteLine("Pleas select number of bytes to read from EEPROM (-y/--eepromreadbytes)");
                    Environment.ExitCode = 1;
                    return;
                }

                /*
                                HexFile flashHexFile;
                                if (doFlash)  // check hexfiles prior to doing COM stuff
                                {
                                    flashHexFile = new HexFile(Console.WriteLine, options.Verbose);
                                    if (!flashHexFile.Load(options.FlashFile))
                                    {
                                        Console.WriteLine($"Failed to load file '{options.FlashFile}': {flashHexFile.ErrorString}");
                                        ExitResult(1);
                                        return;
                                    }
                                }

                                HexFile eepHexFile;
                                if (doEeprom)  // check hexfiles prior to doing COM stuff
                                {
                                    eepHexFile = new HexFile(Console.WriteLine, options.Verbose);
                                    if (!eepHexFile.Load(options.EepromFile))
                                    {
                                        Console.WriteLine($"Failed to load file '{options.EepromFile}': {eepHexFile.ErrorString}");
                                        ExitResult(1);
                                        return;
                                    }
                                }

                */

                byte[] appCmd = null;
                if (sendAppCmd)
                {
                    appCmd = ParseAppCmd(options.StringCommand);
                    if (appCmd == null)
                    {
                        Console.WriteLine("Could not parse application command. Please check format and escape sequences");
                        Environment.ExitCode = 1;
                        return;
                    }
                }

                var port = new SerialPort(options.SerialPort, options.BaudRate)
                {
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                };
                port.Open();
                try
                {
                    var chip45 = new Chip45(port, true, true, Console.WriteLine);
                    var arg = new Chip45.ConnectOptions
                    {
                        BytesBeforeConnect = appCmd,
                        PreStringTimeoutAfterMs = options.DelayAfterPrestring
                    };
                    chip45.ConnectBootloader(null, new DoWorkEventArgs(arg));
                    if (!chip45.Connected)
                    {
                        Console.WriteLine("Not connected!");
                        Environment.ExitCode = 1;
                        return;
                    }


                    if (doEepromRead)
                    {
                        var eeReadArg = new Chip45.ReadEepromOptions
                        {
                            EepromSaveFileName = options.EepromReadFile,
                            BytesToRead = options.EepromReadBytes
                        };
                        chip45.ReadEeprom(null, new DoWorkEventArgs(eeReadArg));
                    }

                    if (doFlash)
                    {
                        var flashArg = new Chip45.ProgramOptions
                        {
                            ProgramType = Chip45.ProgramTypes.Flash,
                            ProgramFile = options.FlashFile
                        };

                        chip45.Program(null, new DoWorkEventArgs(flashArg));
                    }
                    if (doEeprom)
                    {
                        var eeArg = new Chip45.ProgramOptions
                        {
                            ProgramType = Chip45.ProgramTypes.Eeprom,
                            ProgramFile = options.EepromFile,
                            EepromWriteDelay = options.EepromDelay
                        };
                        chip45.Program(null, new DoWorkEventArgs(eeArg));
                    }

                    if (options.RunApp)
                    {
                        chip45.DisconnectBootloader(null, new DoWorkEventArgs(null));
                    }
                }
                catch (Exception ee)
                {
                    port.Close();
                    if(options.Verbose)
                        Console.WriteLine("Error! " + ee);
                    else
                        Console.WriteLine("Error! " + ee.Message);
                }


            }
            catch (CommandLineException ee)
            {
                Console.WriteLine(ee.Message);
            }

            Environment.ExitCode = 0;

        }

        static byte[] ParseAppCmd(string iStr)
        {
            const string specialCharsSym = "tnr\\";
            var specialChars = new byte[] { 0x09, 0x0a, 0x0d, 0x5c };
            const string hexChars = "0123456789abcdefABCDEF";
            var hexVals = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 10, 11, 12, 13, 14, 15 };

            var byteRes = new List<byte>();
            for (var it = 0; it < iStr.Length; it++)
            {
                var c = iStr[it];
                if (c == '\\')
                {
                    ++it;
                    if (it == iStr.Length)
                        return null;
                    c = iStr[it];
                    var pos = specialCharsSym.IndexOf(c);
                    if (pos >= 0)
                        byteRes.Add(specialChars[pos]);
                    else
                    {
                        byte databyte = 0;
                        pos = hexChars.IndexOf(c);
                        if (pos < 0)
                            return null;
                        databyte = (byte)(hexVals[pos] << 4);
                        ++it;
                        if (it == iStr.Length)
                            return null;
                        c = iStr[it];
                        pos = hexChars.IndexOf(c);
                        if (pos < 0)
                            return null;
                        databyte |= hexVals[pos];

                        byteRes.Add(databyte);
                    }
                }
                else
                    byteRes.Add(Encoding.ASCII.GetBytes(new[] { c })[0]);
            }
            return byteRes.ToArray();
        }

    }
}
