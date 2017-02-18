using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ExpressionEvaluator;
using HomeServer.ModbusCustomMessages;
using HomeServer.Models;
using HomeServer.Models.Base;
using Modbus.Device;
using Newtonsoft.Json;

namespace HomeServer.Objects
{
    /// <summary>
    /// Контроллер (Arduino)
    /// </summary>
    public class ShController
    {
        private const int ModbusResultSuccess = 0x8080;
        /// <summary>
        /// После прошивки у всех устройств адрес по умолчанию,
        /// который потом заменяется на желаемый
        /// </summary>
        private const byte ModbusAddresForNewDevice = 0x7f;
        private const int MaxDiscreteOrCoilIndex = 15;
        private const int MaxDeviceStatusIndex = 7;
        private const ushort CommandHoldingRegister = 0;

        /// <summary>
        /// Состояние контроллера
        /// null - ещё не известно
        /// true - нормально работает
        /// false - ошибка связи
        /// </summary>
        public bool? State =  null;
        public string ControllerGroupName;
        public string Name;

        public List<HomeServerSettings.ActiveValue> ActiveValues; 
        /// <summary>
        /// Количество ошибок обращения к контроллеру
        /// Если подряд 5 раз вылетят ошибки, то обращаемся к этому контроллеру только раз в минуту
        /// </summary>
        public int ErrorCount = 0;

        /// <summary>
        /// Время последнего опроса
        /// </summary>
        public DateTime LastAccess;

        interface IResetParameter
        {
            bool PendingReset { get; set; }
            void Reset();
        }

        #region DiscreteOrCoil

        public class ActionOnDiscreteOrCoil
        {
            private HomeServerSettings.ControllerGroup.Controller.Parameter _parameter;
            private bool? _currentState;
            private HomeServerSettings.ActiveValue _activeValue;
            public bool? InitialState { get; private set; }

            public CheckBoolStatus CheckBoolStatus { get; set; }
            public ushort Index => _parameter.ModbusIndex;
            private Action<bool> CallBack { get; set; }
            public TimeSpan? CheckInterval { get; set; }

            protected ActionOnDiscreteOrCoil(HomeServerSettings.ControllerGroup.Controller.Parameter parameter,
                CheckBoolStatus checkBoolStatus, 
                Action<bool> callBack,
                HomeServerSettings.ActiveValue activeValue,
                bool? initialState)
            {
                _parameter = parameter;
                CheckBoolStatus = checkBoolStatus;
                CallBack = callBack;
                _activeValue = activeValue;
                InitialState = initialState;
                _currentState = initialState;
            }

            public bool CheckState(bool newState)
            {
                if (newState == _currentState)
                    return false;

                if (_activeValue == null)
                    return false;
                //BaseUtils.WriteParamToBase(_id, boolValue: newState);

                switch (CheckBoolStatus)
                {
                    case CheckBoolStatus.OnTrue:
                        if (newState)
                            _activeValue.SetNewValue(true);
                            //CallBack(true);
                        break;
                    case CheckBoolStatus.OnFalse:
                        if (!newState)
                            _activeValue.SetNewValue(false);
                            //CallBack(false);
                        break;
                    case CheckBoolStatus.OnBoth:
                        _activeValue.SetNewValue(newState);
                        //CallBack(newState);
                        break;
                }
                _currentState = newState;


                

                return true;
            }

        }


        private class ActionOnDiscrete : ActionOnDiscreteOrCoil
        {
            public ActionOnDiscrete(HomeServerSettings.ControllerGroup.Controller.Parameter parameter,
                CheckBoolStatus checkBoolStatus, Action<bool> callBack,
                HomeServerSettings.ActiveValue activeValue,
                bool? initialState)
                : base(parameter, checkBoolStatus, callBack, activeValue, initialState)
            {
            }
        }

        internal class ActionOnCoil : ActionOnDiscreteOrCoil, IResetParameter
        {
            public bool ResetAfter { get; private set; }

            public ActionOnCoil(HomeServerSettings.ControllerGroup.Controller.Parameter parameter, 
                CheckBoolStatus checkBoolStatus, Action<bool> callBack, 
                bool resetAfter, HomeServerSettings.ActiveValue activeValue,
                bool? initialState)
                : base(parameter, checkBoolStatus, callBack, activeValue, initialState)
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
            private HomeServerSettings.ControllerGroup.Controller.Parameter _parameter;
            private ushort? _currentValue;
            private uint? _currentLongValue;
            protected DateTime _lastCheck = DateTime.MinValue;

            private HomeServerSettings.ActiveValue _activeValue;
            /// <summary>
            /// Сбросить после чтения
            /// </summary>
            public bool ResetAfterRead { get; set; }

            public ushort Index => _parameter.ModbusIndex;
            private Action<ushort> CallBack { get; set; }
            private Action<uint> CallBackULong { get; set; }
            public TimeSpan? CheckInterval { get; set; }
            public ushort? UInt16Default { get; set; }
            public uint? ULongDefault { get; set; }
            /// <summary>
            /// Вызывать CallBack даже когда данные не изменились
            /// </summary>
            public bool RaiseOlwais { get; set; }
            //            public HomeServerSettings.ControllerGroup.Controller.DataTypes RegisterType { get; set; }

            public HomeServerSettings.ControllerGroup.Controller.DataTypes RegisterType => _parameter.DataType;

            public ActionOnRegister(HomeServerSettings.ControllerGroup.Controller.Parameter parameter, 
                Action<ushort> callBack, 
                HomeServerSettings.ActiveValue activeValue,
                Action<uint> callBackULong = null)
            {
                _parameter = parameter;
                CallBack = callBack;
                _activeValue = activeValue;
                CallBackULong = callBackULong;
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

                _lastCheck = DateTime.Now;
                if (_currentValue == newValue)
                    return false;

                _currentValue = newValue;

                if (_activeValue == null)
                    return false;


                object convertedValue = newValue;

                switch (_parameter.DataType)
                {
                    case HomeServerSettings.ControllerGroup.Controller.DataTypes.UInt16:
                    {
                        double resValue = newValue;
                        if (Math.Abs(_parameter.Multiple) > 0.000001)
                            resValue *= _parameter.Multiple;
                        convertedValue = resValue;
                    }
                        break;
                    case HomeServerSettings.ControllerGroup.Controller.DataTypes.ModbusUInt16Bool:
                        break;
                    case HomeServerSettings.ControllerGroup.Controller.DataTypes.Double:
                        {
                            // делаем число со знаком
                            double resValue = (short)newValue;
                            if (Math.Abs(_parameter.Multiple) > 0.000001)
                                resValue *= _parameter.Multiple;
                            convertedValue = resValue;
                        }
                        break;
                    case HomeServerSettings.ControllerGroup.Controller.DataTypes.ULong:
                        break;
                    case HomeServerSettings.ControllerGroup.Controller.DataTypes.RdDateTime:
                        break;
                    case HomeServerSettings.ControllerGroup.Controller.DataTypes.RdTime:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                if (!_activeValue.SetNewValue(convertedValue))
                    return false;
                //TODO Вынести это в setters??
                if (ResetAfterRead)
                {
                    _currentValue = UInt16Default ?? 0;
                    Reset();
                }

                /*!!!
                                if (_currentValue == newValue)
                                    return false;

                                //WriteToBase(_id, intValue: newValue);
                                _currentValue = newValue;

                                if (UInt16Default != null && _currentValue == UInt16Default.Value)
                                    return false;
                                //!!!CallBack?.Invoke(newValue);
                                if (ResetAfterRead)
                                {
                                    _currentValue = UInt16Default ?? 0;
                                    Reset();
                                }
                */
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
                //WriteToBase(_id, intValue: newValue);
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

            public ActionOnRegisterDateTime(HomeServerSettings.ControllerGroup.Controller.Parameter parameter,
                Action<DateTime> callBack, HomeServerSettings.ActiveValue activeValue) 
                : base(parameter, null, activeValue)
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
        private ushort _deviceStatusMinIndex = MaxDeviceStatusIndex;
        /// <summary>
        /// Максимальный индекс дискретного регистра для проверки
        /// </summary>
        private ushort _deviceStatusMaxIndex;

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
            private readonly Action<string> _writeToLog;
            private readonly Action<bool> _resultAction;

            public ModbusSetter(HomeServerSettings.ControllerGroup.Controller.Setter setter, Action<string> writeToLog, Action<bool> resultAction = null)
            {
                Index = setter.ModbusIndex;
                SetterType = setter.Type;
                _setter = setter;
                _writeToLog = writeToLog;
                _resultAction = resultAction;
            }

            public string Id => _setter.Id;

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
            /// <param name="value"></param>
            /// <returns></returns>
            public bool Set(ModbusSerialMaster modbus, byte slaveAddress, object value)
            {
                IsPending = false;

                switch (SetterType)
                {
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.Bool:
                        var resultBoolStatus = SendUInt16(modbus, slaveAddress, value);
                        _resultAction?.Invoke(resultBoolStatus);
                        return resultBoolStatus;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.RealDateTime:
                        var resultStatus = SendCurrentTime(modbus, slaveAddress);
                        _resultAction?.Invoke(resultStatus);
                        return resultStatus;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.UInt16:
                        var resultUInt16Status = SendUInt16(modbus, slaveAddress, value);
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
                        var resultCommandStatus = SendCommand(modbus, slaveAddress, value);
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
                if (val == null)
                    return 0;
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



            private bool SendCommand(ModbusSerialMaster modbus, byte slaveAddress, object value)
            {
                var command = _setter.Command;

                if(command?.Id == null)
                    return false;
                var requestValues = new int[1];
                if (value is double)
                {
                    requestValues[0] = (int)((double)value);
                }
                else if (value is int)
                {
                    requestValues[0] = (int) value;
                }
                else if (value is bool)
                {
                    requestValues[0] = (bool) value ? 0xFF : 0;
                }

                    //                var requestValues = _pendingObject as int[];


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

            private bool SendCurrentTime(ModbusSerialMaster modbus, byte slaveAddress)
            {
                SetCurrentTime(modbus, slaveAddress, _writeToLog);
/*
                var curTime = DateTime.Now;
                var msg = new WriteSysUserCommandRequest(SlaveAddress, true,
                    
                    0x10, 0x00,
                    (byte) curTime.Hour,//23,//
                    (byte)curTime.Minute,//59,//
                    (byte)curTime.Day,
                    (byte)curTime.Second,
                    (byte)(curTime.Year%100),
                    (byte)curTime.Month);

                var resp = modbus.ExecuteCustomMessage<WriteSysUserCommand>(msg);
*/
                Thread.Sleep(50);

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
                Thread.Sleep(5);

                var msg = new ReadExceptionStatusRequest(slaveAddress);
                var resp = modbus.ExecuteCustomMessage<ReadExceptionStatus>(msg);
                return resp.ExceptionStatusBits[0];
            }

            private bool SendCoil(IModbusMaster modbus, byte slaveAddress, bool value)
            {
                modbus.WriteSingleCoil(slaveAddress, Index, value);//_pendingObject
                Thread.Sleep(5);
                var msg = new ReadExceptionStatusRequest(slaveAddress);
                var resp = modbus.ExecuteCustomMessage<ReadExceptionStatus>(msg);
                return resp.ExceptionStatusBits[0];
            }

            private bool SendUInt16(IModbusMaster modbus, byte slaveAddress, object value)
            {
                ushort converted = 0;

                if (value is double)
                {
                    converted = (ushort) (Math.Round((double) value));
                }
                else if (value is ushort)
                    converted = (ushort) value;
                else if (value is int)
                    converted = (ushort) (int)value;
                else if (value is long)
                    converted = (ushort) (long)value;
                else if (value is ulong)
                    converted = (ushort) (ulong)value;

                modbus.WriteSingleRegister(slaveAddress, Index, converted);//_pendingObject
                Thread.Sleep(5);
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
        private readonly Action<string> _writeToLog;
        private readonly byte[] _slaveId;

        public ShController(string id, string slaveId, byte slaveAddress, Action<string> writeToLog)
        {
            Id = id;
            SlaveAddress = slaveAddress;
            _writeToLog = writeToLog;

            if(string.IsNullOrEmpty(slaveId))
                return;
            var slaveIdSplit = slaveId.Split('.');
            if (slaveIdSplit.Length != 4)
                return;
            _slaveId = new byte[slaveIdSplit.Length];
            for (var i = 0; i < slaveIdSplit.Length; i++)
            {
                byte b;
                if (!byte.TryParse(slaveIdSplit[i].Trim(), out b))
                {
                    _slaveId = null;
                    break;
                }
                _slaveId[i] = b;
            }
        }


        /// <summary>
        /// Проверка SlaveId устройства
        /// </summary>
        /// <param name="modbus"></param>
        /// <param name="address"></param>
        /// <returns>Если устройство не отвечает по данному адресу, возвращаем null
        /// Если SlaveId не совпадает, возвращаем false
        /// </returns>
        private bool? CheckSlaveId(ModbusSerialMaster modbus, byte address)
        {
            try
            {
                var slaveId = GetSlaveId(modbus, address);
                if (slaveId != null)
                {
                    // Если от устройства получено значение, значит адрес присвоен и любое несоответствие означает, что 
                    // это не то устройство или какая-то ошибка
                    if (slaveId.Length != _slaveId.Length)
                        return false;
                    return slaveId.SequenceEqual(_slaveId);
                }
                return false;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Присвоение устройству заданного адреса, если ещё не присвоен
        /// На основе SlaveId
        /// </summary>
        /// <param name="modbus"></param>
        private bool AssignAddress(ModbusSerialMaster modbus)
        {
            if (CheckSlaveId(modbus, SlaveAddress) == true)
                return true;

            // Проверяем, еслить ли новое устройство в сети
            if (CheckSlaveId(modbus, ModbusAddresForNewDevice) != true)
                return false;

            // Пытаемся установить адрес
            if (!SetDesiredAddress(modbus))
                return false;
            WriteToLog($"Address changed to {SlaveAddress}");
            Console.WriteLine($"Address for {_slaveId[0]}.{_slaveId[1]}.{_slaveId[2]}.{_slaveId[3]} changed to {SlaveAddress}");

            return CheckSlaveId(modbus, SlaveAddress) == true;
        }

        private bool SetDesiredAddress(ModbusSerialMaster modbus)
        {
            var msg = new WriteSysUserCommandRequest(ModbusAddresForNewDevice, true,
                0x01, SlaveAddress);

            var resp = modbus.ExecuteCustomMessage<WriteSysUserCommand>(msg);

            var msgCheck = new ReadExceptionStatusRequest(SlaveAddress);
            var respCheck = modbus.ExecuteCustomMessage<ReadExceptionStatus>(msgCheck);
            return respCheck.ExceptionStatusBits[0];
        }

        private byte[] GetSlaveId(ModbusSerialMaster modbus, byte slaveAddress)
        {
            var msg = new ReportSlaveIdRequest(slaveAddress);
            var resp = modbus.ExecuteCustomMessage<ReportSlaveId>(msg);
            return resp.ReseivedId;
        }

        private void WriteToLog(string message)
        {
            var id = string.Empty;
            if (_slaveId != null)
                id = string.Join(".", _slaveId);
            _writeToLog?.Invoke($"{ControllerGroupName} / {Name} : {id}: {message}");
        }

        private bool _addresIsSet;
        private bool _needTimeSet;
        private int _lastTimeSetHour = -1;

        public virtual bool GetStatus(ModbusSerialMaster modbus)
        {

//            var startDt = DateTime.Now;

            var timeJustSet = false;
            var curOperation = string.Empty;
            if (_slaveId != null && !_addresIsSet)
            {
                curOperation = "Checking address";
                _addresIsSet = AssignAddress(modbus);
                if (_addresIsSet == false)
                {
                    WriteToLog("Error assigning address");
                    return false;
                }
                GetDeviceStatus(modbus);
                if (_needTimeSet)
                {
                    SetCurrentTime(modbus, SlaveAddress, WriteToLog);
                    timeJustSet = true;
                }
            }

//            Console.WriteLine($"        {Id}: After set address: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");

            try
            {
                // Установка времени если сменился час
                if (_needTimeSet && !timeJustSet)
                { 
                    var curHour = DateTime.Now.Hour;
                    if (curHour != _lastTimeSetHour)
                    {
                        SetCurrentTime(modbus, SlaveAddress, WriteToLog);


                        // Ответ не ждём
                        _lastTimeSetHour = curHour;
                    }
                }

//                Console.WriteLine($"        {Id}: After set time: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");
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
//                Console.WriteLine($"        {Id}: After discrete: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");

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
//                Console.WriteLine($"        {Id}: After coils: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");
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
                        Thread.Sleep(6);
                        curOperation = $"CheckInput ";
                        //Console.WriteLine($"                {Id}: Before ReadInputRegisters: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");
                        var inputsStatus = modbus.ReadInputRegisters(SlaveAddress, _inputRegisterMinIndex,
                            (ushort)(_inputRegisterMaxIndex - _inputRegisterMinIndex + 1));
//                        Console.WriteLine($"                {Id}: After ReadInputRegisters: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");
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
//                Console.WriteLine($"        {Id}: After registers: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");

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
                        Thread.Sleep(6);
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
//                Console.WriteLine($"        {Id}: After holdings: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");

                // Остальное
                if (_actionOnReceiveSlaveId != null && _actionOnReceiveSlaveId.IsNeedCheck())
                {
                    curOperation = $"Check Device Identification";
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
//                Console.WriteLine($"        {Id}: After other: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");

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
//                Console.WriteLine($"        {Id}: After last: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");

            }
            catch (Exception ee)
            {
                throw new Exception($"{curOperation} " + ee);//.Message
            }
            return true;
        }

        private static WriteSysUserCommand SetCurrentTime(ModbusSerialMaster modbus, byte slaveAddress, Action<string> writeToLog)
        {
            var curTime = DateTime.Now;
            var msg = new WriteSysUserCommandRequest(slaveAddress, true,

                0x10, 0x00,
                (byte)curTime.Hour,//23,//
                (byte)curTime.Minute,//59,//
                (byte)curTime.Day,
                (byte)curTime.Second,
                (byte)(curTime.Year % 100),
                (byte)(curTime.Month -1));

            writeToLog("Time sync send");

            var res = modbus.ExecuteCustomMessage<WriteSysUserCommand>(msg);

            // Для отладки ждём
            Thread.Sleep(5);//500
            var msgCheck = new ReadExceptionStatusRequest(slaveAddress);
            var respCheck = modbus.ExecuteCustomMessage<ReadExceptionStatus>(msgCheck);
            if(respCheck.ExceptionStatusBits[2])
                writeToLog("Time not set!!!!");
            else if(respCheck.ExceptionStatusBits[3])
                writeToLog("Time need sync!!!!");
            else if (!respCheck.ExceptionStatusBits[0])
                writeToLog("Error set time!!!!");
            else
                writeToLog("Time OK.");
            return res;
        }

        private const int InputTimeSetBit = 0;
        private const int InputNeedTimeSetBit = 1;
        private void GetDeviceStatus(ModbusSerialMaster modbus)
        {
            //curOperation = $"Check Device Status ";
            var msg = new ReadDeviceStatusRequest(SlaveAddress);
            var resp = modbus.ExecuteCustomMessage<ReadDeviceStatus>(msg);
            _needTimeSet = resp.DeviceStatusBits[InputNeedTimeSetBit];
        }

        /// <summary>
        /// Действия с шиной
        /// </summary>
        /// <param name="modbus"></param>
        public void SetCoils(ModbusSerialMaster modbus)
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

            
        }

        public void SetSetters(ModbusSerialMaster modbus)
        {
            if (_setters != null)
            {
                foreach (var activeValue in ActiveValues)
                {
                    if (!activeValue.IsChanged)
                        continue;
                    foreach (var setter in _setters)
                    {
                        if (setter.Id == activeValue.Id)
                        {
                            Console.WriteLine($">>{setter.Id} = {activeValue.Value}");
                            var setResult = setter.Set(modbus, SlaveAddress, activeValue.Value);
                        }
                    }
                    //activeValue.Value.ResetChange();
                }


                
            }
        }

        /// <summary>
        /// Прописывает метод, который будет вызван после изменения статуса дискретного регистра или катушки
        /// </summary>
        /// <param name="id">Id параметра, которое пишется в базу</param>
        /// <param name="parameter"></param>
        /// <param name="isCoil"></param>
        /// <param name="checkBoolStatus">При каком событии вызывать метод</param>
        /// <param name="index">Индекс регистра или катушки</param>
        /// <param name="callback">Метод, который будет вызываться</param>
        /// <param name="activeValue"></param>
        /// <param name="initialState">Начальное состояние</param>
        /// <param name="resetAfter">Сброс значения только для катушек и CheckBoolStatus != OnBoth</param>
        /// <returns>Reset action</returns>
        public Action SetActionOnDiscreteOrCoil(HomeServerSettings.ControllerGroup.Controller.Parameter parameter,
            CheckBoolStatus checkBoolStatus,
            Action<bool> callback, HomeServerSettings.ActiveValue activeValue,
             bool resetAfter = false)
        {
            if (parameter.ModbusIndex > MaxDiscreteOrCoilIndex)
                throw new ArgumentOutOfRangeException(nameof(parameter.ModbusIndex));
//            if (callback == null)
//                throw new ArgumentNullException(nameof(callback));

            var isCoil = parameter.ModbusType == HomeServerSettings.ControllerGroup.Controller.ModbusTypes.Coil;
            var initialState = parameter.BoolDefault;
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
                if (_coilMinIndex > parameter.ModbusIndex)
                    _coilMinIndex = parameter.ModbusIndex;
                if (_coilMaxIndex < parameter.ModbusIndex)
                    _coilMaxIndex = parameter.ModbusIndex;
                if (_coilChecks == null)
                    _coilChecks = new List<ActionOnCoil>();
                var actionOnCoil = new ActionOnCoil(parameter, checkBoolStatus, callback, resetAfter, activeValue, initialState);

                _coilChecks.Add(actionOnCoil);
                return () =>
                {
                    actionOnCoil.Reset();
                };
            }
            else
            {
                if (_discreteRegisterMinIndex > parameter.ModbusIndex)
                    _discreteRegisterMinIndex = parameter.ModbusIndex;
                if (_discreteRegisterMaxIndex < parameter.ModbusIndex)
                    _discreteRegisterMaxIndex = parameter.ModbusIndex;
                if (_discreteChecks == null)
                    _discreteChecks = new List<ActionOnDiscrete>();
                var actionOnDiscrete = new ActionOnDiscrete(parameter, checkBoolStatus, callback, activeValue, initialState);
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
        public Action SetActionOnDeviceStatus(CheckBoolStatus checkBoolStatus, ushort index,
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
        /// <param name="parameter"></param>
        /// <param name="id">Id параметра, которое пишется в базу</param>
        /// <param name="isHolding"></param>
        /// <param name="index"></param>
        /// <param name="registerType"></param>
        /// <param name="callback"></param>
        /// <param name="uLongCallback"></param>
        /// <param name="activeValue"></param>
        /// <param name="raiseOlwais">Вызывать callback даже когда показания не изменились</param>
        /// <param name="checkInterval"></param>
        /// <returns>Reset action</returns>
        public Action SetActionOnRegister(HomeServerSettings.ControllerGroup.Controller.Parameter parameter, 
            Action<ushort> callback,
            Action<uint> uLongCallback, 
            HomeServerSettings.ActiveValue activeValue,
            bool raiseOlwais = false, TimeSpan? checkInterval = null, 
            bool resetAfterRead = false, ushort uInt16Default = 0, uint uLongDefault = 0, double doubleDefault = 0)
        {
//            if (callback == null && uLongCallback == null)
//                throw new ArgumentNullException(nameof(callback) + " and " + nameof(uLongCallback));

            var isHolding = parameter.ModbusType ==
                        HomeServerSettings.ControllerGroup.Controller.ModbusTypes.HoldingRegister;
            var endIndex = parameter.ModbusIndex;
            switch (parameter.DataType)
            {
                case HomeServerSettings.ControllerGroup.Controller.DataTypes.UInt16:
                    break;
                case HomeServerSettings.ControllerGroup.Controller.DataTypes.ModbusUInt16Bool:
                    break;
                case HomeServerSettings.ControllerGroup.Controller.DataTypes.Double:
                    break;
                case HomeServerSettings.ControllerGroup.Controller.DataTypes.ULong:
                    endIndex = (ushort)(parameter.ModbusIndex + 1);
                    break;
                case HomeServerSettings.ControllerGroup.Controller.DataTypes.RdDateTime:
                    break;
                case HomeServerSettings.ControllerGroup.Controller.DataTypes.RdTime:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter.DataType), parameter.DataType, null);
            }
            if (!isHolding)
            {
                if (_inputRegisterMinIndex > parameter.ModbusIndex)
                    _inputRegisterMinIndex = parameter.ModbusIndex;
                if (_inputRegisterMaxIndex < endIndex)
                    _inputRegisterMaxIndex = endIndex;
                if (_inputChecks == null)
                    _inputChecks = new List<ActionOnRegister>();

                var holdingCheck = new ActionOnRegister(parameter, callback, activeValue, uLongCallback)
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
                if (_holdingRegisterMinIndex > parameter.ModbusIndex)
                    _holdingRegisterMinIndex = parameter.ModbusIndex;
                if (_holdingRegisterMaxIndex < endIndex)
                    _holdingRegisterMaxIndex = endIndex;
                if (_holdingChecks == null)
                    _holdingChecks = new List<ActionOnRegister>();
                _holdingChecks.Add(new ActionOnRegister(parameter, callback, activeValue, uLongCallback) { RaiseOlwais = raiseOlwais, CheckInterval = checkInterval, ResetAfterRead = resetAfterRead, ULongDefault = uLongDefault });
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isHolding"></param>
        /// <param name="index"></param>
        /// <param name="callback"></param>
        /// <param name="activeValue"></param>
        /// <param name="raiseOlwais"></param>
        /// <param name="checkInterval"></param>
        /// <returns>Reset action</returns>
        public Action SetActionOnRegisterDateTime(HomeServerSettings.ControllerGroup.Controller.Parameter parameter,
            Action<DateTime> callback,
            HomeServerSettings.ActiveValue activeValue,
            bool raiseOlwais = false, TimeSpan? checkInterval = null)
        {
            //            if (callback == null)
            //                throw new ArgumentNullException(nameof(callback));
            var isHolding = parameter.ModbusType ==
                                    HomeServerSettings.ControllerGroup.Controller.ModbusTypes.HoldingRegister;
            var endIndex = (ushort)(parameter.ModbusIndex + 2);
            if (!isHolding)
            {
                if (_inputRegisterMinIndex > parameter.ModbusIndex)
                    _inputRegisterMinIndex = parameter.ModbusIndex;
                if (_inputRegisterMaxIndex < endIndex)
                    _inputRegisterMaxIndex = endIndex;
                if (_inputChecks == null)
                    _inputChecks = new List<ActionOnRegister>();
                var holdingCheck = new ActionOnRegisterDateTime(parameter, callback, activeValue)
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
                if (_holdingRegisterMinIndex > parameter.ModbusIndex)
                    _holdingRegisterMinIndex = parameter.ModbusIndex;
                if (_holdingRegisterMaxIndex < endIndex)
                    _holdingRegisterMaxIndex = endIndex;
                if (_holdingChecks == null)
                    _holdingChecks = new List<ActionOnRegister>();
                _inputChecks.Add(new ActionOnRegisterDateTime(parameter, callback, activeValue)
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
            var newSetter = new ModbusSetter(setter, WriteToLog, result);
            _setters.Add(newSetter);
            return newSetter;
        }
        #region DataBase



        #endregion DataBase

    }
}
