using System.IO;

namespace Modbus
{
    static class HexUtils
    {
        public static bool WriteHexfile(string fileName, HexFile hf)
        {
            File.WriteAllLines(fileName, hf.GetHexFile());
            //    QFile out(filename);
            //    if (!out.open(QIODevice::WriteOnly))
            //    {
            //        cout << "Could not write file '" << filename.data() << "'" << endl;
            //        return false;
            //    }
            //    out.write(hf.getHexFile().join("").toLocal8Bit());
            //    out.close();
            return true;
        }
    }
}
