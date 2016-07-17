using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using HomeServer.Models;
using HomeServer.Objects;
using Newtonsoft.Json;

namespace HomeServer
{
    class Program
    {
        private const string ServerSettingsFileName = "HomeServerSettings.json";

        private static HomeServerSettings _homeServerSettings;

        private static List<ShController> _allControllers;

        private static MqttClienWorker _clientWorker;

        public static bool IsLinux => Environment.OSVersion.Platform == PlatformID.Unix;

        static void Main(string[] args)
        {
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
            ModbusMasterThread.Init(options.SerialPort, options.BaudRate, _homeServerSettings.HeartBeatMs);
            ModbusMasterThread.WriteToLog += (sender, s) =>
            {
                WriteToLog(s);
            };
            //            CreateSocket();
            try
            {
                CreateMqttClient(options.ServerAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine(options.Verbose ? e.ToString() : e.Message);
            }
            if (!ModbusMasterThread.StartListening())
            {
                Environment.Exit(1);
                return;
            }
            //new Server(11000);

            //            
        }

        private static void WriteToLog(string msg)
        {
            _clientWorker.SendMessage($"{HsEnvelope.LogMessage}", msg, false);

        }

        private static void CreateMqttClient(string serverAddress)
        {
            _clientWorker = new MqttClienWorker(serverAddress, _allControllers, _homeServerSettings.ControllerGroups);
        }

        private static bool LoadSettings()
        {
//            var settingsPath = SettingsFileName;
//            if (IsLinux)
//                settingsPath = Path.Combine("/etc", SettingsFileName);

            var serverSettingsPath = ServerSettingsFileName;
            if (IsLinux)
                serverSettingsPath = Path.Combine("/etc", ServerSettingsFileName);


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
                        Name = controller.Name
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
                    interval = tmpVal;
            }

            Action reserAction = null;

            switch (parameter.ModbusType)
            {
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.Discrete:
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.Coil:
                    var isCoil = parameter.ModbusType == HomeServerSettings.ControllerGroup.Controller.ModbusTypes.Coil;
                    reserAction = controllerObject.SetActionOnDiscreteOrCoil(isCoil,
                                       CheckBoolStatus.OnBoth,
                                       parameter.ModbusIndex,
                                       (state) =>
                                       {
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
                                                           case HomeServerSettings.ControllerGroup.Controller.Parameter.EchoValue.Argument.ArgumentTypes.Literal:
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

                                                   case HomeServerSettings.ControllerGroup.Controller.Parameter.EchoValue.EchoTypes.Setter:
                                                       MqttClienWorker.ProceedSetValue(
                                                           new[] { HsEnvelope.HomeServerTopic, HsEnvelope.ControllersSetValue, parameter.Echo.Id},
                                                           state.ToString() + arguments);
//                                                       _clientWorker.SendMessage($"{HsEnvelope.ControllersSetValue}/{parameter.Echo.Id}", state.ToString(), false);

                                                       break;
                                                   default:
                                                       throw new ArgumentOutOfRangeException();
                                               }
                                           }
                                       }, parameter.BoolDefault);
                    if (Options.Current.Verbose)
                        Console.WriteLine($"Created DiscreteOrCoil    {parameter.Id,-30} {parameter.Name,-50} {parameter.RefreshRate}");
                    break;
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.InputRegister:
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.HoldingRegister:
                    var isHolding = parameter.ModbusType == HomeServerSettings.ControllerGroup.Controller.ModbusTypes.HoldingRegister;
                    switch (parameter.DataType)
                    {
                        case HomeServerSettings.ControllerGroup.Controller.DataTypes.UInt16:
                            reserAction = controllerObject.SetActionOnRegister(isHolding, parameter.ModbusIndex, parameter.DataType, value =>
                            {
                                double resValue = value;
                                if (parameter.Multiple != 0)
                                    resValue *= parameter.Multiple;

                                _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.UInt16Result}", resValue.ToString(CultureInfo.InvariantCulture), parameter.Retain);
                            }, null, false, interval, uInt16Default: parameter.UintDefault);
                            if (Options.Current.Verbose)
                                Console.WriteLine($"Created Register          {parameter.Id,-30} {parameter.Name,-50} {parameter.RefreshRate}");

                            break;
                        case HomeServerSettings.ControllerGroup.Controller.DataTypes.Double:
                            reserAction = controllerObject.SetActionOnRegister(isHolding, parameter.ModbusIndex, parameter.DataType, value =>
                            {
                                double resValue = value;
                                if (parameter.Multiple != 0)
                                    resValue *= parameter.Multiple;
                                _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.DoubleResult}", resValue.ToString(CultureInfo.InvariantCulture), parameter.Retain);
                            }, null, false, interval, resetAfterRead: parameter.ResetAfterRead, doubleDefault: parameter.DoubleDefault);
                            if (Options.Current.Verbose)
                                Console.WriteLine($"Created Double Register   {parameter.Id,-30} {parameter.Name,-50} {parameter.RefreshRate}");


                            break;
                        case HomeServerSettings.ControllerGroup.Controller.DataTypes.ULong:
                            reserAction = controllerObject.SetActionOnRegister(isHolding, parameter.ModbusIndex, parameter.DataType, null, value =>
                            {
                                double resValue = value;
                                if (parameter.Multiple != 0)
                                    resValue *= parameter.Multiple;
                                _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.ULongResult}", resValue.ToString(CultureInfo.InvariantCulture), parameter.Retain);
                            }, false, interval, resetAfterRead: parameter.ResetAfterRead, uLongDefault: parameter.ULongDefault);
                            if (Options.Current.Verbose)
                                Console.WriteLine($"Created Long Register     {parameter.Id,-30} {parameter.Name,-50} {parameter.RefreshRate}");

                            break;
                        case HomeServerSettings.ControllerGroup.Controller.DataTypes.RdDateTime:
                            reserAction = controllerObject.SetActionOnRegisterDateTime(false, parameter.ModbusIndex,
                                value =>
                                {
                                    _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.DateTimeResult}", value.ToString(HsEnvelope.DateTimeFormat), true);
                                }, false, interval);
                            if (Options.Current.Verbose)
                                Console.WriteLine($"Created DateTime Register {parameter.Id,-30} {parameter.Name,-50} {parameter.RefreshRate}");

                            break;
                        case HomeServerSettings.ControllerGroup.Controller.DataTypes.RdTime:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.DeviceId:
                    if (Options.Current.Verbose)
                        Console.WriteLine($"Created DeviceId Reseiver {parameter.Id,-30} {parameter.Name,-50}");

                    controllerObject.SetActionOnSlaveId(value =>
                    {
                        _clientWorker.SendMessage(
                            $"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.StringResult}",
                            value, parameter.Retain);
                    });
                    break;
                case HomeServerSettings.ControllerGroup.Controller.ModbusTypes.DeviceStatus:
                    reserAction = controllerObject.SetActionOnDeviceStatus(
                   CheckBoolStatus.OnBoth,
                   parameter.ModbusIndex,
                   (state) =>
                   {
                       _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.BoolResult}", state.ToString(), true);
                       if (parameter.Echo != null)
                       {
                           switch (parameter.Echo.Type)
                           {
                               case HomeServerSettings.ControllerGroup.Controller.Parameter.EchoValue.EchoTypes.Setter:
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
                        Console.WriteLine($"Created DevStat Reseiver  {parameter.Id,-30} {parameter.Name,-50}");

                    break;
                //                default:
                //                    throw new ArgumentOutOfRangeException();
            }
            if (reserAction != null)
                MqttClienWorker.ParameterResets.Add(parameter.Id, reserAction);
        }

        private static void CreateSetter(HomeServerSettings.ControllerGroup.Controller.Setter setter, ShController controllerObject)
        {
            var setterObj = controllerObject.SetSetter(setter, resultStatus =>
            {
                _clientWorker.SendMessage($"{HsEnvelope.ControllersSetterResult}/{setter.Id}", resultStatus.ToString(), false);
                Console.WriteLine($"Set value '{setter.Id}' status  \t{resultStatus}");
            });
            MqttClienWorker.Setters.Add(setter.Id, setterObj);
            if (Options.Current.Verbose)
                Console.WriteLine($"Created Setter            {setter.Id,-30} {setter.Name,-50}");

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
