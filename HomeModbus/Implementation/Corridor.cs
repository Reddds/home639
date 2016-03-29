using System;
using System.Collections.Generic;
using HomeModbus.Objects;

namespace HomeModbus.Implementation
{
    class Corridor : ControllerGroup
    {
        public event EventHandler<bool> LightInCorridorChanged;
        public bool LightInCorridorState { get; private set; }

        public Corridor()
        {
            ShControllers = new List<ShController>();

            var corridorController = new ShController(1);
            ShControllers.Add(corridorController);
            corridorController.SetActionOnDiscreteOrCoil(false, ShController.CheckCoilStatus.OnBoth, 3, OnLight);

        }

        private void OnLight(ShController.ActionOnDiscreteOrCoil actionOn, bool lightState)
        {
            LightInCorridorChanged?.Invoke(this, lightState);
        }
    }
}
