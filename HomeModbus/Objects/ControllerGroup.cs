using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeModbus.Objects
{
    /// <summary>
    /// Комната
    /// </summary>
    class ControllerGroup
    {
        public event EventHandler<string> WriteToLog;
        public List<ShController> ShControllers;

        public void ToLog(string msg)
        {
            WriteToLog?.Invoke(this, msg);
        }
    }
}
