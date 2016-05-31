using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ExpressionEvaluator;
using HomeServer.ModbusCustomMessages;
using HomeServer.Models;
using Modbus.Device;
using Modbus.IO;
using Newtonsoft.Json;


namespace HomeServer.Objects
{
    /// <summary>
    /// Контроллер (Arduino)
    /// </summary>
    public class ShController
    {
        private const int ModbusResultSuccess = 0x8080;

        private const int MaxDiscreteOrCoilIndex = 15;
        private const int MaxDeviceStatusIndex = 7;
        private const ushort CommandHoldingRegister = 0;


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

            public CheckBoolStatus CheckBoolStatus { get; set; }
            public ushort Index { get; set; }
            private Action<bool> CallBack { get; set; }
            public TimeSpan? CheckInterval { get; set; }

            protected ActionOnDiscreteOrCoil(ushort index, CheckBoolStatus checkBoolStatus, Action<bool> callBack,
                bool? initialState)
            {
                Index = index;
                CheckBoolStatus = checkBoolStatus;
                CallBack = callBack;
                InitialState = initialState;
                _currentState = initialState;
            }

            public bool CheckState(bool newState)
            {
                if (newState == _currentState)
                    return false;
                switch (CheckBoolStatus)
                {
                    case CheckBoolStatus.OnTrue:
                        if (newState)
                            CallBack(true);
                        break;
                    case CheckBoolStatus.OnFalse:
                        if (!newState)
                            CallBack(false);
                        break;
                    case CheckBoolStatus.OnBoth:
                        CallBack(newState);
                        break;
                }
                _currentState = newState;
                return true;
            }
        }

        private class ActionOnDiscrete : ActionOnDiscreteOrCoil
        {
            public ActionOnDiscrete(ushort index, CheckBoolStatus checkBoolStatus, Action<bool> callBack,
                bool? initialState)
                : base(index, checkBoolStatus, callBack, initialState)
            {
            }
        }

        internal class ActionOnCoil : ActionOnDiscreteOrCoil, IResetParameter
        {
            public bool ResetAfter { get; private set; }

            public ActionOnCoil(ushort index, CheckBoolStatus checkBoolStatus, Action<bool> callBack, bool resetAfter,
                bool? initialState)
                : base(index, checkBoolStatus, callBack, initialState)
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
            public ushort? UInt16Default { get; set; }
            public uint? ULongDefault { get; set; }
            /// <summary>
            /// Вызывать CallBack даже когда данные не изменились
            /// </summary>
            public bool RaiseOlwais { get; set; }
            public HomeServerSettings.ControllerGroup.Controller.DataTypes RegisterType { get; set; }

            public ActionOnRegister(ushort index, HomeServerSettings.ControllerGroup.Controller.DataTypes registerType, Action<ushort> callBack, Action<uint> callBackULong = null)
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
                if (UInt16Default != null && _currentValue == UInt16Default.Value)
                    return false;
                CallBack?.Invoke(newValue);
                if (ResetAfterRead)
                {
                    _currentValue = UInt16Default ?? 0;
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

            public ActionOnRegisterDateTime(ushort index, Action<DateTime> callBack) : base(index, HomeServerSettings.ControllerGroup.Controller.DataTypes.RdDateTime, null)
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

        #region Others

        private class ActionOnReceiveSlaveId
        {
            private string _slaveId = null;

            private Action<string> CallBack { get; set; }

            public ActionOnReceiveSlaveId(Action<string> callBack)
            {
                CallBack = callBack;
            }

            /// <summary>
            /// надо ли проверять сейчас
            /// </summary>
            /// <returns></returns>
            public bool IsNeedCheck()
            {
                return _slaveId == null;
            }

            public void SendValue(string str)
            {
                _slaveId = str;
                CallBack?.Invoke(str);
            }
        }

        private ActionOnReceiveSlaveId _actionOnReceiveSlaveId = null;


        private class ActionOnDeviceStatus
        {
            private bool? _currentState;
            public bool? InitialState { get; private set; }
            public ushort Index { get; set; }
            public CheckBoolStatus CheckBoolStatus { get; set; }
            private Action<bool> CallBack { get; set; }
            public TimeSpan? CheckInterval { get; set; }

            public ActionOnDeviceStatus(ushort index, CheckBoolStatus checkBoolStatus, Action<bool> callBack,
                bool? initialState)
            {
                Index = index;
                CheckBoolStatus = checkBoolStatus;
                CallBack = callBack;
                InitialState = initialState;
                _currentState = initialState;
            }

            public bool CheckState(bool newState)
            {
                if (newState == _currentState)
                    return false;
                switch (CheckBoolStatus)
                {
                    case CheckBoolStatus.OnTrue:
                        if (newState)
                            CallBack(true);
                        break;
                    case CheckBoolStatus.OnFalse:
                        if (!newState)
                            CallBack(false);
                        break;
                    case CheckBoolStatus.OnBoth:
                        CallBack(newState);
                        break;
                }
                _currentState = newState;
                return true;
            }
        }
        /// <summary>
        /// Минимальный индекс дискретного регистра для проверки
        /// </summary>
        private byte _deviceStatusMinIndex = MaxDeviceStatusIndex;
        /// <summary>
        /// Максимальный индекс дискретного регистра для проверки
        /// </summary>
        private byte _deviceStatusMaxIndex;

        private List<ActionOnDeviceStatus> _actionsOnDeviceStatus = null;
        #endregion Others

        #region Setters

        public class ModbusSetter
        {
            public ushort Index;
            public HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes SetterType;
            public bool IsPending { get; private set; }
            private object _pendingObject;
            private readonly HomeServerSettings.ControllerGroup.Controller.Setter _setter;
            private readonly Action<bool> _resultAction;

            public ModbusSetter(HomeServerSettings.ControllerGroup.Controller.Setter setter, Action<bool> resultAction = null)
            {
                Index = setter.ModbusIndex;
                SetterType = setter.Type;
                _setter = setter;
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
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.RealDateTime:
                        var resultStatus = SendTime(modbus, slaveAddress);
                        _resultAction?.Invoke(resultStatus);
                        return resultStatus;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.UInt16:
                        var resultUInt16Status = SendUInt16(modbus, slaveAddress);
                        _resultAction?.Invoke(resultUInt16Status);
                        return resultUInt16Status;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.MultipleUInt16:
                        var resultMultipleUInt16Status = SendMultipleUInt16(modbus, slaveAddress);
                        _resultAction?.Invoke(resultMultipleUInt16Status);
                        return resultMultipleUInt16Status;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.File:
                        var resultFileStatus = SendFile(modbus, slaveAddress);
                        _resultAction?.Invoke(resultFileStatus);
                        return resultFileStatus;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.Command:
                        if (_setter.Command == null)
                            return false;
                        var resultCommandStatus = SendCommand(modbus, slaveAddress);
                        _resultAction?.Invoke(resultCommandStatus);
                        return resultCommandStatus;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return false;
            }

            private int Convert(string convertion, int origValue)
            {
                var reg = new TypeRegistry();
                reg.RegisterSymbol("val", origValue);
                var p = new CompiledExpression(convertion) { TypeRegistry = reg };
                return (int)p.Eval();
            }

            private uint GetBitValue(HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.BitsValue val, IReadOnlyList<int> requestData)
            {
               
                switch (val.Type)
                {
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.BitsValue.BitsTypes.Zero:
                        return 0;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.BitsValue.BitsTypes.Literal:
                        return val.Value << val.Index;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.BitsValue.BitsTypes.FromRequest:
                        if (val.Count == 0)
                            return 0;
                        if (requestData == null || val.ParamIndex < 0 || val.ParamIndex >= requestData.Count)
                            return 0;
                        var retVal = requestData[val.ParamIndex];
                        var mask = 1;
                        for (var i = 1; i < val.Count; i++)
                        {
                            mask <<= 1;
                            mask |= 1;
                        }
                        return (uint) ((retVal & mask) << val.Index);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private byte GetByteValue(HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.ByteValue val,
                IReadOnlyList<int> requestData)
            {
                switch (val.Type)
                {
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.ByteValue.ByteTypes.Zero:
                        return 0;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.ByteValue.ByteTypes.Literal:
                        return val.Value;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.ByteValue.ByteTypes.FromRequest:
                        if (requestData != null && val.ParamIndex >= 0 && val.ParamIndex < requestData.Count)
                        {
                            var retVal = requestData[val.ParamIndex];
                            if (!string.IsNullOrEmpty(val.Conversion))
                                retVal = Convert(val.Conversion, retVal);
                            return (byte)retVal;
                        }
                        break;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.ByteValue.ByteTypes.BitsFromRequest:
                        if (val.Bits == null)
                            return 0;
                        var retVal1 = (byte)val.Bits.Aggregate(0, (current, bitsValue) => current | (byte) GetBitValue(bitsValue, requestData));
                        return retVal1;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return 0;
            }

            private ushort GetWordValue(HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.WordValue val,
                IReadOnlyList<int> requestData)
            {
                switch (val.Type)
                {
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.WordValue.WordTypes.Zero:
                        return 0;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.WordValue.WordTypes.Literal:
                        return val.Value;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.WordValue.WordTypes.TwoBytesFromRequest:
                        var retVal = (ushort) 0;
                        if (val.Hi != null)
                            retVal = (ushort) (GetByteValue(val.Hi, requestData) << 8);
                        if (val.Lo != null)
                            retVal |= GetByteValue(val.Lo, requestData);
                        return retVal;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SendCommand.WordValue.WordTypes.WordFromRequest:
                        if (requestData != null && val.ParamIndex >= 0 && val.ParamIndex < requestData.Count)
                            return (ushort)requestData[val.ParamIndex];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return 0;
            }

            private bool SendCommand(ModbusSerialMaster modbus, byte slaveAddress)
            {
                var command = _setter.Command;

                if(command?.Id == null)
                    return false;

                var requestValues = _pendingObject as int[];


                var msg = new WriteSysUserCommandRequest(slaveAddress, command.IsSystem,
                    GetByteValue(command.Id, requestValues), GetByteValue(command.Data, requestValues),
                    GetWordValue(command.Additional1, requestValues),
                    GetWordValue(command.Additional2, requestValues),
                    GetWordValue(command.Additional3, requestValues));

                var resp = modbus.ExecuteCustomMessage<WriteSysUserCommand>(msg);

                Thread.Sleep(200);

                var msgCheck = new ReadExceptionStatusRequest(slaveAddress);
                var respCheck = modbus.ExecuteCustomMessage<ReadExceptionStatus>(msgCheck);
                return respCheck.ExceptionStatusBits[0];
            }

            private bool SendTime(ModbusSerialMaster modbus, byte slaveAddress)
            {
                var curTime = DateTime.Now;
                var msg = new WriteSysUserCommandRequest(slaveAddress, true,
                    
                    0x10, 0x00,
                    (byte) curTime.Hour,//23,//
                    (byte)curTime.Minute,//59,//
                    (byte)curTime.Day,
                    (byte)curTime.Second,
                    (byte)(curTime.Year%100),
                    (byte)curTime.Month);

                var resp = modbus.ExecuteCustomMessage<WriteSysUserCommand>(msg);
                Thread.Sleep(500);

                var msgCheck = new ReadExceptionStatusRequest(slaveAddress);
                var respCheck = modbus.ExecuteCustomMessage<ReadExceptionStatus>(msgCheck);

                
                //var modbusResult = modbus.ReadInputRegisters(slaveAddress, 0, 1);
                return respCheck.ExceptionStatusBits[0];
                /*if (setTimeRes.Length > 0 && setTimeRes[0] == 0xffff)
                    WriteToLog?.Invoke(this, "Время установлено успешно!");
                else
                {
                    WriteToLog?.Invoke(this, $"Ошибка установки времени! ({setTimeRes[0]:X4})");
                    //MessageBox.Show($"Ошибка установки времени! ({setTimeRes[0]:X4})");
                }*/

            }

            private bool SendMultipleUInt16(ModbusSerialMaster modbus, byte slaveAddress)
            {
                //            _modbus.WriteMultipleRegisters(2, 8, timeData);
                modbus.WriteMultipleRegisters(slaveAddress, CommandHoldingRegister, (ushort[])_pendingObject);
                Thread.Sleep(50);

                var msg = new ReadExceptionStatusRequest(slaveAddress);
                var resp = modbus.ExecuteCustomMessage<ReadExceptionStatus>(msg);
                return resp.ExceptionStatusBits[0];
            }

            private bool SendUInt16(ModbusSerialMaster modbus, byte slaveAddress)
            {
                modbus.WriteSingleRegister(slaveAddress, Index, (ushort)_pendingObject);
                Thread.Sleep(50);
                var msg = new ReadExceptionStatusRequest(slaveAddress);
                var resp = modbus.ExecuteCustomMessage<ReadExceptionStatus>(msg);
                return resp.ExceptionStatusBits[0];
            }

            private bool SendFile(ModbusSerialMaster modbus, byte slaveAddress)
            {
                var hexBytes = (byte[]) _pendingObject;
                var sent = 0;
                ushort wordsSent = (ushort) (Index / 2);
                do
                {
                    // Для табло максимальный размер буфера 140
                    var wordsToSend = (int)ModbusWriteFileRecord.MaxDataCountByBufferSize(140);
                    var bytesToSend = wordsToSend * 2;
                    if (sent + bytesToSend > hexBytes.Length)
                        bytesToSend = hexBytes.Length - sent;
                    //slaveAddress, Index, wordsSent
                    // Поддерживается пока только файл №1
                    var msg = new ModbusWriteFileRecordRequest(slaveAddress, 1, wordsSent, hexBytes, sent, bytesToSend);
                    var resp = modbus.ExecuteCustomMessage<ModbusWriteFileRecord>(msg);
                    sent += bytesToSend;
                    wordsSent += (ushort)wordsToSend;
                } while (sent < hexBytes.Length);

                var msgResult = new ReadExceptionStatusRequest(slaveAddress);
                var respResult = modbus.ExecuteCustomMessage<ReadExceptionStatus>(msgResult);
                return respResult.ExceptionStatusBits[0];
            }

            //            private string ReportSlaveIdRequest(ModbusSerialMaster modbus, byte slaveAddress)
            //            {
            //                var msg = new ReportSlaveIdRequest(slaveAddress);
            //                var resp = modbus.ExecuteCustomMessage<ReportSlaveId>(msg);
            //                return null;
            //            }
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
                                case HomeServerSettings.ControllerGroup.Controller.DataTypes.ULong:
                                    holdingCheck.CheckStateULong(holdingsStatus[holdingCheck.Index - _holdingRegisterMinIndex], holdingsStatus[holdingCheck.Index + 1 - _holdingRegisterMinIndex]);
                                    break;
                                default:
                                    holdingCheck.CheckState(holdingsStatus[holdingCheck.Index - _holdingRegisterMinIndex]);
                                    break;
                            }
                        }
                    }
                }
                // Остальное
                if (_actionOnReceiveSlaveId != null && _actionOnReceiveSlaveId.IsNeedCheck())
                {
                    curOperation = $"Check Slave Id";
//                    var msg = new ReportSlaveIdRequest(SlaveAddress);
//                    var resp = modbus.ExecuteCustomMessage<ReportSlaveId>(msg);
                    var msg = new ReadDeviceIdentificationRequest(SlaveAddress, ReadDeviceIdentification.ReadDeviceIdCodes.Basic, 0);
                    var resp = modbus.ExecuteCustomMessage<ReadDeviceIdentification>(msg);
                    var objs = resp.StringObjects;
                    msg = new ReadDeviceIdentificationRequest(SlaveAddress, ReadDeviceIdentification.ReadDeviceIdCodes.Regular, 0);
                    resp = modbus.ExecuteCustomMessage<ReadDeviceIdentification>(msg);
                    objs.AddRange(resp.StringObjects);
                    _actionOnReceiveSlaveId.SendValue(JsonConvert.SerializeObject(objs));
                    //                    var rtuTransport = (ModbusRtuTransport)modbus.Transport;
                    //                    if (resp.ReseivedId != null)
                    //                    {
                    //                        _actionOnReceiveSlaveId.SendValue(resp.ReseivedId);
                    //                    }
                }
                // Катушки
                if (_actionsOnDeviceStatus != null)
                {
                    curOperation = $"Check Device Status ";
                    var msg = new ReadDeviceStatusRequest(SlaveAddress);
                    var resp = modbus.ExecuteCustomMessage<ReadDeviceStatus>(msg);
                    foreach (var check in _actionsOnDeviceStatus)
                    {
                        check.CheckState(resp.DeviceStatusBits[check.Index]);
                    }
                }
            }
            catch (Exception ee)
            {
                throw new Exception($"{curOperation} " + ee);//.Message
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

                        if (check.InitialState != null)
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
                            case HomeServerSettings.ControllerGroup.Controller.DataTypes.ULong:
                                Console.WriteLine("Reset ULong");
                                var resetData = new ushort[2];
                                resetData[0] = (ushort)(check.ULongDefault >> 16);
                                resetData[1] = (ushort)(check.ULongDefault & 0xFFFF);
                                modbus.WriteMultipleRegisters(SlaveAddress, check.Index, resetData);
                                break;
                            default:
                                
                                modbus.WriteSingleRegister(SlaveAddress, check.Index, check.UInt16Default ?? 0);
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
        /// <param name="checkBoolStatus">При каком событии вызывать метод</param>
        /// <param name="index">Индекс регистра или катушки</param>
        /// <param name="callback">Метод, который будет вызываться</param>
        /// <param name="initialState">Начальное состояние</param>
        /// <param name="resetAfter">Сброс значения только для катушек и CheckBoolStatus != OnBoth</param>
        /// <returns>Reset action</returns>
        public Action SetActionOnDiscreteOrCoil(bool isCoil, CheckBoolStatus checkBoolStatus, ushort index,
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
            if (checkBoolStatus == CheckBoolStatus.OnBoth && resetAfter)
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
                var actionOnCoil = new ActionOnCoil(index, checkBoolStatus, callback, resetAfter, initialState);

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
                var actionOnDiscrete = new ActionOnDiscrete(index, checkBoolStatus, callback, initialState);
                _discreteChecks.Add(actionOnDiscrete);
                return null;
            }
        }

        /// <summary>
        /// Реакция на изменение состояния устройства
        /// </summary>
        /// <param name="checkBoolStatus"></param>
        /// <param name="index"></param>
        /// <param name="callback"></param>
        /// <param name="initialState"></param>
        /// <returns></returns>
        public Action SetActionOnDeviceStatus(CheckBoolStatus checkBoolStatus, byte index,
            Action<bool> callback, bool? initialState = null)
        {
            if (index > MaxDeviceStatusIndex)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            {
                if (_deviceStatusMinIndex > index)
                    _deviceStatusMinIndex = index;
                if (_deviceStatusMaxIndex < index)
                    _deviceStatusMaxIndex = index;
                if (_actionsOnDeviceStatus == null)
                    _actionsOnDeviceStatus = new List<ActionOnDeviceStatus>();
                var actionOnDeviceStatus = new ActionOnDeviceStatus(index, checkBoolStatus, callback, initialState);
                _actionsOnDeviceStatus.Add(actionOnDeviceStatus);
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
        public Action SetActionOnRegister(bool isHolding, ushort index, HomeServerSettings.ControllerGroup.Controller.DataTypes registerType, Action<ushort> callback,
            Action<uint> uLongCallback,
            bool raiseOlwais = false, TimeSpan? checkInterval = null, bool resetAfterRead = false, ushort uInt16Default = 0, uint uLongDefault = 0)
        {
            if (callback == null && uLongCallback == null)
                throw new ArgumentNullException(nameof(callback) + " and " + nameof(uLongCallback));
            var endIndex = index;
            switch (registerType)
            {
                case HomeServerSettings.ControllerGroup.Controller.DataTypes.UInt16:
                    break;
                case HomeServerSettings.ControllerGroup.Controller.DataTypes.Float:
                    break;
                case HomeServerSettings.ControllerGroup.Controller.DataTypes.ULong:
                    endIndex = (ushort)(index + 1);
                    break;
                case HomeServerSettings.ControllerGroup.Controller.DataTypes.RdDateTime:
                    break;
                case HomeServerSettings.ControllerGroup.Controller.DataTypes.RdTime:
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
                    RaiseOlwais = raiseOlwais,
                    CheckInterval = checkInterval
                });
                return null;
            }
        }

        public void SetActionOnSlaveId(Action<string> callback, TimeSpan? checkInterval = null)
        {
            _actionOnReceiveSlaveId = new ActionOnReceiveSlaveId(callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setter"></param>
        /// <param name="result">При установки параметра возвращаем правильно ли установилось или нет</param>
        /// <returns></returns>
        public ModbusSetter SetSetter(HomeServerSettings.ControllerGroup.Controller.Setter setter, Action<bool> result = null)
        {
            if (_setters == null)
                _setters = new List<ModbusSetter>();
            var newSetter = new ModbusSetter(setter, result);
            _setters.Add(newSetter);
            return newSetter;
        }

    }
}
