//#define GENERATE_SCHEMA
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using HomeServer.Models;
using HomeServer.Models.Base;
using HomeServer.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace HomeServer
{
    class Program
    {
        private const string ServerSettingsFileName = "HomeServerSettings.json";

        private const ushort ModbusTrue = 0x00FF; // 0xFF00 (Big Endian)
        private const ushort ModbusFalse = 0x0000;

        private static HomeServerSettings _homeServerSettings;

        private static List<ShController> _allControllers;

        private static MqttClienWorker _clientWorker;

        public static bool IsLinux => Environment.OSVersion.Platform == PlatformID.Unix;

        static void Main(string[] args)
        {
#if GENERATE_SCHEMA
            var generator = new JSchemaGenerator {DefaultRequired = Required.Default};
            generator.GenerationProviders.Add(new StringEnumGenerationProvider());

            var schema = generator.Generate(typeof(HomeServerSettings));
            schema.Title = "JSON Schema for Home Server Settings";
            

            using (var file = File.CreateText(@"D:\Work\Home\SmartHome\Modbus\HomeServer\Models\HomeServerSettindsSchema.json"))
            using (var writer = new JsonTextWriter(file))
            {
                writer.Formatting = Formatting.Indented;
                schema.WriteTo(writer);
            }
#endif


            var options = new Options();
            Options.Current = options;

            var parser = new CommandLineParser.CommandLineParser { AcceptSlash = false };
            parser.ExtractArgumentAttributes(options);
            parser.ShowUsageHeader = "Home Automation Server\n"
                + "Example:  -p COM7 -b 9600 -s tor -v\n";
            try
            {
                parser.ParseCommandLine(args);
                if (options.Verbose)
                    parser.ShowParsedArguments();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
                return;
            }
            //            if (args.Length < 3)
            //            {
            //                Console.WriteLine("При запуске надо обязательно указать порт и скорость порта\nНапример: HomeServer COM7 9600");
            //                Environment.Exit(1);
            //            }
            //            _modbusMasterThread = new ModbusMasterThread("COM7", 9600);
            if (LoadSettings())
                ApplySettings();
            //            CreateSocket();
            try
            {
                CreateMqttClient(options.ServerAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine(options.Verbose ? e.ToString() : e.Message);
            }
            _clientWorker.UpdateSettings = s =>
            {
                try
                {
                    JsonConvert.DeserializeObject<HomeServerSettings>(s);
                    File.WriteAllText(SettingsPath, s);
                    //if(!ModbusMasterThread.IsPaused)
                    //    ModbusMasterThread.StopListening();
                    ModbusMasterThread.Close(true);
                    // Ждём 10 секунд пока поток остановится
                    //ModbusMasterThread.ThreadStopped.WaitOne(10000);
                    if (LoadSettings())
                    {
                        _clientWorker.SendMessage($"{HsEnvelope.ServerStatus}/{HsEnvelope.SendSettings}", true.ToString(), false);

                        ApplySettings();
                        ModbusMasterThread.Init(options.SerialPort, options.BaudRate, _homeServerSettings.HeartBeatMs);
                        if (!ModbusMasterThread.StartListening())
                        {
                            Environment.Exit(1);
                            return "Error starting listening!";
                        }
                    }
                }
                catch (Exception ee)
                {
                    return ee.Message;
                }
                return null;
            };
            ModbusMasterThread.ListeningChanged += (sender, b) =>
            {
                _clientWorker.SendMessage($"{HsEnvelope.ServerStatus}/{HsEnvelope.ListeningStatus}", b.ToString(), true);
            };
            ModbusMasterThread.SendControllerStatus += (sender, b) =>
            {
                SendControllerState(b.Item1, b.Item2);
            };
            ModbusMasterThread.Init(options.SerialPort, options.BaudRate, _homeServerSettings.HeartBeatMs);
            ModbusMasterThread.WriteToLog += (sender, s) =>
            {
                WriteToLog(s);
            };


            if (!ModbusMasterThread.StartListening())
            {
                Environment.Exit(1);
                return;
            }
            //new Server(11000);

            //            
        }

        private static void SendControllerState(string controllerName, bool state)
        {
            _clientWorker.SendMessage($"{HsEnvelope.ControllerStatus}/{controllerName}", state.ToString(), true);
        }
        private static void WriteToLog(string msg)
        {
            _clientWorker.SendMessage($"{HsEnvelope.ServerStatus}/{HsEnvelope.LogMessage}", msg, false);

        }

        private static void CreateMqttClient(string serverAddress)
        {
            _clientWorker = new MqttClienWorker(serverAddress, _allControllers, _homeServerSettings);
        }


        private static string SettingsPath => IsLinux ? Path.Combine("/etc", ServerSettingsFileName) : ServerSettingsFileName;

        private static bool LoadSettings()
        {
//            var settingsPath = SettingsFileName;
//            if (IsLinux)
//                settingsPath = Path.Combine("/etc", SettingsFileName);

            var serverSettingsPath = SettingsPath;


//            if (!File.Exists(settingsPath))
//            {
//                Console.WriteLine("Файл с настройками не найден!");
//                Environment.Exit(1);
//                return false;
//            }

            if (!File.Exists(serverSettingsPath))
            {
                Console.WriteLine($"Файл с настройками ({serverSettingsPath}) не найден!");
                Environment.Exit(1);
                return false;
            }
            try
            {
//                _homeSettings = HomeSettings.LoadFromFile(settingsPath);
/*                 _homeServerSettings = new HomeServerSettings();
               _homeServerSettings.ControllerGroups = new[]
                {
                    new HomeServerSettings.ControllerGroup
                    {
                        Name = "sd;lksdfk",
                        Controllers = new []
                        {
                            new HomeServerSettings.ControllerGroup.Controller
                            {
                                Id = "erwerw",
                                ModbusAddress = 3,
                                Name = "8240384094802394",
                                Parameters = new []
                                {
                                    new HomeServerSettings.ControllerGroup.Controller.Parameter
                                    {
                                        DataType = HomeServerSettings.ControllerGroup.Controller.DataTypes.ULong,
                                        Id = "dsfsdffsd"
                                    }
                                }
                            } 
                        }
                    }
                };
                var ttt = JsonConvert.SerializeObject(_homeServerSettings);*/
                
                _homeServerSettings = JsonConvert.DeserializeObject<HomeServerSettings>(File.ReadAllText(serverSettingsPath));
                return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
                Environment.Exit(1);
            }
            return false;
        }

        private static void ApplySettings()
        {
            ModbusMasterThread.ActiveValues = _homeServerSettings.ActiveValues;
            ModbusMasterThread.ControolerObjects.Clear();
            MqttClienWorker.ActiveValues = _homeServerSettings.ActiveValues;
            MqttClienWorker.ParameterResets.Clear();
            MqttClienWorker.Setters.Clear();
            _allControllers = new List<ShController>();
            if (Options.Current.Verbose)
            {
                Console.WriteLine($"{"Action",-25} {"Id",-30} {"Name",-50} Rate");
                Console.WriteLine(
                    $"~~~~~~~~~~~~~~~~~~~~~~~~~ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ ~~~~~~~~~~~~");
            }
            foreach (var controllerGroup in _homeServerSettings.ControllerGroups)
            {
                if (controllerGroup.Disabled)
                    continue;
                var controllerGroupObject = new ControllerGroup();
                if (ModbusMasterThread.ControolerObjects == null)
                    ModbusMasterThread.ControolerObjects = new List<ControllerGroup>();
                ModbusMasterThread.ControolerObjects.Add(controllerGroupObject);



                foreach (var controller in controllerGroup.Controllers)
                {
                    if (controller.Disabled)
                        continue;
                    var controllerObject = new ShController(controller.Id, controller.SlaveId, controller.ModbusAddress,
                        WriteToLog)
                    {
                        ControllerGroupName = controllerGroup.Name,
                        Name = controller.Name,
                        ActiveValues = _homeServerSettings.ActiveValues
                    };

                    if (controllerGroupObject.ShControllers == null)
                        controllerGroupObject.ShControllers = new List<ShController>();
                    controllerGroupObject.ShControllers.Add(controllerObject);
                    _allControllers.Add(controllerObject);
                    if(controller.Parameters != null)
                        foreach (var parameter in controller.Parameters)
                        {
                            CreateParameter(parameter, controllerObject);
                        }
                    if(controller.Setters != null)
                        foreach (var setter in controller.Setters)
                        {
                            CreateSetter(setter, controllerObject);
                        }
                    //                    ProcessPararmeters(controller.Parameters, layoutGroupObject, controllerObject);
                    //                    ProcessSetters(controller.Setters, layoutGroupObject, controllerObject);
                }
            }
        }

        private void WriteToBaseMumeric(HomeServerSettings.ControllerGroup.Controller.Parameter parameter,
            long? intValue = null, double? doubleValue = null)
        {
            var val = intValue ?? doubleValue ?? 0;
            if (parameter.WriteToBaseIntervalTime != null && parameter.NextTimeToWriteToBase <= DateTime.Now)
            {
                parameter.NextTimeToWriteToBase += parameter.WriteToBaseIntervalTime.Value;

                if (parameter.WriteToBaseMethod ==
                    HomeServerSettings.ControllerGroup.Controller.Parameter.WriteToBaseMethods.Average
                    && parameter.AverageValuesToWriteToBase != null && parameter.AverageValuesToWriteToBase.Count > 0)
                {
                    var avg = parameter.AverageValuesToWriteToBase.Sum();
                    avg /= parameter.AverageValuesToWriteToBase.Count;

                    parameter.AverageValuesToWriteToBase = null;
                    if (intValue != null)
                        BaseUtils.WriteParamToBase(parameter.Id, intValue: (long) Math.Round(avg));
                    else
                        BaseUtils.WriteParamToBase(parameter.Id, doubleValue: doubleValue);
                }
                else
                    BaseUtils.WriteParamToBase(parameter.Id, intValue: intValue, doubleValue: doubleValue);
            }
            else if(parameter.WriteToBaseMethod == HomeServerSettings.ControllerGroup.Controller.Parameter.WriteToBaseMethods.Average) // если счейчас писать не надо и режим среднего значения
            {
                if (parameter.AverageValuesToWriteToBase == null)
                    parameter.AverageValuesToWriteToBase = new List<double>();
                parameter.AverageValuesToWriteToBase.Add(val);
            }
        }

        private static void PrintOnConsole(string status, string id, string name, string refreshRate)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{status,-26}");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{id,-30}");
            Console.ResetColor();

            Console.WriteLine($" {name,-50} {refreshRate}");
        }
        private static void CreateParameter(HomeServerSettings.ControllerGroup.Controller.Parameter parameter,
                                            ShController controllerObject)
        {
            //            var indicatorControl = new IndicatorControl
            //            {
            //                Caption = parameter.Name
            //            };
            //            //                        roomTab.MainPanel.Items = new SerializableItemCollection();
            //            layoutGroupObject.Items.Add(indicatorControl);
            TimeSpan? interval = null;
            if (!string.IsNullOrEmpty(parameter.RefreshRate))
            {
                TimeSpan tmpVal;
                if (TimeSpan.TryParseExact(parameter.RefreshRate, "g", null, TimeSpanStyles.None, out tmpVal))
                {
                    interval = tmpVal;
                }
            }

            if (!string.IsNullOrEmpty(parameter.WriteToBaseInterval))
            {
                TimeSpan tmpVal;
                if (TimeSpan.TryParseExact(parameter.WriteToBaseInterval, "g", null, TimeSpanStyles.None, out tmpVal))
                    parameter.WriteToBaseIntervalTime = tmpVal;
            }

            Action resetAction = null;

            HomeServerSettings.ActiveValue currentActiveValue = null;
            if (_homeServerSettings.ActiveValues.ContainsKey(parameter.Id))
            {
                currentActiveValue = _homeServerSettings.ActiveValues[parameter.Id];
                
            }


            switch (parameter.ModbusType)
            {
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.Discrete:
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.Coil:
                    var isCoil = parameter.ModbusType == HomeServerSettings.ControllerGroup.Controller.ModbusTypes.Coil;
                    resetAction = controllerObject.SetActionOnDiscreteOrCoil(parameter, isCoil,
                                       CheckBoolStatus.OnBoth,
                                       parameter.ModbusIndex,
                                       (state) =>
                                       {
                                           if (_homeServerSettings.ActiveValues.ContainsKey(parameter.Id))
                                           {
                                               var av = _homeServerSettings.ActiveValues[parameter.Id];
                                               av.SetNewValue(state);
                                           }

                                           BaseUtils.WriteParamToBase(parameter.Id, boolValue: state);
                                           _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.BoolResult}", state.ToString(), true);
                                           if (parameter.Echo != null)
                                           {

                                               var arguments = string.Empty;
                                               if (parameter.Echo.Arguments != null &&
                                                   parameter.Echo.Arguments.Length > 0)
                                               {
                                                   var tmpArg = new List<string>();
                                                   foreach (var argument in parameter.Echo.Arguments)
                                                   {
                                                       switch (argument.Type)
                                                       {
                                                           case HomeServerSettings.EchoValue.Argument.ArgumentTypes.Literal:
                                                               tmpArg.Add(argument.Value.ToString());
                                                               break;
                                                           default:
                                                               throw new ArgumentOutOfRangeException();
                                                       }
                                                   }
                                                   arguments = "," + string.Join(",", tmpArg);
                                               }
                                               switch (parameter.Echo.Type)
                                               {

                                                   case HomeServerSettings.EchoValue.EchoTypes.Setter:
                                                       MqttClienWorker.ProceedSetValue(
                                                           new[] { HsEnvelope.HomeServerTopic, HsEnvelope.ControllersSetValue, parameter.Echo.Id},
                                                           state.ToString() + arguments);
//                                                       _clientWorker.SendMessage($"{HsEnvelope.ControllersSetValue}/{parameter.Echo.Id}", state.ToString(), false);

                                                       break;
                                                   default:
                                                       throw new ArgumentOutOfRangeException();
                                               }
                                           }
                                       }, currentActiveValue, parameter.BoolDefault);
                    if (Options.Current.Verbose)
                        PrintOnConsole("Created DiscreteOrCoil", parameter.Id, parameter.Name, parameter.RefreshRate);
                    break;
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.InputRegister:
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.HoldingRegister:
                    var isHolding = parameter.ModbusType == HomeServerSettings.ControllerGroup.Controller.ModbusTypes.HoldingRegister;
                    switch (parameter.DataType)
                    {
                        case HomeServerSettings.ControllerGroup.Controller.DataTypes.UInt16:
                            resetAction = controllerObject.SetActionOnRegister(parameter, isHolding, parameter.DataType, value =>
                            {
                                double resValue = value;
                                if (parameter.Multiple != 0)
                                    resValue *= parameter.Multiple;

                                if (_homeServerSettings.ActiveValues.ContainsKey(parameter.Id))
                                {
                                    var av = _homeServerSettings.ActiveValues[parameter.Id];
                                    av.SetNewValue(resValue);
                                }

                                if (parameter.WriteToBaseIntervalTime != null && parameter.NextTimeToWriteToBase <= DateTime.Now)
                                {
                                    parameter.NextTimeToWriteToBase += parameter.WriteToBaseIntervalTime.Value;
                                    BaseUtils.WriteParamToBase(parameter.Id, intValue: (long) resValue);
                                }
                                _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.UInt16Result}", resValue.ToString(CultureInfo.InvariantCulture), parameter.Retain);

                                if (parameter.Echo != null)
                                {
                                    switch (parameter.Echo.Type)
                                    {
                                        case HomeServerSettings.EchoValue.EchoTypes.Setter:
                                            MqttClienWorker.ProceedSetValue(
                                                new[] { HsEnvelope.HomeServerTopic, HsEnvelope.ControllersSetValue, parameter.Echo.Id },
                                                value.ToString());
                                            //                                                       _clientWorker.SendMessage($"{HsEnvelope.ControllersSetValue}/{parameter.Echo.Id}", state.ToString(), false);

                                            break;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }


                            }, null, currentActiveValue, false, interval, uInt16Default: parameter.UintDefault);
                            if (Options.Current.Verbose)
                                PrintOnConsole("Created Register", parameter.Id, parameter.Name, parameter.RefreshRate);
                            
                            break;
                        case HomeServerSettings.ControllerGroup.Controller.DataTypes.ModbusUInt16Bool:
                            resetAction = controllerObject.SetActionOnRegister(parameter, isHolding, parameter.DataType, value =>
                            {
                                var resValue = value == ModbusTrue;

                                if (parameter.WriteToBaseIntervalTime != null && parameter.NextTimeToWriteToBase <= DateTime.Now)
                                {
                                    parameter.NextTimeToWriteToBase += parameter.WriteToBaseIntervalTime.Value;
                                    BaseUtils.WriteParamToBase(parameter.Id, boolValue: resValue);
                                }
                                _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.BoolResult}", resValue.ToString(CultureInfo.InvariantCulture), parameter.Retain);
                            }, null, currentActiveValue, false, interval, uInt16Default: parameter.UintDefault);
                            if (Options.Current.Verbose)
                                PrintOnConsole("Created Register", parameter.Id, parameter.Name, parameter.RefreshRate);

                            break;
                        case HomeServerSettings.ControllerGroup.Controller.DataTypes.Double:
                            resetAction = controllerObject.SetActionOnRegister(parameter, isHolding, parameter.DataType, value =>
                            {
                                // Чтобы со знаком было
                                double resValue = (short)value;
                                if (parameter.Multiple != 0)
                                    resValue *= parameter.Multiple;

                                if (_homeServerSettings.ActiveValues.ContainsKey(parameter.Id))
                                {
                                    var av = _homeServerSettings.ActiveValues[parameter.Id];
                                    av.SetNewValue(resValue);
                                }



                                if (parameter.WriteToBaseIntervalTime != null && parameter.NextTimeToWriteToBase <= DateTime.Now)
                                {
                                    parameter.NextTimeToWriteToBase += parameter.WriteToBaseIntervalTime.Value;
                                    BaseUtils.WriteParamToBase(parameter.Id, doubleValue: resValue);
                                }
                                _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.DoubleResult}", resValue.ToString(CultureInfo.InvariantCulture), parameter.Retain);


                                if (parameter.Echo != null)
                                {
                                    switch (parameter.Echo.Type)
                                    {
                                        case HomeServerSettings.EchoValue.EchoTypes.Setter:
                                            MqttClienWorker.ProceedSetValue(
                                                new[] { HsEnvelope.HomeServerTopic, HsEnvelope.ControllersSetValue, parameter.Echo.Id },
                                                ((ushort)resValue).ToString());
                                            //                                                       _clientWorker.SendMessage($"{HsEnvelope.ControllersSetValue}/{parameter.Echo.Id}", state.ToString(), false);

                                            break;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }

                            }, null, currentActiveValue, false, interval, resetAfterRead: parameter.ResetAfterRead, doubleDefault: parameter.DoubleDefault);
                            if (Options.Current.Verbose)
                                PrintOnConsole("Created Double Register", parameter.Id, parameter.Name, parameter.RefreshRate);


                            break;
                        case HomeServerSettings.ControllerGroup.Controller.DataTypes.ULong:
                            resetAction = controllerObject.SetActionOnRegister(parameter, isHolding, parameter.DataType, null, value =>
                            {
                                double resValue = value;
                                if (parameter.Multiple != 0)
                                    resValue *= parameter.Multiple;

                                if (_homeServerSettings.ActiveValues.ContainsKey(parameter.Id))
                                {
                                    var av = _homeServerSettings.ActiveValues[parameter.Id];
                                    av.SetNewValue(resValue);
                                }



                                if (parameter.WriteToBaseIntervalTime != null && parameter.NextTimeToWriteToBase <= DateTime.Now)
                                {
                                    parameter.NextTimeToWriteToBase += parameter.WriteToBaseIntervalTime.Value;
                                    BaseUtils.WriteParamToBase(parameter.Id, intValue: (long) resValue);
                                }
                                _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.ULongResult}", resValue.ToString(CultureInfo.InvariantCulture), parameter.Retain);
                            }, currentActiveValue, false, interval, resetAfterRead: parameter.ResetAfterRead, uLongDefault: parameter.ULongDefault);
                            if (Options.Current.Verbose)
                                PrintOnConsole("Created Long Register", parameter.Id, parameter.Name, parameter.RefreshRate);

                            break;
                        case HomeServerSettings.ControllerGroup.Controller.DataTypes.RdDateTime:
                            resetAction = controllerObject.SetActionOnRegisterDateTime(parameter, false,
                                value =>
                                {
                                    _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.DateTimeResult}", value.ToString(HsEnvelope.DateTimeFormat), true);
                                }, currentActiveValue, false, interval);
                            if (Options.Current.Verbose)
                                PrintOnConsole("Created DateTime Register", parameter.Id, parameter.Name, parameter.RefreshRate);

                            break;
                        case HomeServerSettings.ControllerGroup.Controller.DataTypes.RdTime:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.DeviceId:
                    if (Options.Current.Verbose)
                        PrintOnConsole("Created DeviceId Register", parameter.Id, parameter.Name, parameter.RefreshRate);

                    controllerObject.SetActionOnSlaveId(value =>
                    {
                        _clientWorker.SendMessage(
                            $"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.StringResult}",
                            value, parameter.Retain);
                    });
                    break;
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.DeviceStatus:
                    resetAction = controllerObject.SetActionOnDeviceStatus(
                   CheckBoolStatus.OnBoth,
                   parameter.ModbusIndex,
                   (state) =>
                   {
                       _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.BoolResult}", state.ToString(), true);
                       if (parameter.Echo != null)
                       {
                           switch (parameter.Echo.Type)
                           {
                               case HomeServerSettings.EchoValue.EchoTypes.Setter:
                                   MqttClienWorker.ProceedSetValue(
                                       new[] { HsEnvelope.HomeServerTopic, HsEnvelope.ControllersSetValue, parameter.Echo.Id },
                                       state.ToString());
                                                       //                                                       _clientWorker.SendMessage($"{HsEnvelope.ControllersSetValue}/{parameter.Echo.Id}", state.ToString(), false);

                                                       break;
                               default:
                                   throw new ArgumentOutOfRangeException();
                           }
                       }
                   }, parameter.BoolDefault);
                    if (Options.Current.Verbose)
                        PrintOnConsole("Created DevStat Register", parameter.Id, parameter.Name, parameter.RefreshRate);

                    break;
                //                default:
                //                    throw new ArgumentOutOfRangeException();
            }
            if (resetAction != null)
                MqttClienWorker.ParameterResets.Add(parameter.Id, resetAction);
        }

        private static void CreateSetter(HomeServerSettings.ControllerGroup.Controller.Setter setter, ShController controllerObject)
        {
            var setterObj = controllerObject.SetSetter(setter, resultStatus =>
            {
                _clientWorker.SendMessage($"{HsEnvelope.ControllersSetterResult}/{setter.Id}", resultStatus.ToString(), setter.Retain);
                Console.WriteLine($"Set value '{setter.Id}' status  \t{resultStatus}");
            });
            MqttClienWorker.Setters.Add(setterObj);//setter.Id, 
            if (Options.Current.Verbose)
                PrintOnConsole("Created Setter", setter.Id, setter.Name, "");

            /*
                        switch (setter.Type)
                        {
                            case SetterTypes.RealDateTime:
                                var setterObj = controllerObject.SetSetter(setter.ModbusIndex, setter.Type, resultStatus =>
                                {
                                    _clientWorker.SendMessage($"{HsEnvelope.ControllersSetterResult}/{setter.Id}", resultStatus.ToString(), false);
                                    Console.WriteLine($"Set value '{setter.Id}' status  \t{resultStatus}");
                                });
                                MqttClienWorker.Setters.Add(setter.Id, setterObj);
                                if (Options.Current.Verbose)
                                    Console.WriteLine($"Created Setter            {setter.Id} \t{setter.Name}");

                                break;
                            case SetterTypes.UInt16:
                                var setterUInt16Obj = controllerObject.SetSetter(setter.ModbusIndex, setter.Type, resultStatus =>
                                {
                                    _clientWorker.SendMessage($"{HsEnvelope.ControllersSetterResult}/{setter.Id}", resultStatus.ToString(), false);
                                    Console.WriteLine($"Set value '{setter.Id}' status  \t{resultStatus}");
                                });
                                MqttClienWorker.Setters.Add(setter.Id, setterUInt16Obj);
                                if (Options.Current.Verbose)
                                    Console.WriteLine($"Created Setter            {setter.Id} \t{setter.Name}");

                                break;
                            case SetterTypes.File:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }*/
        }

    }
}
