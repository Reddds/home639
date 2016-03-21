using System.Collections.Generic;
using System.Windows;
using CommandLineParser.Arguments;

namespace Chip45Programmer
{
    // fields of this class will be bound
    class Options
    {
        //class has several fields and properties bound to various argument types

        [ValueArgument(typeof(string), 'p', "port", Description = "Serial port (without colon)", Optional = false)]
        public string SerialPort;

        [ValueArgument(typeof(int), 'b', "baud", Description = "Baud rate (default:9600)", DefaultValue = 9600)]
        public int BaudRate;

        [ValueArgument(typeof(string), 'f', "flash", Description = "Program flash memory file")]
        public string FlashFile;

        [ValueArgument(typeof(string), 'e', "eeprom", Description = "Program EEPROM file")]
        public string EepromFile;

        [ValueArgument(typeof(int), 'd', "eepromdelay", Description = "Delay (in ms) to wait between sending two lines of EEPROM data.\nSet or increase this if writing EEPROM fails.", DefaultValue = 0)]
        public int EepromDelay;

        [ValueArgument(typeof(string), 'R', "eepromread", Description = "Read EEPROM from device\nUsage: -r destination.hex")]
        public string EepromReadFile;

        [ValueArgument(typeof(int), 'y', "eepromreadbytes", Description = "Bytes to read from EEPROM", DefaultValue = 0)]
        public int EepromReadBytes;


        [SwitchArgument('r', "runapp", false, Description = "Start application/leave bootloader on exit")]
        public bool RunApp;

        [SwitchArgument('v', "verbose", false, Description = "Be verbose")]
        public bool Verbose;

        [SwitchArgument('q', "quiet", false, Description = "Be quiet")]
        public bool Quiet;

        [ValueArgument(typeof(string), 'c', "appcmd", Description = "Command that should be sent to device before the attempts to enter the bootloader.\n"
            + "Embed the command in double quotes and escape special characters with backslash and two hex digits. For example: \\07\\22 for bell and "
            + "double quote character. The following escape sequences may be used:\n"
            + "\\\\ \\t \\n \\r")]
        public string StringCommand;

        [ValueArgument(typeof(int), 'a', "delayafterprestring", Description = "Delay (in ms) after sending prestring.", DefaultValue = 0)]
        public int DelayAfterPrestring;

    }
}
