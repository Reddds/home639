using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Chip45Programmer;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Gauges;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using HomeModbus.Controls;
using HomeModbus.Models;
using HomeModbus.Models.Base;
using HomeModbus.Objects;
using HomeModbus.Properties;
using HomeModbus.Tooltip;
using log4net;using log4net.Config;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Taskbar;
using Newtonsoft.Json;
using Visibility = HomeModbus.Models.Visibility;

// Import log4net classes.

namespace HomeModbus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));

        private const string SettingsFileName = "HomeSettings.xml";
        private HomeSettings _homeSettings;

        private const string ValuesLogLogFileName = "ValuesLog.json";
        private readonly ModbusMasterThread _modbusMasterThread;

        const double BrightStep = 100 / 15d;

        private readonly CollectionViewSource _demoView = new CollectionViewSource();

        /// <summary>
        /// Включён ли режим опроса
        /// </summary>
        private bool _isListening;

        private class DemoCommand
        {
            public bool Led1 { get; set; }
            public bool Led2 { get; set; }
            public bool Led3 { get; set; }
            public bool Led4 { get; set; }
            /// <summary>
            /// Яркость
            /// </summary>
            public byte Brightnes { get; set; }
            /// <summary>
            /// Задержка в секундах
            /// </summary>
            public bool DelayInSec { get; set; }
            public byte Delay { get; set; }

            public int BytesCount => DelayInSec ? 3 : 2;

            public byte[] ToBytes()
            {
                var outBytes = new List<byte>();
                var ledMask = (Led1 ? 0x1 : 0)
                              | (Led2 ? 0x2 : 0)
                              | (Led3 ? 0x4 : 0)
                              | (Led4 ? 0x8 : 0);
                var ledByte = (byte)((ledMask << 4) + Brightnes);

                outBytes.Add(ledByte);
                if (DelayInSec)
                {
                    outBytes.Add(0xff);
                    outBytes.Add(Delay);
                }
                else
                {
                    outBytes.Add(Delay);
                }
                return outBytes.ToArray();
            }

            public override string ToString()
            {
                return
                    $@"СветодиодЫ № {(Led1 ? "1 " : "")} {(Led2 ? "2 " : "")} {(Led3 ? "3 " : "")} {(Led4 ? "4 " : "")} на {Brightnes * BrightStep:F1}% и пауза {(DelayInSec ? Delay : Delay * 10)} {(DelayInSec ? "с" : "мс")}";
            }
        };

        private readonly int[] _shortBaudrates =
        {
            230400,
            115200,
            76800,
            57600,
            38400,
            28800,
            19200,
            14400,
            9600,
            4800,
            2400
        };

        readonly List<DemoCommand> _demoCommands = new List<DemoCommand>();


        private readonly FancyBalloon _moveDetectedBaloon;
        private readonly FancyBalloon _callBalloon;
        private BalloonStack _balloonStack;
        private int _bedroomTemp;
        private int _bedroomHym;

        private List<LogValues> _mainValueLog;

        public int BedroomTemp
        {
            get { return _bedroomTemp; }
            set
            {
                _bedroomTemp = value;
                OnPropertyChanged(nameof(BedroomTemp));
            }
        }

        public int BedroomHym
        {
            get { return _bedroomHym; }
            set
            {
                _bedroomHym = value;
                OnPropertyChanged(nameof(BedroomHym));
            }
        }

        private void WriteToLog(string msg)
        {
            Log.Info(msg);

            ExecDispatched(() =>
            {
                LbLog.Items.Add(msg);
                LbLog.SelectedIndex = LbLog.Items.Count - 1;
                LbLog.ScrollIntoView(LbLog.SelectedItem);
            });
        }

        private void ExecDispatched(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.Background,
              action);
        }



        public MainWindow()
        {
            XmlConfigurator.Configure();

            InitializeComponent();




            LbBaudRates.ItemsSource = _shortBaudrates;
            LbBaudRates.SelectedItem = Settings.Default.SelectedBaudrate;

            FillPorts();

            var comPortTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 5) };
            comPortTimer.Tick += (sender, args) =>
            {
                FillPorts();
            };
            comPortTimer.Start();


            LoadValuesLog();


            _balloonStack = new BalloonStack();
            //            _moveDetectedBaloon = new FancyBalloon("Движение!", FancyBalloon.BaloonStyles.Alarm);
            //_balloonStack.AddBaloon(_moveDetectedBaloon);

            //            _callBalloon = new FancyBalloon("Вызов!", FancyBalloon.BaloonStyles.Exclamation);
            //            _callBalloon.Closing += (sender, args) =>
            //            {
            //                _modbusMasterThread.ResetCall();
            //
            //            };
            //_balloonStack.AddBaloon(_callBalloon);

            MyNotifyIcon.ShowCustomBalloon(_balloonStack, PopupAnimation.Slide, null);




            var bright = new Dictionary<string, int>();
            var p = 100d;
            for (var i = 0xf; i >= 0; i--)
            {
                var percent = p.ToString("F1") + "%";
                p -= BrightStep;
                bright.Add(percent, i);
            }
            CbBrightnes.ItemsSource = bright;
            CbBrightnes.SelectedIndex = 0;

            _demoView.Source = _demoCommands;
            LbDemoCommands.ItemsSource = _demoView.View;


            _modbusMasterThread = new ModbusMasterThread();


            if (LoadSettings())
                ApplySettings();



/*
            var corridor = new Corridor();
            //            _modbusMasterThread.ControolerObjects.Add(corridor);
            corridor.LightInCorridorChanged += (sender, lightState) =>
            {
                ExecDispatched(() =>
                {
                    if (lightState)
                    {
                        if (_moveDetectedBaloon.Parent == null)
                            ShowBalloon(_moveDetectedBaloon);
                        Background = Brushes.Crimson;
                        // This will highlight the icon in red
                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                        // to highlight the entire icon
                        TaskbarManager.Instance.SetProgressValue(100, 100);
                    }
                    else
                    {
                        if (_moveDetectedBaloon.Parent != null)
                            _moveDetectedBaloon.Close();
                        Background = Brushes.White;
                        // This will highlight the icon in red
                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                        // to highlight the entire icon
                        //TaskbarManager.Instance.SetProgressValue(100, 100);
                    }
                });
            };
*/
            _modbusMasterThread.PortChanged += (sender, args) =>
            {
                OnPropertyChanged("IsPortOpen");
            };
            _modbusMasterThread.ListeningChanged += (sender, isListening) =>
            {
                IsListening = isListening;
            };
            _modbusMasterThread.WriteToLog += (sender, msg) =>{
                WriteToLog(msg);
            };
        }

        private void LoadValuesLog()
        {
            //            if (!File.Exists(ValuesLogLogFileName))
            //            {
            //                _mainValueLog = new List<LogValues>();
            //                return;
            //            }
            //
            //            _mainValueLog = JsonConvert.DeserializeObject<List<LogValues>>(File.ReadAllText(ValuesLogLogFileName));
            using (var db = new ValueLogContext())
            {

                var valueLog = new ValueLog { Time = DateTime.Now, ParameterId = "test1212", Value = 9238745982374923 };
                db.ValueLogs.Add(valueLog);
                db.SaveChanges();

                // Display all Blogs from the database 
                var query = from b in db.ValueLogs
                            select b;
                var tstr = string.Empty;
                foreach (var item in query)
                {
                    tstr += item.Time + Environment.NewLine;
                }
                MessageBox.Show(tstr);
            }
        }


        private ImageSource GetImageSource(string imageName, string defaultImage = null, double height = -1)
        {
            var workDir = AppDomain.CurrentDomain.BaseDirectory;
            var imagesPath = Path.Combine(workDir, @"Assets\Images\Objects");
            var imagePath = Path.Combine(imagesPath, imageName);
            if (!File.Exists(imagePath))
            {
                if (defaultImage == null)
                    return null;
                imagePath = Path.Combine(imagesPath, defaultImage);
                if (!File.Exists(imagePath))
                {
                    return null;
                }
            }
            var bmp = new BitmapImage(new Uri(imagePath));
            bmp.Freeze();
            return bmp;
        }

        private void ApplySettings()
        {
            var allControllers = new List<ShController>();

            foreach (var controllerGroup in _homeSettings.ControllerGroups)
            {
                var controllerGroupObject = new ControllerGroup();
                if (_modbusMasterThread.ControolerObjects == null)
                    _modbusMasterThread.ControolerObjects = new List<ControllerGroup>();
                _modbusMasterThread.ControolerObjects.Add(controllerGroupObject);

                foreach (var controller in controllerGroup.Controllers)
                {
                    var controllerObject = new ShController(controller.Id, controller.ModbusAddress);
                    if(controllerGroupObject.ShControllers == null)
                        controllerGroupObject.ShControllers = new List<ShController>();
                    controllerGroupObject.ShControllers.Add(controllerObject);
                    allControllers.Add(controllerObject);
                    //                    ProcessPararmeters(controller.Parameters, layoutGroupObject, controllerObject);
                    //                    ProcessSetters(controller.Setters, layoutGroupObject, controllerObject);

                }
            }

            foreach (var room in _homeSettings.Rooms)
            {
                var roomTab = new RoomTab { Header = room.Name };
                MainTabs.Items.Add(roomTab);

                foreach (var layoutGroup in room.Layout.LayoutGroup)
                {
                    ProcessLayoutGroup(roomTab.MainPanel, layoutGroup, allControllers, _homeSettings.ControllerGroups);
                }
                foreach (var visibility in room.Layout.Visibility)
                {
                    ProcessVisibility(roomTab.MainPanel, visibility, allControllers, _homeSettings.ControllerGroups);
                }}


/*
            foreach (var room in _homeSettings.Rooms)
            {
                var roomTab = new RoomTab { Header = room.Name };
                //                Closed += (sender, args) =>
                //                {
                //                    roomTab.DlmMainLayout.SaveLayoutToXml(Guid.NewGuid().ToString() + "_layout.xml");
                //                };
                MainTabs.Items.Add(roomTab);

                var controllerGroupObject = new ControllerGroup();
                if (_modbusMasterThread.ControolerObjects == null)
                    _modbusMasterThread.ControolerObjects = new List<ControllerGroup>();
                _modbusMasterThread.ControolerObjects.Add(controllerGroupObject);
                controllerGroupObject.ShControllers = new List<ShController>();
                foreach (var controller in room.Controllers)
                {
                    var controllerObject = new ShController(controller.Controller.ModbusAddress);
                    controllerGroupObject.ShControllers.Add(controllerObject);
                    ProcessPararmeters(controller.Controller.Parameters, layoutGroupObject, controllerObject);
                    ProcessSetters(controller.Controller.Setters, layoutGroupObject, controllerObject);

                }
            }
*/
        }

        private void ProcessVisibility(DevExpress.Xpf.Docking.LayoutGroup layoutGroupObject, Visibility visibility, List<ShController> allControllers, List<HomeSettingsControllerGroup> controllerGroups)
        {
            // Ищем соответствующий параметр
            if (!string.IsNullOrEmpty(visibility.ParameterId))
            {
                foreach (var controllerGroup in controllerGroups)
                {
                    foreach (var controller in controllerGroup.Controllers)
                    {
                        foreach (var parameter in controller.Parameters)
                        {
                            if (parameter.Id == visibility.ParameterId)
                            {
                                var controllerObject = allControllers.FirstOrDefault(c => c.Id == controller.Id);
                                if (controllerObject == null)
                                {
                                    return;
                                }
                                CreateVisibilityParameter(layoutGroupObject, visibility, parameter, controllerObject);
                                return;
                            }
                        }
                    }
                }
            }
            // Ищем соответствующий setter
            else if (!string.IsNullOrEmpty(visibility.SetterId))
            {
                foreach (var controllerGroup in controllerGroups)
                {
                    foreach (var controller in controllerGroup.Controllers)
                    {
                        foreach (var setter in controller.Setters)
                        {
                            if (setter.Id == visibility.SetterId)
                            {
                                var controllerObject = allControllers.FirstOrDefault(c => c.Id == controller.Id);
                                if (controllerObject == null)
                                {
                                    return;
                                }
                                CreateVisibilitySetter(layoutGroupObject, visibility, setter, controllerObject);
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void CreateVisibilitySetter(DevExpress.Xpf.Docking.LayoutGroup layoutGroupObject, Visibility visibility, HomeSettingsControllerGroupControllerSetter setter, ShController controllerObject)
        {
            var setterControl = new IndicatorControl
            {
                Caption = setter.Name
            };
            layoutGroupObject.Add(setterControl);
            switch (setter.Type)
            {
                case SetterTypes.RealDateTime:
                    var btn = new Button { Content = "Установить текущее время" };
                    var setterObj = controllerObject.SetSetter(setter.ModbusIndex, SetterTypes.RealDateTime);
                    btn.Click += (sender, args) =>
                    {
                        setterObj.PendingSet();
                    };
                    setterControl.SpMain.Children.Add(btn);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CreateVisibilityParameter(DevExpress.Xpf.Docking.LayoutGroup layoutGroupObject, Visibility visibility, HomeSettingsControllerGroupControllerParameter parameter, ShController controllerObject)
        {
            var indicatorControl = new IndicatorControl
            {
                Caption = parameter.Name
            };
            //                        roomTab.MainPanel.Items = new SerializableItemCollection();
            layoutGroupObject.Items.Add(indicatorControl);
            if (parameter.Icon != null)
            {
                indicatorControl.CaptionImage = GetImageSource(parameter.Icon);
                indicatorControl.ShowCaptionImage = true;
            }
            switch (parameter.ModbusType)
            {
                case ModbusTypes.Discrete:
                case ModbusTypes.Coil:
                    //                                var simpleCheckBox = new CheckBox() { Content = parameter.Name };
                    ObjectButton binaryIndicator = null;
                    //                                indicatorControl.SpMain.Children.Add(simpleCheckBox);
                    var isCoil = parameter.ModbusType == ModbusTypes.Coil;
                    FancyBalloon dutyBalloon = null;
                    var showWhileParameterSet = false;
                    {
                        if (visibility.ShowBalloon != null)
                        {
                            var balloonSettings = visibility.ShowBalloon;
                            if (balloonSettings.ShowWhileParameterSetSpecified && balloonSettings.ShowWhileParameterSet)
                            {
                                //                                            dutyBalloon = new FancyBalloon(balloonSettings.Text1, GetBalloonStyle(balloonSettings.Type));
                                showWhileParameterSet = true;
                            }
                        }
                        var binaryIndicatorSettings = visibility.BinaryIndicator;
                        if (binaryIndicatorSettings != null)
                        {
                            binaryIndicator = new ObjectButton();
                            indicatorControl.SpMain.Children.Add(binaryIndicator);
                            var onIcon = string.IsNullOrEmpty(binaryIndicatorSettings.OnIcon)
                                ? "DefaultChecked.png"
                                : binaryIndicatorSettings.OnIcon;
                            var offIcon = string.IsNullOrEmpty(binaryIndicatorSettings.OffIcon)
                                ? "DefaultUnChecked.png"
                                : binaryIndicatorSettings.OffIcon;

                            binaryIndicator.EnabledChecked = GetImageSource(onIcon, "DefaultChecked.png");

                            binaryIndicator.EnabledUnchecked = GetImageSource(offIcon, "DefaultUnChecked.png");
                        }
                    }
                    controllerObject.SetActionOnDiscreteOrCoil(isCoil,
                        showWhileParameterSet ? ShController.CheckCoilStatus.OnBoth : ShController.CheckCoilStatus.OnTrue,
                        parameter.ModbusIndex,
                        (actionOn, state) =>
                        {
                            ExecDispatched(() =>
                            {

                                if (binaryIndicator != null)
                                {
                                    binaryIndicator.IsChecked = state;
                                }
                                if (visibility.ShowBalloon != null)
                                {
                                    if (state)
                                    {
                                        var balloonSettings = visibility.ShowBalloon;
                                        var balloon = new FancyBalloon(balloonSettings.Text1,
                                            GetBalloonStyle(balloonSettings.Type));
                                        ShowBalloon(balloon);
                                        if (balloonSettings.OnClose != null)
                                        {
                                            balloon.Closing += (sender, args) =>
                                            {
                                                if (balloonSettings.OnClose.ResetParameter != null)
                                                {
                                                    var actionOnCoil = actionOn as ShController.ActionOnCoil;
                                                    actionOnCoil?.Reset();
                                                }
                                            };
                                        }
                                        if (showWhileParameterSet)
                                            dutyBalloon = balloon;
                                    }
                                    else if (showWhileParameterSet && dutyBalloon != null)
                                    {
                                        dutyBalloon.Close();
                                    }
                                }


                            });
                        });
                    break;
                case ModbusTypes.InputRegister:
                case ModbusTypes.HoldingRegister:
                    SimpleIndicator simpleIndicator = null;
                    AnalogIndicator analogControl = null;
                    LastTimeIndicator lastTimeControl = null;
                    Series chartSerie = null;// MainChartLog.Diagram.Series[0];

                    var analogIndicator = visibility.AnalogIndicator;
                    if (analogIndicator != null)
                    {
                        analogControl = new AnalogIndicator();
                        indicatorControl.SpMain.Children.Add(analogControl);
                        var scale = analogControl.MainGauge.Scales[0];
                        scale.StartValue = analogIndicator.Scale.Min;
                        scale.EndValue = analogIndicator.Scale.Max;
                        if (analogIndicator.Scale.MajorCountSpecified)
                            scale.MajorIntervalCount = analogIndicator.Scale.MajorCount;
                        if (analogIndicator.Scale.MinorCountSpecified)
                            scale.MinorIntervalCount = analogIndicator.Scale.MinorCount;

                        if (analogIndicator.Scale.GoodValueSpecified)
                            scale.Markers.Add(new ArcScaleMarker()
                            {
                                Value = analogIndicator.Scale.GoodValue
                            });

                        if (analogIndicator.Scale.Ranges != null)
                        {
                            foreach (var range in analogIndicator.Scale.Ranges)
                            {
                                scale.Ranges.Add(new ArcScaleRange()
                                {
                                    StartValue = new RangeValue(range.StartValue),
                                    EndValue = new RangeValue(range.EndValue)
                                });
                            }
                        }

                        if (!string.IsNullOrEmpty(analogIndicator.Icon))
                        {
                            analogControl.IIcon.Source = GetImageSource(analogIndicator.Icon);
                        }
                    }
                    var digitalIndicatorSettings = visibility.DigitalIndicator;
                    if (digitalIndicatorSettings != null)
                    {
                        simpleIndicator = new SimpleIndicator();
                        indicatorControl.SpMain.Children.Add(simpleIndicator);
                    }
                    var lastTimeIndicator = visibility.LastTimeIndicator;
                    if (lastTimeIndicator != null)
                    {
                        lastTimeControl = new LastTimeIndicator();
                        indicatorControl.SpMain.Children.Add(lastTimeControl);
                        if (!string.IsNullOrEmpty(lastTimeIndicator.Icon))
                        {
                            lastTimeControl.IIcon.Source = GetImageSource(lastTimeIndicator.Icon);
                        }
                    }
                    var chartSettings = visibility.Chart;
                    if (chartSettings != null)
                    {
                        chartSerie = new LineSeries2D();
                        MainChartLog.Diagram.Series.Add(chartSerie);
                        chartSerie.DisplayName = parameter.Name;
                    }

                    //                    else
                    //                    {
                    //                        simpleIndicator = new SimpleIndicator();
                    //                        indicatorControl.SpMain.Children.Add(simpleIndicator);
                    //                        //                                    simpleIndicator.LTitle.Content = parameter.Name;
                    //                    }
                    TimeSpan? interval = null;
                    if (!string.IsNullOrEmpty(parameter.RefreshRate))
                    {
                        TimeSpan tmpVal;
                        if (TimeSpan.TryParseExact(parameter.RefreshRate, "g", null, TimeSpanStyles.None, out tmpVal))
                            interval = tmpVal;
                    }
                    switch (parameter.DataType)
                    {
                        case DataTypes.UInt16:
                            controllerObject.SetActionOnRegister(false, parameter.ModbusIndex, value =>
                            {
                                ExecDispatched(() =>
                                {
                                    if (analogControl != null)
                                    {
                                        analogControl.Value = value;
                                    }
                                    if (lastTimeControl != null)
                                    {
                                        lastTimeControl.Value = DateTime.Now;
                                    }
                                    if (simpleIndicator != null)
                                        simpleIndicator.Value = value;
                                    if (chartSerie != null)
                                    {
                                        var curTime = DateTime.Now;

                                        chartSerie.Points.Add(new SeriesPoint(curTime, value));

                                    }

                                });
                            }, false, interval);
                            break;
                        case DataTypes.Float:
                            break;
                        case DataTypes.RdDateTime:
                            controllerObject.SetActionOnRegisterDateTime(false, parameter.ModbusIndex,
                                value =>
                                {
                                    if (lastTimeControl != null)
                                    {
                                        lastTimeControl.Value = value;
                                    }
                                }, false, interval);
                            break;
                        case DataTypes.RdTime:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessLayoutGroup(DevExpress.Xpf.Docking.LayoutGroup layoutGroupObject, LayoutGroup layoutGroup, List<ShController> allControllers, List<HomeSettingsControllerGroup> controllerGroups)
        {
            var newLayoutGroupObject = new DevExpress.Xpf.Docking.LayoutGroup();
            layoutGroupObject.Items.Add(newLayoutGroupObject);
            newLayoutGroupObject.Orientation = layoutGroup.Orientation == Orientations.Horizontal
                ? Orientation.Horizontal
                : Orientation.Vertical;foreach (var layoutGroup1 in layoutGroup.LayoutGroup1)
            {
                ProcessLayoutGroup(newLayoutGroupObject, layoutGroup1, allControllers, _homeSettings.ControllerGroups);
            }
            foreach (var visibility in layoutGroup.Visibility)
            {
                ProcessVisibility(newLayoutGroupObject, visibility, allControllers, _homeSettings.ControllerGroups);
            }
        }


/*
        private void ProcessSetters(List<HomeSettingsRoomControllersControllerSetter> setters, RoomTab layoutGroupObject,
            ShController controllerObject)
        {
            foreach (var setter in setters)
            {
                var setterControl = new IndicatorControl
                {
                    Caption = setter.Name
                };
                roomTab.MainPanel.Items.Add(setterControl);
                switch (setter.Type)
                {
                    case SetterTypes.RealDateTime:
                        var btn = new Button { Content = "Установить текущее время" };
                        var setterObj = controllerObject.SetSetter(setter.ModbusIndex, SetterTypes.RealDateTime);
                        btn.Click += (sender, args) =>
                        {
                            setterObj.PendingSet();
                        };
                        setterControl.SpMain.Children.Add(btn);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
*/

/*
        private void ProcessPararmeters(List<HomeSettingsControllerGroupControllerParameter> parameters,
    RoomTab roomTab, ShController controllerObject)
        {
            foreach (var parameter in parameters)
            {
                var indicatorControl = new IndicatorControl
                {
                    Caption = parameter.Name
                };
                //                        roomTab.MainPanel.Items = new SerializableItemCollection();
                roomTab.MainPanel.Items.Add(indicatorControl);
                if (parameter.Icon != null)
                {
                    indicatorControl.CaptionImage = GetImageSource(parameter.Icon);
                    indicatorControl.ShowCaptionImage = true;
                }
                switch (parameter.ModbusType)
                {
                    case ModbusTypes.Discrete:
                    case ModbusTypes.Coil:
                        //                                var simpleCheckBox = new CheckBox() { Content = parameter.Name };
                        ObjectButton binaryIndicator = null;
                        //                                indicatorControl.SpMain.Children.Add(simpleCheckBox);
                        var isCoil = parameter.ModbusType == ModbusTypes.Coil;
                        FancyBalloon dutyBalloon = null;
                        var showWhileParameterSet = false;
                        if (parameter.Visibility != null)
                        {
                            if (parameter.Visibility.ShowBalloon != null)
                            {
                                var balloonSettings = parameter.Visibility.ShowBalloon;
                                if (balloonSettings.ShowWhileParameterSetSpecified && balloonSettings.ShowWhileParameterSet)
                                {
                                    //                                            dutyBalloon = new FancyBalloon(balloonSettings.Text1, GetBalloonStyle(balloonSettings.Type));
                                    showWhileParameterSet = true;
                                }
                            }
                            var binaryIndicatorSettings = parameter.Visibility.BinaryIndicator;
                            if (binaryIndicatorSettings != null)
                            {
                                binaryIndicator = new ObjectButton();
                                indicatorControl.SpMain.Children.Add(binaryIndicator);
                                var onIcon = string.IsNullOrEmpty(binaryIndicatorSettings.OnIcon)
                                    ? "DefaultChecked.png"
                                    : binaryIndicatorSettings.OnIcon;
                                var offIcon = string.IsNullOrEmpty(binaryIndicatorSettings.OffIcon)
                                    ? "DefaultUnChecked.png"
                                    : binaryIndicatorSettings.OffIcon;

                                binaryIndicator.EnabledChecked = GetImageSource(onIcon, "DefaultChecked.png");

                                binaryIndicator.EnabledUnchecked = GetImageSource(offIcon, "DefaultUnChecked.png");
                            }
                        }
                        controllerObject.SetActionOnDiscreteOrCoil(isCoil,
                            showWhileParameterSet ? ShController.CheckCoilStatus.OnBoth : ShController.CheckCoilStatus.OnTrue,
                            parameter.ModbusIndex,
                            (actionOn, state) =>
                            {
                                ExecDispatched(() =>
                                {
                                    if (parameter.Visibility != null)
                                    {
                                        if (binaryIndicator != null)
                                        {
                                            binaryIndicator.IsChecked = state;
                                        }
                                        if (parameter.Visibility.ShowBalloon != null)
                                        {
                                            if (state)
                                            {
                                                var balloonSettings = parameter.Visibility.ShowBalloon;
                                                var balloon = new FancyBalloon(balloonSettings.Text1,
                                                    GetBalloonStyle(balloonSettings.Type));
                                                ShowBalloon(balloon);
                                                if (balloonSettings.OnClose != null)
                                                {
                                                    balloon.Closing += (sender, args) =>
                                                    {
                                                        if (balloonSettings.OnClose.ResetParameter != null)
                                                        {
                                                            var actionOnCoil = actionOn as ShController.ActionOnCoil;
                                                            actionOnCoil?.Reset();
                                                        }
                                                    };
                                                }
                                                if (showWhileParameterSet)
                                                    dutyBalloon = balloon;
                                            }
                                            else if (showWhileParameterSet && dutyBalloon != null)
                                            {
                                                dutyBalloon.Close();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //                                                simpleCheckBox.IsChecked = state;
                                    }
                                });
                            });
                        break;
                    case ModbusTypes.InputRegister:
                    case ModbusTypes.HoldingRegister:
                        SimpleIndicator simpleIndicator = null;
                        AnalogIndicator analogControl = null;
                        LastTimeIndicator lastTimeControl = null;
                        Series chartSerie = null;// MainChartLog.Diagram.Series[0];
                        if (parameter.Visibility != null)
                        {
                            var analogIndicator = parameter.Visibility.AnalogIndicator;
                            if (analogIndicator != null)
                            {
                                analogControl = new AnalogIndicator();
                                indicatorControl.SpMain.Children.Add(analogControl);
                                var scale = analogControl.MainGauge.Scales[0];
                                scale.StartValue = analogIndicator.Scale.Min;
                                scale.EndValue = analogIndicator.Scale.Max;
                                if (analogIndicator.Scale.MajorCountSpecified)
                                    scale.MajorIntervalCount = analogIndicator.Scale.MajorCount;
                                if (analogIndicator.Scale.MinorCountSpecified)
                                    scale.MinorIntervalCount = analogIndicator.Scale.MinorCount;

                                if (analogIndicator.Scale.GoodValueSpecified)
                                    scale.Markers.Add(new ArcScaleMarker()
                                    {
                                        Value = analogIndicator.Scale.GoodValue
                                    });

                                if (analogIndicator.Scale.Ranges != null)
                                {
                                    foreach (var range in analogIndicator.Scale.Ranges)
                                    {
                                        scale.Ranges.Add(new ArcScaleRange()
                                        {
                                            StartValue = new RangeValue(range.StartValue),
                                            EndValue = new RangeValue(range.EndValue)
                                        });
                                    }
                                }

                                if (!string.IsNullOrEmpty(analogIndicator.Icon))
                                {
                                    analogControl.IIcon.Source = GetImageSource(analogIndicator.Icon);
                                }
                            }
                            var digitalIndicatorSettings = parameter.Visibility.DigitalIndicator;
                            if (digitalIndicatorSettings != null)
                            {
                                simpleIndicator = new SimpleIndicator();
                                indicatorControl.SpMain.Children.Add(simpleIndicator);
                            }
                            var lastTimeIndicator = parameter.Visibility.LastTimeIndicator;
                            if (lastTimeIndicator != null)
                            {
                                lastTimeControl = new LastTimeIndicator();
                                indicatorControl.SpMain.Children.Add(lastTimeControl);
                                if (!string.IsNullOrEmpty(lastTimeIndicator.Icon))
                                {
                                    lastTimeControl.IIcon.Source = GetImageSource(lastTimeIndicator.Icon);
                                }
                            }
                            var chartSettings = parameter.Visibility.Chart;
                            if (chartSettings != null)
                            {
                                chartSerie = new LineSeries2D();
                                MainChartLog.Diagram.Series.Add(chartSerie);
                                chartSerie.DisplayName = parameter.Name;
                            }
                        }
                        else
                        {
                            simpleIndicator = new SimpleIndicator();
                            indicatorControl.SpMain.Children.Add(simpleIndicator);
                            //                                    simpleIndicator.LTitle.Content = parameter.Name;
                        }
                        TimeSpan? interval = null;
                        if (!string.IsNullOrEmpty(parameter.RefreshRate))
                        {
                            TimeSpan tmpVal;
                            if (TimeSpan.TryParseExact(parameter.RefreshRate, "g", null, TimeSpanStyles.None, out tmpVal))
                                interval = tmpVal;
                        }
                        switch (parameter.DataType)
                        {
                            case DataTypes.UInt16:
                                controllerObject.SetActionOnRegister(false, parameter.ModbusIndex, value =>
                                {
                                    ExecDispatched(() =>
                                    {
                                        if (analogControl != null)
                                        {
                                            analogControl.Value = value;
                                        }
                                        if (lastTimeControl != null)
                                        {
                                            lastTimeControl.Value = DateTime.Now;
                                        }
                                        if (simpleIndicator != null)
                                            simpleIndicator.Value = value;
                                        if (chartSerie != null)
                                        {
                                            var curTime = DateTime.Now;

                                            chartSerie.Points.Add(new SeriesPoint(curTime, value));

                                        }

                                    });
                                }, false, interval);
                                break;
                            case DataTypes.Float:
                                break;
                            case DataTypes.RdDateTime:
                                controllerObject.SetActionOnRegisterDateTime(false, parameter.ModbusIndex,
                                    value =>
                                    {
                                        if (lastTimeControl != null)
                                        {
                                            lastTimeControl.Value = value;
                                        }
                                    }, false, interval);
                                break;
                            case DataTypes.RdTime:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
*/


        /*
                private void ProcessPararmeters(List<HomeSettingsRoomControllersControllerParameter> parameters,
                    RoomTab roomTab, ShController controllerObject)
                {
                    foreach (var parameter in parameters)
                    {
                        var indicatorControl = new IndicatorControl
                        {
                            Caption = parameter.Name
                        };
                        //                        roomTab.MainPanel.Items = new SerializableItemCollection();
                        roomTab.MainPanel.Items.Add(indicatorControl);
                        if (parameter.Icon != null)
                        {
                            indicatorControl.CaptionImage = GetImageSource(parameter.Icon);
                            indicatorControl.ShowCaptionImage = true;
                        }
                        switch (parameter.ModbusType)
                        {
                            case ModbusTypes.Discrete:
                            case ModbusTypes.Coil:
                                //                                var simpleCheckBox = new CheckBox() { Content = parameter.Name };
                                ObjectButton binaryIndicator = null;
                                //                                indicatorControl.SpMain.Children.Add(simpleCheckBox);
                                var isCoil = parameter.ModbusType == ModbusTypes.Coil;
                                FancyBalloon dutyBalloon = null;
                                var showWhileParameterSet = false;
                                if (parameter.Visibility != null)
                                {
                                    if (parameter.Visibility.ShowBalloon != null)
                                    {
                                        var balloonSettings = parameter.Visibility.ShowBalloon;
                                        if (balloonSettings.ShowWhileParameterSetSpecified && balloonSettings.ShowWhileParameterSet)
                                        {
                                            //                                            dutyBalloon = new FancyBalloon(balloonSettings.Text1, GetBalloonStyle(balloonSettings.Type));
                                            showWhileParameterSet = true;
                                        }
                                    }
                                    var binaryIndicatorSettings = parameter.Visibility.BinaryIndicator;
                                    if (binaryIndicatorSettings != null)
                                    {
                                        binaryIndicator = new ObjectButton();
                                        indicatorControl.SpMain.Children.Add(binaryIndicator);
                                        var onIcon = string.IsNullOrEmpty(binaryIndicatorSettings.OnIcon)
                                            ? "DefaultChecked.png"
                                            : binaryIndicatorSettings.OnIcon;
                                        var offIcon = string.IsNullOrEmpty(binaryIndicatorSettings.OffIcon)
                                            ? "DefaultUnChecked.png"
                                            : binaryIndicatorSettings.OffIcon;

                                        binaryIndicator.EnabledChecked = GetImageSource(onIcon, "DefaultChecked.png");

                                        binaryIndicator.EnabledUnchecked = GetImageSource(offIcon, "DefaultUnChecked.png");
                                    }
                                }
                                controllerObject.SetActionOnDiscreteOrCoil(isCoil,
                                    showWhileParameterSet ? ShController.CheckCoilStatus.OnBoth : ShController.CheckCoilStatus.OnTrue,
                                    parameter.ModbusIndex,
                                    (actionOn, state) =>
                                    {
                                        ExecDispatched(() =>
                                        {
                                            if (parameter.Visibility != null)
                                            {
                                                if (binaryIndicator != null)
                                                {
                                                    binaryIndicator.IsChecked = state;
                                                }
                                                if (parameter.Visibility.ShowBalloon != null)
                                                {
                                                    if (state)
                                                    {
                                                        var balloonSettings = parameter.Visibility.ShowBalloon;
                                                        var balloon = new FancyBalloon(balloonSettings.Text1,
                                                            GetBalloonStyle(balloonSettings.Type));
                                                        ShowBalloon(balloon);
                                                        if (balloonSettings.OnClose != null)
                                                        {
                                                            balloon.Closing += (sender, args) =>
                                                            {
                                                                if (balloonSettings.OnClose.ResetParameter != null)
                                                                {
                                                                    var actionOnCoil = actionOn as ShController.ActionOnCoil;
                                                                    actionOnCoil?.Reset();
                                                                }
                                                            };
                                                        }
                                                        if (showWhileParameterSet)
                                                            dutyBalloon = balloon;
                                                    }
                                                    else if (showWhileParameterSet && dutyBalloon != null)
                                                    {
                                                        dutyBalloon.Close();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //                                                simpleCheckBox.IsChecked = state;
                                            }
                                        });
                                    });
                                break;
                            case ModbusTypes.InputRegister:
                            case ModbusTypes.HoldingRegister:
                                SimpleIndicator simpleIndicator = null;
                                AnalogIndicator analogControl = null;
                                LastTimeIndicator lastTimeControl = null;
                                Series chartSerie = null;// MainChartLog.Diagram.Series[0];
                                if (parameter.Visibility != null)
                                {
                                    var analogIndicator = parameter.Visibility.AnalogIndicator;
                                    if (analogIndicator != null)
                                    {
                                        analogControl = new AnalogIndicator();
                                        indicatorControl.SpMain.Children.Add(analogControl);
                                        var scale = analogControl.MainGauge.Scales[0];
                                        scale.StartValue = analogIndicator.Scale.Min;
                                        scale.EndValue = analogIndicator.Scale.Max;
                                        if (analogIndicator.Scale.MajorCountSpecified)
                                            scale.MajorIntervalCount = analogIndicator.Scale.MajorCount;
                                        if (analogIndicator.Scale.MinorCountSpecified)
                                            scale.MinorIntervalCount = analogIndicator.Scale.MinorCount;

                                        if (analogIndicator.Scale.GoodValueSpecified)
                                            scale.Markers.Add(new ArcScaleMarker()
                                            {
                                                Value = analogIndicator.Scale.GoodValue
                                            });

                                        if (analogIndicator.Scale.Ranges != null)
                                        {
                                            foreach (var range in analogIndicator.Scale.Ranges)
                                            {
                                                scale.Ranges.Add(new ArcScaleRange()
                                                {
                                                    StartValue = new RangeValue(range.StartValue),
                                                    EndValue = new RangeValue(range.EndValue)
                                                });
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(analogIndicator.Icon))
                                        {
                                            analogControl.IIcon.Source = GetImageSource(analogIndicator.Icon);
                                        }
                                    }
                                    var digitalIndicatorSettings = parameter.Visibility.DigitalIndicator;
                                    if (digitalIndicatorSettings != null)
                                    {
                                        simpleIndicator = new SimpleIndicator();
                                        indicatorControl.SpMain.Children.Add(simpleIndicator);
                                    }
                                    var lastTimeIndicator = parameter.Visibility.LastTimeIndicator;
                                    if (lastTimeIndicator != null)
                                    {
                                        lastTimeControl = new LastTimeIndicator();
                                        indicatorControl.SpMain.Children.Add(lastTimeControl);
                                        if (!string.IsNullOrEmpty(lastTimeIndicator.Icon))
                                        {
                                            lastTimeControl.IIcon.Source = GetImageSource(lastTimeIndicator.Icon);
                                        }
                                    }
                                    var chartSettings = parameter.Visibility.Chart;
                                    if (chartSettings != null)
                                    {
                                        chartSerie = new LineSeries2D();
                                        MainChartLog.Diagram.Series.Add(chartSerie);
                                        chartSerie.DisplayName = parameter.Name;
                                    }
                                }
                                else
                                {
                                    simpleIndicator = new SimpleIndicator();
                                    indicatorControl.SpMain.Children.Add(simpleIndicator);
                                    //                                    simpleIndicator.LTitle.Content = parameter.Name;
                                }
                                TimeSpan? interval = null;
                                if (!string.IsNullOrEmpty(parameter.RefreshRate))
                                {
                                    TimeSpan tmpVal;
                                    if (TimeSpan.TryParseExact(parameter.RefreshRate, "g", null, TimeSpanStyles.None, out tmpVal))
                                        interval = tmpVal;
                                }
                                switch (parameter.DataType)
                                {
                                    case DataTypes.UInt16:
                                        controllerObject.SetActionOnRegister(false, parameter.ModbusIndex, value =>
                                        {
                                            ExecDispatched(() =>
                                            {
                                                if (analogControl != null)
                                                {
                                                    analogControl.Value = value;
                                                }
                                                if (lastTimeControl != null)
                                                {
                                                    lastTimeControl.Value = DateTime.Now;
                                                }
                                                if (simpleIndicator != null)
                                                    simpleIndicator.Value = value;
                                                if (chartSerie != null)
                                                {
                                                    var curTime = DateTime.Now;

                                                    chartSerie.Points.Add(new SeriesPoint(curTime, value));

                                                }

                                            });
                                        }, false, interval);
                                        break;
                                    case DataTypes.Float:
                                        break;
                                    case DataTypes.RdDateTime:
                                        controllerObject.SetActionOnRegisterDateTime(false, parameter.ModbusIndex,
                                            value =>
                                            {
                                                if (lastTimeControl != null)
                                                {
                                                    lastTimeControl.Value = value;
                                                }
                                            }, false, interval);
                                        break;
                                    case DataTypes.RdTime:
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
        */

        private FancyBalloon.BaloonStyles GetBalloonStyle(BalloonTypes balloonType)
        {
            switch (balloonType)
            {
                case BalloonTypes.Normal:
                    return FancyBalloon.BaloonStyles.Normal;
                case BalloonTypes.Info:
                    return FancyBalloon.BaloonStyles.Info;
                case BalloonTypes.Warning:
                    return FancyBalloon.BaloonStyles.Warning;
                case BalloonTypes.Exclamation:
                    return FancyBalloon.BaloonStyles.Exclamation;
                case BalloonTypes.Alarm:
                    return FancyBalloon.BaloonStyles.Alarm;
                case BalloonTypes.Error:
                    return FancyBalloon.BaloonStyles.Error;
                default:
                    throw new ArgumentOutOfRangeException(nameof(balloonType), balloonType, null);
            }
        }

        private bool LoadSettings()
        {
            if (!File.Exists(SettingsFileName))
            {
                MessageBox.Show("Файл с настройками не найден!");
                return false;
            }
            try
            {
                _homeSettings = HomeSettings.LoadFromFile(SettingsFileName);
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return false;
            }
        }


        private void FillPorts()
        {
            var ports = SerialPort.GetPortNames();
            LbPorts.ItemsSource = ports;
            if (!string.IsNullOrEmpty(Settings.Default.SelectedCom))
                LbPorts.SelectedItem = Settings.Default.SelectedCom;
        }


        private void StartWatchingClick(object sender, RoutedEventArgs e)
        {
            _modbusMasterThread.StartListening();
        }


        public bool IsPortOpen => _modbusMasterThread != null && _modbusMasterThread.IsPortOpen;

        private void Window_Closed(object sender, EventArgs e)
        {
            _modbusMasterThread.StopListening();
            _modbusMasterThread.Close();
        }

        private void SelectPort(object sender, SelectionChangedEventArgs e)
        {
            var lb = (ListBox)sender;

            var selectedPort = lb.SelectedItem as string;
            if (selectedPort == null)
                return;

            Settings.Default.SelectedCom = selectedPort;
            Settings.Default.Save();
        }

        private void SelectBaudrate(object sender, SelectionChangedEventArgs e)
        {
            var lb = (ListBox)sender;

            if (lb.SelectedItem == null)
                return;

            Settings.Default.SelectedBaudrate = (int)lb.SelectedItem;
            Settings.Default.Save();
        }
        /*
                private void TogglePortClick(object sender, RoutedEventArgs e)
                {
                    if (IsPortOpen)
                    {
                        ClosePort();
                    }
                    else
                    {
                        CreatePort();
                    }
                }
        */


        /// <summary>
        /// Включён ли режим опроса
        /// </summary>
        public bool IsListening
        {
            get { return _isListening; }
            set
            {
                _isListening = value;
                OnPropertyChanged("IsListening");

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (IudDelayAfter.Value == null)
            {
                MessageBox.Show("Выберите задержку");
                return;
            }

            if (CalcDemoSpace() > 254)
            {
                MessageBox.Show("Ещё одна команда не влезет");
                return;
            }

            var demoCommand = new DemoCommand
            {
                Led1 = CbLed1.IsChecked == true,
                Led2 = CbLed2.IsChecked == true,
                Led3 = CbLed3.IsChecked == true,
                Led4 = CbLed4.IsChecked == true,
                Brightnes = (byte)(int)CbBrightnes.SelectedValue
            };

            if (RbDelayMs.IsChecked == true)
            {
                var delayMs = (int)Math.Round((double)(IudDelayAfter.Value.Value / 10d));
                IudDelayAfter.Value = delayMs * 10;
                demoCommand.Delay = (byte)delayMs;
            }
            else
            {
                demoCommand.DelayInSec = true;
                demoCommand.Delay = (byte)IudDelayAfter.Value.Value;
            }
            _demoCommands.Add(demoCommand);
            _demoView.View.Refresh();
            LDemoBytesLeft.Content = 256 - CalcDemoSpace();
            //TbDemoCommands.Text += 
        }

        private int CalcDemoSpace()
        {
            var bytesCount = 0;
            foreach (var demoCommand in _demoCommands)
            {
                bytesCount += demoCommand.BytesCount;
            }
            return bytesCount;
        }

        private void RbDelayMs_Checked(object sender, RoutedEventArgs e)
        {
            IudDelayAfter.Maximum = 2540;
        }

        private void RbDelayS_Checked(object sender, RoutedEventArgs e)
        {
            IudDelayAfter.Maximum = 255;
        }

        private void DeleteLastDemoCommand(object sender, RoutedEventArgs e)
        {
            if (_demoCommands.Count > 0)
            {
                _demoCommands.RemoveAt(_demoCommands.Count - 1);

                LDemoBytesLeft.Content = 256 - CalcDemoSpace();
                _demoView.View.Refresh();
            }
        }

        private void SaveDemo(object sender, RoutedEventArgs e)
        {
            if (_demoCommands.Count == 0)
            {
                MessageBox.Show("Сохранять нечего");
                return;
            }
            var sfd = new SaveFileDialog
            {
                Filter = "EEPROM|*.eep",
                DefaultExt = ".eep"
            };
            if (sfd.ShowDialog() != true)
                return;

            var bytes = new List<byte>();
            foreach (var demoCommand in _demoCommands)
            {
                bytes.AddRange(demoCommand.ToBytes());
            }

            // Размер
            bytes.Insert(0, (byte)(bytes.Count - 1));
            // Сигнатура
            bytes.Insert(0, 0x22);
            var hexFile = new HexFile((s => { }), false);
            hexFile.AddRange(bytes);
            File.WriteAllLines(sfd.FileName, hexFile.GetHexFile());
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            /*
                        var eepromFileName = FpDemoEeprom.FileName.Trim();
                        if (string.IsNullOrEmpty(eepromFileName) || !File.Exists(eepromFileName))
                        {
                            MessageBox.Show("Выберите файл с демо");
                            return;
                        }
                        CreatePort();

                        _modbus = ModbusSerialMaster.CreateRtu(_port);

                        var demoHex = new HexFile((s => { }), false);
                        demoHex.Load(eepromFileName);
                        var hexBytes = demoHex.ToArray();

                        var sent = 0;
                        ushort wordsSent = 0;
                        do
                        {
                            // Для ардуино максимальный размер буфера 64
                            var wordsToSend = (int)ModbusWriteFileRecord.MaxDataCountByBufferSize(60);
                            var bytesToSend = wordsToSend * 2;
                            if (sent + bytesToSend > hexBytes.Length)
                                bytesToSend = hexBytes.Length - sent;
                            var msg = new ModbusWriteFileRecordRequest(1, 1, wordsSent, hexBytes, sent, bytesToSend);
                            var resp = _modbus.ExecuteCustomMessage<ModbusWriteFileRecord>(msg);
                            sent += bytesToSend;
                            wordsSent += (ushort)wordsToSend;
                        } while (sent < hexBytes.Length);

                        //(0x15) — Запись в файл(Write File Record)

            */
        }

        private void SetCurrentTime(object sender, RoutedEventArgs e)
        {
            //            ShowBaloon("Установка времени", FancyBalloon.BaloonStyles.Exclamation);
            //            return;

            _modbusMasterThread.SetCurrentTime();

            /*
                        CreatePort();

                        if (_modbus == null)
                            _modbus = ModbusSerialMaster.CreateRtu(_port);

                        var curTime = DateTime.Now;
                        var timeData = new ushort[3];
                        timeData[0] = (ushort)((curTime.Hour << 8) | curTime.Minute);
                        timeData[1] = (ushort)((curTime.Day << 8) | curTime.Second);
                        timeData[2] = (ushort)((curTime.Month << 8) | (curTime.Year % 100));

                        //            _modbus.WriteMultipleRegisters(2, 8, timeData);
                        _modbus.WriteMultipleRegisters(2, 8, timeData);
                        Thread.Sleep(500);

                        var setTimeRes = _modbus.ReadInputRegisters(2, 4, 1);
                        if (setTimeRes.Length > 0 && setTimeRes[0] == 0xffff)
                            MessageBox.Show("Время установлено успешно!");
                        else
                        {
                            MessageBox.Show($"Ошибка установки времени! ({setTimeRes[0]:X4})");
                        }

            */

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Tag == null)
                e.Cancel = true;
            Hide();
        }

        private void ShowBalloon(FancyBalloon balloon)
        {
            if (MyNotifyIcon.CustomBalloon != null && MyNotifyIcon.CustomBalloon.IsOpen)
            {
                _balloonStack.AddBaloon(balloon);
                return;
            }
            _balloonStack = new BalloonStack();
            _balloonStack.AddBaloon(balloon);
            MyNotifyIcon.ShowCustomBalloon(_balloonStack, PopupAnimation.Slide, null);

        }

        private void BStop_Click(object sender, RoutedEventArgs e)
        {
            IsListening = false;
            _modbusMasterThread.StopListening();
            //StopListening();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _modbusMasterThread.GetTempAndHym();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
