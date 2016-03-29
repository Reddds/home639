using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeModbus.Objects;

namespace HomeModbus.Implementation
{
    class Bathroom : ControllerGroup
    {
        public event EventHandler<bool> BathDoorChanged;
//        public bool BathDoorState { get; private set; }

        public event EventHandler<bool> BathLightChanged;
//        public bool BathLightState { get; private set; }

        DateTime _bathLightStart = DateTime.Now;


        public Bathroom()
        {
            ShControllers = new List<ShController>();
            var bathroomController = new ShController(3);
            ShControllers.Add(bathroomController);
            bathroomController.SetActionOnDiscreteOrCoil(false, ShController.CheckCoilStatus.OnBoth, 0, OnDoor);
            bathroomController.SetActionOnDiscreteOrCoil(false, ShController.CheckCoilStatus.OnBoth, 1, OnLight);
        }

        private void OnDoor(ShController.ActionOnDiscreteOrCoil actionOn, bool state)
        {
            BathDoorChanged?.Invoke(this, state);
        }

        private void OnLight(ShController.ActionOnDiscreteOrCoil actionOn, bool state)
        {
            BathLightChanged?.Invoke(this, state);
            if (state)
            {
                _bathLightStart = DateTime.Now;
            }
            else
            {
                ToLog($"Свет в ванне горел {DateTime.Now.Subtract(_bathLightStart).ToString(@"dd' Дней 'hh\:mm\:ss")}");
            }
        }
    }
}
