using System;
using System.Collections.Generic;
using System.Threading;
using HomeServer.Models;
using Modbus.Device;

namespace HomeServer.Objects
{
    /// <summary>
    /// Контроллер (Arduino)
    /// </summary>
    public class ShController
    {
        private const int MaxDiscreteOrCoilIndex = 15;

        /// <summary>
        /// Количество ошибок обращения к контроллеру
        /// Если подряд 5 раз вылетят ошибки, то обращаемся к этому контроллеру только раз в минуту
        /// </summary>
        public int ErrorCount = 0;

        /// <summary>
        /// Время последнего опроса
        /// </summary>
        public DateTime LactAccess;

        interface IResetParameter
        {
            bool PendingReset { get; set; }
            void Reset();
        }

        #region DiscreteOrCoil

        public class ActionOnDiscreteOrCoil
        {
            private bool? _currentState;
            public bool? InitialState { get; private set; }

            public CheckCoilStatus CheckCoilStatus { get; set; }
            public ushort Index { get; set; }
            private Action<bool> CallBack { get; set; }
            public TimeSpan? CheckInterval { get; set; }

            protected ActionOnDiscreteOrCoil(ushort index, CheckCoilStatus checkCoilStatus, Action<bool> callBack,
                bool? initialState)
            {
                Index = index;
                CheckCoilStatus = checkCoilStatus;
                CallBack = callBack;
                InitialState = initialState;
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
                            CallBack(true);
                        break;
                    case CheckCoilStatus.OnFalse:
                        if (!newState)
                            CallBack(false);
                        break;
                    case CheckCoilStatus.OnBoth:
                        CallBack(newState);
                        break;
                }
                _currentState = newState;
                return true;
            }
        }

        private class ActionOnDiscrete : ActionOnDiscreteOrCoil
        {
            public ActionOnDiscrete(ushort index, CheckCoilStatus checkCoilStatus, Action<bool> callBack,
                bool? initialState)
                : base(index, checkCoilStatus, callBack, initialState)
            {
            }
        }

        internal class ActionOnCoil : ActionOnDiscreteOrCoil, IResetParameter
        {
            public bool ResetAfter { get; private set; }

            public ActionOnCoil(ushort index, CheckCoilStatus checkCoilStatus, Action<bool> callBack, bool resetAfter,
                bool? initialState)
                : base(index, checkCoilStatus, callBack, initialState)
            {
                ResetAfter = resetAfter;
            }

            public bool PendingReset { get; set; }

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

        private class ActionOnRegister : IResetParameter
        {
            private ushort? _currentValue;
            private uint? _currentLongValue;
            protected DateTime _lastCheck = DateTime.MinValue;

            /// <summary>
            /// Сбросить после чтения
            /// </summary>
            public bool ResetAfterRead { get; set; }

            public ushort Index { get; set; }
            private Action<ushort> CallBack { get; set; }
            private Action<uint> CallBackULong { get; set; }
            public TimeSpan? CheckInterval { get; set; }
            public ushort UInt16Default { get; set; }
            public uint ULongDefault { get; set; }
            /// <summary>
            /// Вызывать CallBack даже когда данные не изменились
            /// </summary>
            public bool RaiseOlwais { get; set; }
            public DataTypes RegisterType { get; set; }

            public ActionOnRegister(ushort index, DataTypes registerType, Action<ushort> callBack, Action<uint> callBackULong = null)
            {
                Index = index;
                CallBack = callBack;
                CallBackULong = callBackULong;
                RegisterType = registerType;
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
                if (!IsNeedCheck())
                {
                    return false;
                }
                if (_currentValue == newValue)
                    return false;
                _currentValue = newValue;
                _lastCheck = DateTime.Now;
                if(_currentValue == UInt16Default)
                    return false;
                CallBack?.Invoke(newValue);
                if (ResetAfterRead)
                {
                    _currentValue = UInt16Default;
                    Reset();
                }
                return true;
            }
            public bool CheckStateULong(ushort newValueHi, ushort newValueLo)
            {
                var newValue = (uint)newValueHi;
                newValue <<= 16;
                newValue |= newValueLo;
                // Проверяем, если установлен интервал проверок, то 
                if (!IsNeedCheck())
                {
                    return false;
                }
                if (_currentLongValue == newValue)
                    return false;
                _currentLongValue = newValue;
                _lastCheck = DateTime.Now;
                if (_currentLongValue == ULongDefault)
                {
                    Console.WriteLine("ULongDefault");
                    return false;
                }
                CallBackULong?.Invoke(newValue);
                if (ResetAfterRead)
                {
                    _currentLongValue = ULongDefault;
                    Reset();
                }
                return true;
            }


            public bool PendingReset { get; set; }
            public void Reset()
            {
                PendingReset = true;
            }
        }

        private class ActionOnRegisterDateTime : ActionOnRegister
        {
            private DateTime _currentDateTime;
            private Action<DateTime> CallBack { get; set; }

            public ActionOnRegisterDateTime(ushort index, Action<DateTime> callBack) : base(index, DataTypes.RdDateTime,  null)
            {
                CallBack = callBack;
            }

            public bool CheckState(ushort yearMonth, ushort daySecond, ushort hourMinute)
            {
                // Проверяем, если установлен интервал проверок, то 
                if (!IsNeedCheck())
                {
                    return false;
                }
                if (yearMonth == 0)
                    return false;
                var checkDateTime = new DateTime(
                    (yearMonth >> 8) + 1970,
                    yearMonth & 0xFF,
                    daySecond >> 8,
                    hourMinute >> 8,
                    hourMinute & 0xFF,
                    daySecond & 0xFF);
                if (_currentDateTime == checkDateTime)
                    return false;
                _currentDateTime = checkDateTime;
                _lastCheck = DateTime.Now;
                CallBack?.Invoke(checkDateTime);
                return true;
            }

            public DateTime GetDateTime()
            {
                return _currentDateTime;
            }
        }

        /*        private class ActionOnIputRegister : ActionOnRegister
                {
                    public ActionOnIputRegister(ushort index, Action<ushort> callBack) : base(index, callBack)
                    {
                    }
                }

                private class ActionOnIputRegisterDateTime : ActionOnRegisterDateTime
                {
                    public ActionOnIputRegisterDateTime(ushort index, Action<DateTime> callBack) : base(index, callBack)
                    {
                    }
                }

                private class ActionOnHoldingRegister : ActionOnRegister
                {
                    public ActionOnHoldingRegister(ushort index, Action<ushort> callBack) : base(index, callBack)
                    {
                    }
                }

                private class ActionOnHoldingRegisterDateTime
                {

                }*/

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

        private List<ActionOnRegister> _inputChecks;
        private List<ActionOnRegister> _holdingChecks;

        #endregion

        #region Setters

        public class ModbusSetter
        {
            public ushort Index;
            public SetterTypes SetterType;
            public bool IsPending { get; private set; }
            private object _pendingObject;
            private Action<bool> _resultAction;

            public ModbusSetter(ushort index, SetterTypes setterType, Action<bool> resultAction = null)
            {
                Index = index;
                SetterType = setterType;
                _resultAction = resultAction;
            }

            public void PendingSet(object newValue = null)
            {
                _pendingObject = newValue;
                IsPending = true;
            }

            /// <summary>
            /// Установка новго значения
            /// </summary>
            /// <param name="modbus"></param>
            /// <param name="slaveAddress"></param>
            /// <returns></returns>
            public bool Set(ModbusSerialMaster modbus, byte slaveAddress)
            {
                IsPending = false;
                switch (SetterType)
                {
                    case SetterTypes.RealDateTime:
                        var resultStatus = SendTime(modbus, slaveAddress);
                        _resultAction?.Invoke(resultStatus);
                        return resultStatus;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return false;
            }

            private bool SendTime(ModbusSerialMaster modbus, byte slaveAddress)
            {
                var curTime = DateTime.Now;
                var timeData = new ushort[3];
                timeData[0] = (ushort)(((curTime.Hour << 8)) | curTime.Minute);
                timeData[1] = (ushort)((curTime.Day << 8) | curTime.Second);
                timeData[2] = (ushort)(((curTime.Year % 100) << 8) | curTime.Month);

                //            _modbus.WriteMultipleRegisters(2, 8, timeData);
                modbus.WriteMultipleRegisters(slaveAddress, Index, timeData);
                Thread.Sleep(500);

                var setTimeRes = modbus.ReadInputRegisters(slaveAddress, Index, 1);
                return setTimeRes[0] == 0x0000;
                /*if (setTimeRes.Length > 0 && setTimeRes[0] == 0xffff)
                    WriteToLog?.Invoke(this, "Время установлено успешно!");
                else
                {
                    WriteToLog?.Invoke(this, $"Ошибка установки времени! ({setTimeRes[0]:X4})");
                    //MessageBox.Show($"Ошибка установки времени! ({setTimeRes[0]:X4})");
                }*/

            }

        }

        private List<ModbusSetter> _setters;


        #endregion

        public readonly string Id;
        public byte SlaveAddress;


        public ShController(string id, byte slaveAddress)
        {
            Id = id;
            SlaveAddress = slaveAddress;
        }

        public virtual void GetStatus(ModbusSerialMaster modbus)
        {
            var curOperation = string.Empty;
            try
            {
                // Дискретные
                if (_discreteChecks != null)
                {
                    curOperation = $"CheckDiscrete ";
                    var discreteStatus = modbus.ReadInputs(SlaveAddress, _discreteRegisterMinIndex,
                        (ushort)(_discreteRegisterMaxIndex - _discreteRegisterMinIndex + 1));
                    foreach (var check in _discreteChecks)
                    {
                        check.CheckState(discreteStatus[check.Index - _discreteRegisterMinIndex]);
                    }
                }
                // Катушки
                if (_coilChecks != null)
                {
                    Thread.Sleep(10);
                    curOperation = $"CheckCoil";
                    var coilStatus = modbus.ReadCoils(SlaveAddress, _coilMinIndex,
                        (ushort)(_coilMaxIndex - _coilMinIndex + 1));
                    foreach (var check in _coilChecks)
                    {

                        if (check.CheckState(coilStatus[check.Index - _coilMinIndex]))
                            if (check.ResetAfter)
                            {
                                if (check.InitialState != null)
                                    modbus.WriteSingleCoil(SlaveAddress, check.Index, check.InitialState.Value);
                            }
                    }
                }
                // Регистры
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
                        Thread.Sleep(10);
                        curOperation = $"CheckInput ";
                        var inputsStatus = modbus.ReadInputRegisters(SlaveAddress, _inputRegisterMinIndex,
                            (ushort)(_inputRegisterMaxIndex - _inputRegisterMinIndex + 1));
                        foreach (var inputCheck in _inputChecks)
                        {

                            var dateTimeCheck = inputCheck as ActionOnRegisterDateTime;
                            if (dateTimeCheck != null)
                            {
                                dateTimeCheck.CheckState(inputsStatus[dateTimeCheck.Index - _inputRegisterMinIndex],
                                    inputsStatus[dateTimeCheck.Index - _inputRegisterMinIndex + 1],
                                    inputsStatus[dateTimeCheck.Index - _inputRegisterMinIndex + 2]);
                            }
                            else
                                inputCheck.CheckState(inputsStatus[inputCheck.Index - _inputRegisterMinIndex]);
                        }
                    }
                }
                // Перезаписываемые Регистры
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
                        Thread.Sleep(10);
                        curOperation = $"CheckHolding";
                        var holdingsStatus = modbus.ReadHoldingRegisters(SlaveAddress, _holdingRegisterMinIndex,
                            (ushort)(_holdingRegisterMaxIndex - _holdingRegisterMinIndex + 1));

                        foreach (var holdingCheck in _holdingChecks)
                        {
                            switch (holdingCheck.RegisterType)
                            {
                                case DataTypes.ULong:
                                    holdingCheck.CheckStateULong(holdingsStatus[holdingCheck.Index - _holdingRegisterMinIndex], holdingsStatus[holdingCheck.Index + 1 - _holdingRegisterMinIndex]);
                                    break;
                                default:
                                    holdingCheck.CheckState(holdingsStatus[holdingCheck.Index - _holdingRegisterMinIndex]);
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception ee)
            {

                throw new Exception($"{curOperation} " + ee.Message);
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

                        if(check.InitialState != null)
                            modbus.WriteSingleCoil(SlaveAddress, check.Index, check.InitialState.Value);
                    }
                }
            }

            // Перезаписываемые Регистры
            if (_holdingChecks != null)
            {
                foreach (var check in _holdingChecks)
                {
                    if (check.PendingReset)
                    {
                        check.PendingReset = false;

                        switch (check.RegisterType)
                        {
                            case DataTypes.ULong:
                                Console.WriteLine("Reset ULong");
                                var resetData = new ushort[2];
                                resetData[0] = (ushort) (check.ULongDefault >> 16);
                                resetData[1] = (ushort) (check.ULongDefault & 0xFFFF);
                                modbus.WriteMultipleRegisters(SlaveAddress, check.Index, resetData);
                                break;
                            default:
                                modbus.WriteSingleRegister(SlaveAddress, check.Index, check.UInt16Default);
                                break;
                        }
                    }
                }
            }

            if (_setters != null)
            {
                foreach (var setter in _setters)
                {
                    if (setter.IsPending)
                    {
                        var setResult = setter.Set(modbus, SlaveAddress);

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
        /// <returns>Reset action</returns>
        public Action SetActionOnDiscreteOrCoil(bool isCoil, CheckCoilStatus checkCoilStatus, ushort index,
            Action<bool> callback,
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
                return () =>
                {
                    actionOnCoil.Reset();
                };
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
                return null;
            }
        }

        /// <summary>
        /// Добавить проверку регистра
        /// </summary>
        /// <param name="isHolding"></param>
        /// <param name="index"></param>
        /// <param name="registerType"></param>
        /// <param name="callback"></param>
        /// <param name="uLongCallback"></param>
        /// <param name="raiseOlwais">Вызывать callback даже когда показания не изменились</param>
        /// <param name="checkInterval"></param>
        /// <returns>Reset action</returns>
        public Action SetActionOnRegister(bool isHolding, ushort index, DataTypes registerType, Action<ushort> callback,
            Action<uint> uLongCallback,
            bool raiseOlwais = false, TimeSpan? checkInterval = null, bool resetAfterRead = false, ushort uInt16Default = 0, uint uLongDefault = 0)
        {
            if (callback == null && uLongCallback == null)
                throw new ArgumentNullException(nameof(callback) + " and " + nameof(uLongCallback));
            var endIndex = index;
            switch (registerType)
            {
                case DataTypes.UInt16:
                    break;
                case DataTypes.Float:
                    break;
                case DataTypes.ULong:
                    endIndex = (ushort) (index + 1);
                    break;
                case DataTypes.RdDateTime:
                    break;
                case DataTypes.RdTime:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(registerType), registerType, null);
            }
            if (!isHolding)
            {
                if (_inputRegisterMinIndex > index)
                    _inputRegisterMinIndex = index;
                if (_inputRegisterMaxIndex < endIndex)
                    _inputRegisterMaxIndex = endIndex;
                if (_inputChecks == null)
                    _inputChecks = new List<ActionOnRegister>();

                var holdingCheck = new ActionOnRegister(index, registerType, callback, uLongCallback)
                {
                    RaiseOlwais = raiseOlwais,
                    CheckInterval = checkInterval
                };
                _inputChecks.Add(holdingCheck);

                return () =>
                {
                    holdingCheck.Reset();
                };
            }
            else
            {
                if (_holdingRegisterMinIndex > index)
                    _holdingRegisterMinIndex = index;
                if (_holdingRegisterMaxIndex < endIndex)
                    _holdingRegisterMaxIndex = endIndex;
                if (_holdingChecks == null)
                    _holdingChecks = new List<ActionOnRegister>();
                _holdingChecks.Add(new ActionOnRegister(index, registerType, callback, uLongCallback) { RaiseOlwais = raiseOlwais, CheckInterval = checkInterval, ResetAfterRead = resetAfterRead, ULongDefault = uLongDefault });
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isHolding"></param>
        /// <param name="index"></param>
        /// <param name="callback"></param>
        /// <param name="raiseOlwais"></param>
        /// <param name="checkInterval"></param>
        /// <returns>Reset action</returns>
        public Action SetActionOnRegisterDateTime(bool isHolding, ushort index, Action<DateTime> callback,
            bool raiseOlwais = false, TimeSpan? checkInterval = null)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
            var endIndex = (ushort)(index + 2);
            if (!isHolding)
            {
                if (_inputRegisterMinIndex > index)
                    _inputRegisterMinIndex = index;
                if (_inputRegisterMaxIndex < endIndex)
                    _inputRegisterMaxIndex = endIndex;
                if (_inputChecks == null)
                    _inputChecks = new List<ActionOnRegister>();
                var holdingCheck = new ActionOnRegisterDateTime(index, callback)
                {
                    RaiseOlwais = raiseOlwais,
                    CheckInterval = checkInterval
                };
                _inputChecks.Add(holdingCheck);
                return () =>
                {
                    holdingCheck.Reset();
                };
            }
            else
            {
                if (_holdingRegisterMinIndex > index)
                    _holdingRegisterMinIndex = index;
                if (_holdingRegisterMaxIndex < endIndex)
                    _holdingRegisterMaxIndex = endIndex;
                if (_holdingChecks == null)
                    _holdingChecks = new List<ActionOnRegister>();
                _inputChecks.Add(new ActionOnRegisterDateTime(index, callback)
                {
                    RaiseOlwais = raiseOlwais, CheckInterval = checkInterval
                });
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="result">При установки параметра возвращаем правильно ли установилось или нет</param>
        /// <returns></returns>
        public ModbusSetter SetSetter(ushort index, SetterTypes type, Action<bool> result = null)
        {
            if (_setters == null)
                _setters = new List<ModbusSetter>();
            var newSetter = new ModbusSetter(index, type, result);
            _setters.Add(newSetter);
            return newSetter;
        }

    }
}
