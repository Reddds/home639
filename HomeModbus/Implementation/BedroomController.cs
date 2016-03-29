using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HomeModbus.Objects;
using Modbus.Device;

namespace HomeModbus.Implementation
{
    class BedroomController : ShController
    {

        public override void GetStatus(ModbusSerialMaster modbus)
        {
            base.GetStatus(modbus);
        }

        public BedroomController(byte slaveAddress, string id) : base(id, slaveAddress)
        {
        }
    }
}
