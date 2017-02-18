using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using HomeServer.Models.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema;

namespace HomeServer.Models
{

    public partial class HomeServerSettings
    {

        public enum ActiveValueTypes
        {
            UInt,
            Int,
            Double,
            DateTime,
            Time,
            Bool,
            String
        }

        public class ActiveValue
        {
            /// <summary>
            /// Записывать в базу значение из интервала после предыдущей записи
            /// Last - писать последнее значение
            /// Average - вычислять среднее
            /// </summary>
            public enum WriteToBaseMethods
            {
                Last,
                Average
            }


            [Required]
            public string Id { get; set; }
            public object Value { get; private set; }
            public ActiveValueTypes ValueType { get; set; }
            public string Description { get; set; }

            public bool MqttRetain { get; set; }
            /// <summary>
            /// Сконвертированное значение
            /// </summary>
            [JsonConverter(typeof(MyTimeSpanConverter))]
            public TimeSpan? WriteToBaseInterval;
            public WriteToBaseMethods WriteToBaseMethod { get; set; }

            public DateTime NextTimeToWriteToBase = DateTime.MinValue;

            /// <summary>
            /// Устанавливает новое значение
            /// </summary>
            /// <param name="newValue"></param>
            /// <returns>Если значение изменилось, true</returns>
            public bool SetNewValue(object newValue)
            {
                if(Value != null && Value.Equals(newValue))
                    return false;
                Value = newValue;

                IsChanged = true;

                var resultTypeStr = "";
                switch (ValueType)
                {
                    case ActiveValueTypes.UInt:
                        resultTypeStr = HsEnvelope.UInt16Result;
                        break;
                    case ActiveValueTypes.Int:
                        resultTypeStr = HsEnvelope.UInt16Result;
                        break;
                    case ActiveValueTypes.Double:
                        resultTypeStr = HsEnvelope.DoubleResult;
                        break;
                    case ActiveValueTypes.DateTime:
                        break;
                    case ActiveValueTypes.Time:
                        break;
                    case ActiveValueTypes.Bool:
                        resultTypeStr = HsEnvelope.BoolResult;
                        break;
                    case ActiveValueTypes.String:
                        resultTypeStr = HsEnvelope.StringResult;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                MqttClienWorker.CurrentInstance.SendMessage($"{HsEnvelope.ControllersResult}/{Id}/{resultTypeStr}", Value.ToString(), MqttRetain);

                Console.WriteLine($"!!{Description} = {Value}");

                if (WriteToBaseInterval != null && NextTimeToWriteToBase <= DateTime.Now)
                {
                    NextTimeToWriteToBase += WriteToBaseInterval.Value;

                    switch (ValueType)
                    {
                        case ActiveValueTypes.UInt:
                            BaseUtils.WriteParamToBase(Id, intValue: (long)Value);
                            break;
                        case ActiveValueTypes.Int:
                            BaseUtils.WriteParamToBase(Id, intValue: (long)Value);
                            break;
                        case ActiveValueTypes.Double:
                            BaseUtils.WriteParamToBase(Id, doubleValue: (double)Value);
                            break;
                        case ActiveValueTypes.DateTime:

                            break;
                        case ActiveValueTypes.Time:
                            break;
                        case ActiveValueTypes.Bool:
                            BaseUtils.WriteParamToBase(Id, boolValue: (bool)Value);
                            break;
                        case ActiveValueTypes.String:
                            BaseUtils.WriteParamToBase(Id, stringValue: (string)Value);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }


                return true;
            }

            public void SetNewValueFromString(string newValue)
            {
                switch (ValueType)
                {
                    case ActiveValueTypes.UInt:
                    {
                        uint tempValue;
                        if (uint.TryParse(newValue, out tempValue))
                            SetNewValue(tempValue);
                    }
                        break;
                    case ActiveValueTypes.Int:
                    {
                        int tempValue;
                        if (int.TryParse(newValue, out tempValue))
                            SetNewValue(tempValue);
                    }
                        break;
                    case ActiveValueTypes.Double:
                        {
                            double tempValue;
                            if (double.TryParse(newValue.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out tempValue))
                                SetNewValue(tempValue);
                        }
                        break;
                    case ActiveValueTypes.DateTime:
                        break;
                    case ActiveValueTypes.Time:
                        break;
                    case ActiveValueTypes.Bool:
                        {
                            bool tempValue;
                            if (bool.TryParse(newValue, out tempValue))
                                SetNewValue(tempValue);
                            else
                            {
                                int tempIntValue;
                                if (int.TryParse(newValue, out tempIntValue))
                                    SetNewValue(tempIntValue != 0);
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            /// <summary>
            /// Изменено ли значение за последний цикл проверки параметров
            /// </summary>
            public bool IsChanged { get; private set; }

            public void ResetChange()
            {
                IsChanged = false;
            }

        }

        


        public class EchoValue
        {
            public enum EchoTypes
            {
                Setter
            }

            public class Argument
            {
                public enum ArgumentTypes
                {
                    Literal
                }
                public ArgumentTypes Type { get; set; }
                public object Value { get; set; }
            }

            public EchoTypes Type { get; set; }
            /// <summary>
            /// Id параметра, куда будет пересылаться значение
            /// </summary>
            [Description("Id параметра, куда будет пересылаться значение")]
            public string Id { get; set; }
            public Argument[] Arguments { get; set; }
        }

        public partial class ControllerGroup
        {
            public partial class Controller
            {
                public enum DataTypes
                {
                    /// <remarks/>
                    UInt16,
                    /// <summary>
                    /// Булевое значение MODBUS 0x0000 - false 0xFF00 - true 
                    /// </summary>
                    ModbusUInt16Bool,
                    /// <remarks/>
                    Double,
                    /// <remarks/>
                    ULong,
                    /// <remarks/>
                    RdDateTime,
                    /// <remarks/>
                    RdTime,
                }

                public enum ModbusTypes
                {
                    /// <remarks/>
                    Discrete,
                    /// <remarks/>
                    Coil,
                    /// <remarks/>
                    InputRegister,
                    /// <remarks/>
                    HoldingRegister,
                    SlaveId, // Идентификатор устройства на основании него присваивается адрес
                    /// <remarks/>
                    DeviceId,
                    DeviceStatus,
                }


                public partial class Parameter
                {

                    private ushort? _modbusIndex;

                    private bool? _boolDefault;

                    private ushort? _uintDefault;

                    private uint? _uLongDefault;

                    private bool? _resetAfterRead;

                    private bool? _retain;

                    public string Id { get; set; }

                    public string Name { get; set; }

                    public ModbusTypes ModbusType { get; set; }

                    public ushort ModbusIndex
                    {
                        get
                        {
                            return this._modbusIndex ?? default(byte);
                        }
                        set
                        {
                            this._modbusIndex = value;
                        }
                    }

                    public bool ModbusIndexSpecified
                    {
                        get
                        {
                            return this._modbusIndex.HasValue;
                        }
                        set
                        {
                            if (value == false)
                            {
                                this._modbusIndex = null;
                            }
                        }
                    }
                    [JsonConverter(typeof(MyTimeSpanConverter))]
                    public TimeSpan? RefreshRate { get; set; }
                    /// <summary>
                    /// Период записи результатов в базу
                    /// </summary>
//                    public string WriteToBaseInterval { get; set; }
//                    public WriteToBaseMethods WriteToBaseMethod { get; set; }
                    /// <summary>
                    /// Сконвертированное значение
                    /// </summary>
//                    public TimeSpan? WriteToBaseIntervalTime;

                    public DateTime NextTimeToWriteToBase = DateTime.MinValue;
                    public List<double> AverageValuesToWriteToBase;

                    public DataTypes DataType { get; set; }

                    public bool BoolDefault
                    {
                        get
                        {
                            return this._boolDefault.HasValue && this._boolDefault.Value;
                        }
                        set
                        {
                            this._boolDefault = value;
                        }
                    }

                    public bool BoolDefaultSpecified
                    {
                        get
                        {
                            return this._boolDefault.HasValue;
                        }
                        set
                        {
                            if (value == false)
                            {
                                this._boolDefault = null;
                            }
                        }
                    }

                    public ushort UintDefault
                    {
                        get
                        {
                            return this._uintDefault ?? default(ushort);
                        }
                        set
                        {
                            this._uintDefault = value;
                        }
                    }

                    public bool UintDefaultSpecified
                    {
                        get
                        {
                            return this._uintDefault.HasValue;
                        }
                        set
                        {
                            if (value == false)
                            {
                                this._uintDefault = null;
                            }
                        }
                    }

                    public uint ULongDefault
                    {
                        get
                        {
                            return this._uLongDefault ?? default(uint);
                        }
                        set
                        {
                            this._uLongDefault = value;
                        }
                    }

                    public bool ULongDefaultSpecified
                    {
                        get
                        {
                            return this._uLongDefault.HasValue;
                        }
                        set
                        {
                            if (value == false)
                            {
                                this._uLongDefault = null;
                            }
                        }
                    }

                    public double DoubleDefault { get; set; }

                    public bool ResetAfterRead
                    {
                        get
                        {
                            return this._resetAfterRead.HasValue && this._resetAfterRead.Value;
                        }
                        set
                        {
                            this._resetAfterRead = value;
                        }
                    }

                    public bool Retain
                    {
                        get
                        {
                            return this._retain.HasValue && this._retain.Value;
                        }
                        set
                        {
                            this._retain = value;
                        }
                    }

                    public string Value { get; set; }

                    /// <summary>
                    /// Коэффициент преобразования значения для числовых типов
                    /// </summary>
                    public double Multiple { get; set; }
                    public EchoValue Echo { get; set; }

                }

                public partial class Setter
                {
                    public class SendCommand
                    {
                        public class BitsValue
                        {
                            public enum BitsTypes
                            {
                                Zero, // = 0
                                Literal, // Hardcoded
                                FromRequest // Из строки MQTT
                            };
                            public int Index { get; set; }
                            public int Count { get; set; }
                            public BitsTypes Type { get; set; }
                            public int ParamIndex { get; set; }
                            public uint Value { get; set; }
                        }
                        public class ByteValue
                        {
                            public enum ByteTypes
                            {
                                Zero, // = 0
                                Literal, // Hardcoded
                                FromRequest, // Из строки MQTT
                                BitsFromRequest
                            }
                            public ByteTypes Type { get; set; }
                            public int ParamIndex { get; set; }
                            public byte Value { get; set; }
                            public string Conversion { get; set; }
                            public BitsValue[] Bits { get; set; }
                        }

                        public class WordValue
                        {
                            public enum WordTypes
                            {
                                Zero, // = 0
                                Literal, // Hardcoded
                                TwoBytesFromRequest, // Из строки MQTT
                                WordFromRequest
                            }
                            public ByteValue Hi { get; set; }
                            public ByteValue Lo { get; set; }
                            public WordTypes Type { get; set; }
                            public int ParamIndex { get; set; }
                            public ushort Value { get; set; }
                            public string Conversion { get; set; }
                        }

                        public bool IsSystem { get; set; }
                        public ByteValue Id { get; set; }
                        public ByteValue Data { get; set; }
                        public WordValue Additional1 { get; set; }
                        public WordValue Additional2 { get; set; }
                        public WordValue Additional3 { get; set; }

                    }
                    public enum SetterTypes
                    {
                        Bool,
                        /// <remarks/>
                        RealDateTime,
                        /// <remarks/>
                        UInt16,
                        /// <remarks/>
                        MultipleUInt16,
                        /// <remarks/>
                        File,
                        Command
                    }
                    public string Id { get; set; }
                    public string Name { get; set; }
                    public SetterTypes Type { get; set; }
                    public ushort ModbusIndex { get; set; }
                    public SendCommand Command { get; set; }
                    /// <summary>
                    /// Запомнить результат установки
                    /// </summary>
                    public bool Retain { get; set; }
                }

                public bool Disabled { get; set; }
                public Parameter[] Parameters { get; set; }
                public Setter[] Setters { get; set; }
                public string Id { get; set; }
                public string Name { get; set; }
                public string SlaveId { get; set; }
                public byte ModbusAddress { get; set; }

            }
            public bool Disabled { get; set; }

            public Controller[] Controllers { get; set; }
            public string Name { get; set; }
        }

        public class Plugin
        {
            public string Type { get; set; }

//            public class Parameter
//            {
//                public string Name { get; set; }
//                public Dictionary<string, string> Params { get; set; }
//            }

            public Dictionary<string, string> Params { get; set; }

        }


        public List<ActiveValue> ActiveValues { get; set; }
//        public Dictionary<string, ActiveValue> ActiveValues { get; set; }

        public HomeServerSettings()
        {
//            ActiveValues = new Dictionary<string, ActiveValue>();
            ActiveValues = new List<ActiveValue>();
            this.HeartBeatMs = 1000;
        }

        public ControllerGroup[] ControllerGroups { get; set; }

        public Plugin[] Plugins { get; set; }
        public int HeartBeatMs { get; set; }
    }

    public class MyTimeSpanConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
    JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception($"Unexpected token parsing date. Expected Integer, got {reader.TokenType}.");
            }

            var strValue = (string)reader.Value;

            TimeSpan tmpVal;
            if (TimeSpan.TryParseExact(strValue, "g", null, TimeSpanStyles.None, out tmpVal))
                return tmpVal;

            return null;



//            if (reader.TokenType != JsonToken.Integer)
//            {
//                throw new Exception($"Unexpected token parsing date. Expected Integer, got {reader.TokenType}.");
//            }
//
//            var seconds = (long)reader.Value;
//
//            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);//DateTime(1970, 1, 1)
//
//            date = date.AddSeconds(seconds).ToLocalTime();
//            return date;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TimeSpan) == objectType;
        }

        public override void WriteJson(JsonWriter writer, object value,
    JsonSerializer serializer)
        {
            var ts = (TimeSpan)value;


            writer.WriteStartObject();
//            writer.WritePropertyName("d");
//            writer.WriteValue(ts.Days);
            writer.WritePropertyName("h");
            writer.WriteValue(ts.Hours);
            writer.WritePropertyName("m");
            writer.WriteValue(ts.Minutes);
            writer.WritePropertyName("s");
            writer.WriteValue(ts.Seconds);
            writer.WriteEndObject();



//            long ticks;
//            if (value is DateTime)
//            {
//                var epoc = new DateTime(1970, 1, 1);
//                var delta = ((DateTime)value) - epoc;
//                if (delta.TotalSeconds < 0)
//                {
//                    throw new ArgumentOutOfRangeException("Unix epoc starts January 1st, 1970");
//                }
//                ticks = (long)delta.TotalSeconds;
//            }
//            else
//            {
//                throw new Exception("Expected date object value.");
//            }
//            writer.WriteValue(ticks);
        }
    }

    public static class ActiveValueExtension  
    {
        public static HomeServerSettings.ActiveValue Find(this IEnumerable<HomeServerSettings.ActiveValue> list, string id)
        {
            return list.FirstOrDefault(av => av.Id == id);
        }
    }
}
