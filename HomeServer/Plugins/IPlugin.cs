using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeServer.Models;

namespace HomeServer.Plugins
{
    class PluginBase
    {
        private readonly Dictionary<string, string> _parameters;
        public string Name { get; private set; }

        public PluginBase(Dictionary<string, string> parameters)
        {
            _parameters = parameters;
        }
        /// <summary>
        /// Получение подготовленных параметров из плагина
        /// </summary>
        public virtual void GetValues(List<HomeServerSettings.ActiveValue> activeValues) { }
    }
}
