using System;
using System.Collections.Generic;
using HomeModbus.Objects;

namespace HomeModbus.Implementation
{
    class Bedroom : Room
    {
        public event EventHandler<bool> CallFromBedRoomChanged;
        //        public bool CallFromBedRoomState { get; private set; }
        public event EventHandler MakePoo;


        /// <summary>
        /// Изменилась температура или влажность 1 - температура, 2 - влажность
        /// </summary>
        public event EventHandler<Tuple<int,int>> TemperatureHymidityChanged;

        public Bedroom()
        {
            ShControllers = new List<ShController>();

            var badroomController = new BedroomController(2);
            ShControllers.Add(badroomController);
            badroomController.SetActionOnDiscreteOrCoil(true, ShController.CheckCoilStatus.OnTrue, 4, OnCall);
            badroomController.SetActionOnDiscreteOrCoil(true, ShController.CheckCoilStatus.OnTrue, 3, OnKaka, null, true);

//            badroomController.SetActionOnRegister(false, 2, OnTempHymChanged, false, TimeSpan.FromMinutes(1));
        }
        private void OnCall(ShController.ActionOnDiscreteOrCoil actionOn, bool state)
        {
            CallFromBedRoomChanged?.Invoke(this, state);
        }

        private void OnKaka(ShController.ActionOnDiscreteOrCoil actionOn, bool state)
        {
            ToLog("Покакал");
            MakePoo?.Invoke(this, EventArgs.Empty);
        }

        private void OnTempHymChanged(ushort state)
        {
            var temp = (sbyte)(state >> 8);
            var hym = (sbyte)(state & 0xFF);
            ToLog($"В спальне: Температура: {temp} *С; Влажность: {hym} %");
            TemperatureHymidityChanged?.Invoke(this, new Tuple<int, int>(temp, hym));
        }
    }
}
