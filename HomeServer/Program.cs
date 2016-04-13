using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using HomeServer.Models;
using HomeServer.Objects;
using uPLibrary.Networking.M2Mqtt;

namespace HomeServer
{
    class Program
    {
        private const string SettingsFileName = "HomeSettings.xml";

        private static HomeSettings _homeSettings;
        private static MqttClient _mqttClient;

        private static List<ShController> _allControllers;

        private static ClienSocketWorker _clientWorker;


        static void Main(string[] args)
        {
            var options = new Options();
            Options.Current = options;

            var parser = new CommandLineParser.CommandLineParser {AcceptSlash = false};
            parser.ExtractArgumentAttributes(options);
            parser.ShowUsageHeader = "Home Automation Server\n"
                + "Example:  -p COM7 -b 9600 -s tor -v\n";
            try
            {
                parser.ParseCommandLine(args);
                if(options.Verbose)
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
            ModbusMasterThread.Init(options.SerialPort, options.BaudRate);
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

        private static void CreateMqttClient(string serverAddress)
        {
            _clientWorker = new ClienSocketWorker(serverAddress, _allControllers, _homeSettings.ControllerGroups);
        }

        private static bool LoadSettings()
        {
            if (!File.Exists(SettingsFileName))
            {
                Console.WriteLine("Файл с настройками не найден!");
                Environment.Exit(1);
                return false;
            }
            try
            {
                _homeSettings = HomeSettings.LoadFromFile(SettingsFileName);
                return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                Environment.Exit(1);
            }
            return false;
        }

        private static void ApplySettings()
        {
            _allControllers = new List<ShController>();

            foreach (var controllerGroup in _homeSettings.ControllerGroups)
            {
                var controllerGroupObject = new ControllerGroup();
                if (ModbusMasterThread.ControolerObjects == null)
                    ModbusMasterThread.ControolerObjects = new List<ControllerGroup>();
                ModbusMasterThread.ControolerObjects.Add(controllerGroupObject);

                foreach (var controller in controllerGroup.Controllers)
                {
                    var controllerObject = new ShController(controller.Id, controller.ModbusAddress);
                    if (controllerGroupObject.ShControllers == null)
                        controllerGroupObject.ShControllers = new List<ShController>();
                    controllerGroupObject.ShControllers.Add(controllerObject);
                    _allControllers.Add(controllerObject);
                    foreach (var parameter in controller.Parameters)
                    {
                        CreateParameter(parameter, controllerObject);
                    }
                    foreach (var setter in controller.Setters)
                    {
                        CreateSetter(setter, controllerObject);
                    }
                    //                    ProcessPararmeters(controller.Parameters, layoutGroupObject, controllerObject);
                    //                    ProcessSetters(controller.Setters, layoutGroupObject, controllerObject);
                }
            }
        }
        private static void CreateParameter(HomeSettingsControllerGroupControllerParameter parameter,
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
                case ModbusTypes.Discrete:
                case ModbusTypes.Coil:
                    var isCoil = parameter.ModbusType == ModbusTypes.Coil;
                    reserAction = controllerObject.SetActionOnDiscreteOrCoil(isCoil,
                                       CheckCoilStatus.OnBoth,
                                       parameter.ModbusIndex,
                                       (state) =>
                                       {
                                           _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.BoolResult}", state.ToString(), true);
                                       }, parameter.BoolDefault);
                    if (Options.Current.Verbose)
                        Console.WriteLine($"Created DiscreteOrCoil    {parameter.Id} \t{parameter.Name}");
                    break;
                case ModbusTypes.InputRegister:
                case ModbusTypes.HoldingRegister:
                    switch (parameter.DataType)
                    {
                        case DataTypes.UInt16:
                            reserAction = controllerObject.SetActionOnRegister(false, parameter.ModbusIndex, value =>
                            {
                                _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.UInt16Result}", value.ToString(), true);
                            }, false, interval);
                            if (Options.Current.Verbose)
                                Console.WriteLine($"Created Register          {parameter.Id} \t{parameter.Name}");

                            break;
                        case DataTypes.Float:
                            break;
                        case DataTypes.RdDateTime:
                            reserAction = controllerObject.SetActionOnRegisterDateTime(false, parameter.ModbusIndex,
                                value =>
                                {
                                    _clientWorker.SendMessage($"{HsEnvelope.ControllersResult}/{parameter.Id}/{HsEnvelope.DateTimeResult}", value.ToString(HsEnvelope.DateTimeFormat), true);
                                }, false, interval);
                            if (Options.Current.Verbose)
                                Console.WriteLine($"Created DateTime Register {parameter.Id} \t{parameter.Name}");

                            break;
                        case DataTypes.RdTime:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                    //                default:
                    //                    throw new ArgumentOutOfRangeException();
            }
            if (reserAction != null)
                ClienSocketWorker.ParameterResets.Add(parameter.Id, reserAction);
        }

        private static void CreateSetter(HomeSettingsControllerGroupControllerSetter setter, ShController controllerObject)
        {
            switch (setter.Type)
            {
                case SetterTypes.RealDateTime:
                    var setterObj = controllerObject.SetSetter(setter.ModbusIndex, setter.Type);
                    ClienSocketWorker.Setters.Add(setter.Id, setterObj);
                    if (Options.Current.Verbose)
                        Console.WriteLine($"Created Setter            {setter.Id} \t{setter.Name}");

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
