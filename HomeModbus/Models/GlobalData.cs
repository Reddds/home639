using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeModbus.Models
{
    /// <summary>
    /// Глобальные данные всего умного дома
    /// </summary>
    class GlobalData
    {
        public DateTime Time { get; set; }

       
    }

    class RoomData
    {
        public Dictionary<string, object> Data { get; set; }  
    }
}
