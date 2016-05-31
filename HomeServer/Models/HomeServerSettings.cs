using System;
using System.Collections.Generic;

namespace HomeServer.Models
{

    public partial class HomeServerSettings
    {
        public partial class ControllerGroup
        {
            public partial class Controller
            {
                public enum DataTypes
                {
                    /// <remarks/>
                    UInt16,
                    /// <remarks/>
                    Float,
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
                    /// <remarks/>
                    DeviceId,
                    DeviceStatus,
                }


                public partial class Parameter
                {
                    public class EchoValue
                    {
                        public enum EchoTypes
                        {
                            Setter
                        }

                        public EchoTypes Type { get; set; }
                        public string Id { get; set; }
                    }

                    private byte? _modbusIndex;

                    private bool? _boolDefault;

                    private ushort? _uintDefault;

                    private uint? _uLongDefault;

                    private bool? _resetAfterRead;

                    private bool? _retain;

                    public string Id { get; set; }

                    public string Name { get; set; }

                    public ModbusTypes ModbusType { get; set; }

                    public byte ModbusIndex
                    {
                        get {
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

                    public string RefreshRate { get; set; }

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
                        get {
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

                    public bool ResetAfterReadSpecified
                    {
                        get
                        {
                            return this._resetAfterRead.HasValue;
                        }
                        set
                        {
                            if (value == false)
                            {
                                this._resetAfterRead = null;
                            }
                        }
                    }

                    public bool Retain
                    {
                        get {
                            return this._retain.HasValue && this._retain.Value;
                        }
                        set
                        {
                            this._retain = value;
                        }
                    }

                    public bool RetainSpecified
                    {
                        get
                        {
                            return this._retain.HasValue;
                        }
                        set
                        {
                            if (value == false)
                            {
                                this._retain = null;
                            }
                        }
                    }

                    public string Value { get; set; }

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
                    public byte ModbusIndex { get; set; }
                    public SendCommand Command { get; set; }
                }


                public Parameter[] Parameters { get; set; }
                public Setter[] Setters { get; set; }
                public string Id { get; set; }
                public string Name { get; set; }
                public byte ModbusAddress { get; set; }
            }
            public Controller[] Controllers { get; set; }
            public string Name { get; set; }
        }

        public HomeServerSettings()
        {
            this.HeartBeatMs = 1000;
        }

        public ControllerGroup[] ControllerGroups { get; set; }
        public int HeartBeatMs { get; set; }
    }


}
