﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Modbus.Device;

namespace HomeModbus.Objects
{
    /// <summary>
    /// Контроллер (Arduino)
    /// </summary>
    class ShController
    {
        private const int MaxDiscreteOrCoilIndex = 15;

        #region DiscreteOrCoil
        public enum CheckCoilStatus { OnTrue, OnFalse, OnBoth }

        internal class ActionOnDiscreteOrCoil
        {
            private bool? _currentState;

            public CheckCoilStatus CheckCoilStatus { get; set; }
            public ushort Index { get; set; }
            private Action<ActionOnDiscreteOrCoil, bool> CallBack { get; set; }
            public TimeSpan? CheckInterval { get; set; }

            protected ActionOnDiscreteOrCoil(ushort index, CheckCoilStatus checkCoilStatus, Action<ActionOnDiscreteOrCoil, bool> callBack,
                bool? initialState)
            {
                Index = index;
                CheckCoilStatus = checkCoilStatus;
                CallBack = callBack;

                _currentState = initialState;
            }

            public bool CheckState(bool newState)
            {
                if (newState == _currentState)
                    return false;
                switch (CheckCoilStatus)
                {
                    case CheckCoilStatus.OnTrue:
                        if (newState)
                            CallBack(this, true);
                        break;
                    case CheckCoilStatus.OnFalse:
                        if (!newState)
                            CallBack(this, false);
                        break;
                    case CheckCoilStatus.OnBoth:
                        CallBack(this, newState);
                        break;
                }
                _currentState = newState;
                return true;
            }
        }

        private class ActionOnDiscrete : ActionOnDiscreteOrCoil
        {
            public ActionOnDiscrete(ushort index, CheckCoilStatus checkCoilStatus, Action<ActionOnDiscreteOrCoil, bool> callBack,
                bool? initialState)
                : base(index, checkCoilStatus, callBack, initialState)
            {
            }
        }

        internal class ActionOnCoil : ActionOnDiscreteOrCoil
        {
            public bool ResetAfter { get; private set; }
            public bool PendingReset;

            public ActionOnCoil(ushort index, CheckCoilStatus checkCoilStatus, Action<ActionOnDiscreteOrCoil, bool> callBack, bool resetAfter,
                bool? initialState)
                : base(index, checkCoilStatus, callBack, initialState)
            {
                ResetAfter = resetAfter;
            }

            public void Reset()
            {
                PendingReset = true;
            }
        }

        /// <summary>
        /// Минимальный индекс дискретного регистра для проверки
        /// </summary>
        private ushort _discreteRegisterMinIndex = MaxDiscreteOrCoilIndex;
        /// <summary>
        /// Максимальный индекс дискретного регистра для проверки
        /// </summary>
        private ushort _discreteRegisterMaxIndex;

        /// <summary>
        /// Минимальный индекс катушки для проверки
        /// </summary>
        private ushort _coilMinIndex = MaxDiscreteOrCoilIndex;
        /// <summary>
        /// Максимальный индекс катушки для проверки
        /// </summary>
        private ushort _coilMaxIndex;

        private List<ActionOnDiscrete> _discreteChecks;
        private List<ActionOnCoil> _coilChecks;

        #endregion

        #region Registers

        private class ActionOnRegister
        {
            private ushort? _currentValue;
            private DateTime _lastCheck = DateTime.MinValue;

            public ushort Index { get; set; }
            private Action<ushort> CallBack { get; set; }
            public TimeSpan? CheckInterval { get; set; }
            /// <summary>
            /// Вызывать CallBack даже когда данные не изменились
            /// </summary>
            public bool RaiseOlwais { get; set; }

            public ActionOnRegister(ushort index, Action<ushort> callBack)
            {
                Index = index;
                CallBack = callBack;
            }

            /// <summary>
            /// надо ли проверять сейчас
            /// </summary>
            /// <returns></returns>
            public bool IsNeedCheck()
            {
                if (CheckInterval == null)
                    return true;
                if (DateTime.Now.Subtract(_lastCheck) >= CheckInterval)
                    return true;
                return false;
            }

            public bool CheckState(ushort newValue)
            {
                // Проверяем, если установлен интервал проверок, то 
                if (CheckInterval != null)
                {
                    if (DateTime.Now.Subtract(_lastCheck) < CheckInterval)
                        return false;
                }
                if (_currentValue == newValue)
                    return false;
                _currentValue = newValue;
                _lastCheck = DateTime.Now;
                CallBack?.Invoke(newValue);
                return true;
            }
        }

        private class ActionOnIputRegister : ActionOnRegister
        {
            public ActionOnIputRegister(ushort index, Action<ushort> callBack) : base(index, callBack)
            {
            }
        }

        private class ActionOnHoldingRegister : ActionOnRegister
        {
            public ActionOnHoldingRegister(ushort index, Action<ushort> callBack) : base(index, callBack)
            {
            }
        }

        /// <summary>
        /// Минимальный индекс входного регистра для проверки
        /// </summary>
        private ushort _inputRegisterMinIndex = MaxDiscreteOrCoilIndex;

        /// <summary>
        /// Максимальный индекс входного регистра для проверки
        /// </summary>
        private ushort _inputRegisterMaxIndex;

        /// <summary>
        /// Минимальный индекс содержвательного регистра для проверки
        /// </summary>
        private ushort _holdingRegisterMinIndex = MaxDiscreteOrCoilIndex;

        /// <summary>
        /// Максимальный индекс содержвательного регистра для проверки
        /// </summary>
        private ushort _holdingRegisterMaxIndex;

        private List<ActionOnIputRegister> _inputChecks;
        private List<ActionOnHoldingRegister> _holdingChecks;

        #endregion



        public byte SlaveAddress;


        public ShController(byte slaveAddress)
        {
            SlaveAddress = slaveAddress;
        }

        public virtual void GetStatus(ModbusSerialMaster modbus)
        {
            if (_discreteChecks != null)
            {
                var discreteStatus = modbus.ReadInputs(SlaveAddress, _discreteRegisterMinIndex,
                    (ushort)(_discreteRegisterMaxIndex - _discreteRegisterMinIndex + 1));
                foreach (var check in _discreteChecks)
                {
                    check.CheckState(discreteStatus[check.Index - _discreteRegisterMinIndex]);
                }
            }
            if (_coilChecks != null)
            {
                var coilStatus = modbus.ReadCoils(SlaveAddress, _coilMinIndex,
                    (ushort)(_coilMaxIndex - _coilMinIndex + 1));
                foreach (var check in _coilChecks)
                {
                    if (check.CheckState(coilStatus[check.Index - _coilMinIndex]))
                        if (check.ResetAfter)
                        {
                            switch (check.CheckCoilStatus)
                            {
                                case CheckCoilStatus.OnTrue:
                                    modbus.WriteSingleCoil(SlaveAddress, check.Index, false);
                                    break;
                                case CheckCoilStatus.OnFalse:
                                    modbus.WriteSingleCoil(SlaveAddress, check.Index, true);
                                    break;
                            }
                        }
                }
            }

            if (_inputChecks != null)
            {
                var needCheck = false;
                foreach (var inputCheck in _inputChecks)
                {
                    if (inputCheck.IsNeedCheck())
                    {
                        needCheck = true;
                        break;
                    }
                }
                if (needCheck)
                {
                    var inputsStatus = modbus.ReadInputRegisters(SlaveAddress, _inputRegisterMinIndex,
                        (ushort)(_inputRegisterMaxIndex - _inputRegisterMinIndex + 1));
                    foreach (var inputCheck in _inputChecks)
                    {
                        inputCheck.CheckState(inputsStatus[inputCheck.Index - _inputRegisterMinIndex]);
                    }
                }
            }
            if (_holdingChecks != null)
            {
                var needCheck = false;
                foreach (var holdingCheck in _holdingChecks)
                {
                    if (holdingCheck.IsNeedCheck())
                    {
                        needCheck = true;
                        break;
                    }
                }
                if (needCheck)
                {
                    var holdingsStatus = modbus.ReadInputRegisters(SlaveAddress, _holdingRegisterMinIndex,
                        (ushort)(_holdingRegisterMaxIndex - _holdingRegisterMinIndex + 1));
                    foreach (var holdingCheck in _holdingChecks)
                    {
                        holdingCheck.CheckState(holdingsStatus[holdingCheck.Index - _holdingRegisterMinIndex]);
                    }
                }
            }
        }

        /// <summary>
        /// Действия с шиной
        /// </summary>
        /// <param name="modbus"></param>
        public void DoActions(ModbusSerialMaster modbus)
        {
            if (_coilChecks != null)
            {
                foreach (var check in _coilChecks)
                {
                    if (check.PendingReset)
                    {
                        check.PendingReset = false;
                        switch (check.CheckCoilStatus)
                        {
                            case CheckCoilStatus.OnTrue:
                                modbus.WriteSingleCoil(SlaveAddress, check.Index, false);
                                break;
                            case CheckCoilStatus.OnFalse:
                                modbus.WriteSingleCoil(SlaveAddress, check.Index, true);
                                break;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Прописывает метод, который будет вызван после изменения статуса дискретного регистра или катушки
        /// </summary>
        /// <param name="isCoil"></param>
        /// <param name="checkCoilStatus">При каком событии вызывать метод</param>
        /// <param name="index">Индекс регистра или катушки</param>
        /// <param name="callback">Метод, который будет вызываться</param>
        /// <param name="initialState">Начальное состояние</param>
        /// <param name="resetAfter">Сброс значения только для катушек и checkCoilStatus != OnBoth</param>
        public ActionOnDiscreteOrCoil SetActionOnDiscreteOrCoil(bool isCoil, CheckCoilStatus checkCoilStatus, ushort index, Action<ActionOnDiscreteOrCoil, bool> callback,
            bool? initialState = null, bool resetAfter = false)
        {
            if (index > MaxDiscreteOrCoilIndex)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
            if (!isCoil && resetAfter)
            {
                throw new ArgumentException("Сброс дискретного регистра невозможен!", nameof(resetAfter));
            }
            if (checkCoilStatus == CheckCoilStatus.OnBoth && resetAfter)
            {
                throw new ArgumentException("Сброс при проверке статуса OnBoth невозможен!", nameof(resetAfter));
            }

            if (isCoil)
            {
                if (_coilMinIndex > index)
                    _coilMinIndex = index;
                if (_coilMaxIndex < index)
                    _coilMaxIndex = index;
                if (_coilChecks == null)
                    _coilChecks = new List<ActionOnCoil>();
                var actionOnCoil = new ActionOnCoil(index, checkCoilStatus, callback, resetAfter, initialState);
                _coilChecks.Add(actionOnCoil);
                return actionOnCoil;
            }
            else
            {
                if (_discreteRegisterMinIndex > index)
                    _discreteRegisterMinIndex = index;
                if (_discreteRegisterMaxIndex < index)
                    _discreteRegisterMaxIndex = index;
                if (_discreteChecks == null)
                    _discreteChecks = new List<ActionOnDiscrete>();
                var actionOnDiscrete = new ActionOnDiscrete(index, checkCoilStatus, callback, initialState);
                _discreteChecks.Add(actionOnDiscrete);
                return actionOnDiscrete;
            }
        }

        /// <summary>
        /// Добавить проверку регистра
        /// </summary>
        /// <param name="isHolding"></param>
        /// <param name="index"></param>
        /// <param name="callback"></param>
        /// <param name="raiseOlwais">Вызывать callback даже когда показания не изменились</param>
        /// <param name="checkInterval"></param>
        public void SetActionOnRegister(bool isHolding, ushort index, Action<ushort> callback, bool raiseOlwais = false, TimeSpan? checkInterval = null)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
            if (!isHolding)
            {
                if (_inputRegisterMinIndex > index)
                    _inputRegisterMinIndex = index;
                if (_inputRegisterMaxIndex < index)
                    _inputRegisterMaxIndex = index;
                if (_inputChecks == null)
                    _inputChecks = new List<ActionOnIputRegister>();
                _inputChecks.Add(new ActionOnIputRegister(index, callback) { RaiseOlwais = raiseOlwais, CheckInterval = checkInterval });
            }
            else
            {
                if (_holdingRegisterMinIndex > index)
                    _holdingRegisterMinIndex = index;
                if (_holdingRegisterMaxIndex < index)
                    _holdingRegisterMaxIndex = index;
                if (_holdingChecks == null)
                    _holdingChecks = new List<ActionOnHoldingRegister>();
                _holdingChecks.Add(new ActionOnHoldingRegister(index, callback) { RaiseOlwais = raiseOlwais, CheckInterval = checkInterval });
            }
        }

    }
}