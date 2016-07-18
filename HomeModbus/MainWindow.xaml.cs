using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Chip45ProgrammerLib;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Gauges;
using HomeBasePlugin;
using HomeModbus.Controls;
using HomeModbus.Models;
using HomeModbus.Models.Base;
using HomeModbus.Tooltip;
using HomeServer;
using log4net;
using log4net.Config;
using Microsoft.Win32;
using Newtonsoft.Json;
using SKYPE4COMLib;
using Application = System.Windows.Application;

// Import log4net classes.

namespace HomeModbus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));

        private const string ServerName = "tor";

        private const string SettingsFileName = "HomeClientSettings.json";
        private HomeClientSettings _homeSettings;

        private const string ValuesLogLogFileName = "ValuesLog.json";
        //        private readonly ModbusMasterThread _modbusMasterThread;

        const double BrightStep = 100 / 15d;

        private readonly CollectionViewSource _demoView = new CollectionViewSource();

        private readonly HsClient _client = null;
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
                LbLog.Items.Add($"{DateTime.Now.ToString("F")} {msg}");
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


            //            LoadValuesLog();


            _balloonStack = new BalloonStack();


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

            _client = new HsClient(ServerName, WriteToLog);

            if (LoadSettings())
                ApplySettings();

            ConnectToServer();

            // Starting plugins
            if(_plugins != null)
                foreach (var homePlugin in _plugins)
                {
                    homePlugin.Start();
                }

            SystemEvents.PowerModeChanged += OnPowerChanged;

            LoadSoundJson();
            LoadDiaryJson();
        }

        private const string SoundJsonFile = "sounds.json";
        private const string DiaryJsonFile = "diary.json";
        private void LoadSoundJson()
        {
            if (!File.Exists(SoundJsonFile))
                return;
            TbSoundJson.Text = File.ReadAllText(SoundJsonFile);
        }

        private void LoadDiaryJson()
        {
            if (!File.Exists(DiaryJsonFile))
                return;
            TbDiaryJson.Text = File.ReadAllText(DiaryJsonFile);
        }

        private void OnPowerChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    ConnectToServer();
                    break;
                case PowerModes.Suspend:
                    _client?.Disconnect();
                    break;
            }
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


        public static ImageSource GetImageSource(string imageName, string defaultImage = null, double height = -1)
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
            ProcessPlugins();
            ProcessRooms();

        }

        private Skype _skype;


        private List<IHomePlugin> _plugins;


        void SendEcho(HomeClientSettings.Plugin.PluginEvent.EchoValue echo, string value)
        {
            var arguments = string.Empty;
            if (echo.Arguments != null &&
                echo.Arguments.Length > 0)
            {
                var tmpArg = new List<string>();
                foreach (var argument in echo.Arguments)
                {
                    switch (argument.Type)
                    {
                        case HomeClientSettings.Plugin.PluginEvent.EchoValue.Argument.ArgumentTypes.Literal:
                            tmpArg.Add(argument.Value.ToString());
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                arguments = "," + string.Join(",", tmpArg);
            }
            switch (echo.Type)
            {

                case HomeClientSettings.Plugin.PluginEvent.EchoValue.EchoTypes.Setter:
                    _client.SendMessage($"{HsEnvelope.ControllersSetValue}/{echo.Id}", $"{value}{arguments}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private void ProcessPlugins()
        {
            foreach (var plugin in _homeSettings.Plugins)
            {
                if (plugin.Name == "Skype")
                {
                    _skype = new Skype();
                    try
                    {
                        foreach (var pluginEvent in plugin.Events)
                        {
                            if (pluginEvent.Name == "OnUnreadMessages")
                            {
                                //подписываемся на новые сообщения
                                _skype.MessageStatus += (message, status) =>
                                {
                                    SendEcho(pluginEvent.Echo, (_skype.MissedMessages.Count > 0 || _skype.MissedChats.Count > 0 ? 1 : 0).ToString());
                                };
                            }
                        }
                        //пытаемся присоединиться к скайпу. В данный момент вылезет окошко, где он у вас спросит разрешения на открытие доступа программе.
                        //5 это версия протокола (идёт по-умолчанию), true - отключить ли отваливание по таймауту для запроса к скайпу.
                        _skype.Attach();
                        Console.WriteLine("skype attached");
                    }
                    catch (Exception ex)
                    {
                        //выводим в консольку, если что-то не так
                        Console.WriteLine("top lvl exception : " + ex);
                    }

                }
                else if (plugin.Name == "GmailNotify")
                {
                    if (_plugins == null)
                        _plugins = new List<IHomePlugin>();
                    var gmailNotify = new GmailNotify.GmailNotify();
                    foreach (var pluginEvent in plugin.Events)
                    {
                        gmailNotify.SetEventHendler(plugin.Events[0].Name, objects =>
                        {
                            SendEcho(pluginEvent.Echo, objects != null && objects.Length > 0 ? "1" : "0");
                        });
                    }
                    _plugins.Add(gmailNotify);                    
                }
            }

        }

        private void ProcessRooms()
        {
            foreach (var room in _homeSettings.Rooms)
            {
                var roomTab = new RoomTab { Header = room.Name };
                MainTabs.Items.Add(roomTab);

                if (room.LayoutGroups != null)
                    foreach (var layoutGroup in room.LayoutGroups)
                    {
                        ProcessLayoutGroup(roomTab.MainPanel, layoutGroup);
                    }
                if (room.Visibilities != null)
                    foreach (var visibility in room.Visibilities)
                    {
                        ProcessVisibility(roomTab.MainPanel, visibility);
                    }
            }
        }

        private void ProcessVisibility(LayoutGroup layoutGroupObject, HomeClientSettings.Room.Visibility visibility)
        {
            // Ищем соответствующий параметр
            if (!string.IsNullOrEmpty(visibility.ParameterId))
            {
                CreateVisibilityParameter(layoutGroupObject, visibility);
            }


            // Ищем соответствующий setter
            else if (!string.IsNullOrEmpty(visibility.SetterId))
            {
                CreateVisibilitySetter(layoutGroupObject, visibility);
            }
        }


        private void CreateVisibilitySetter(LayoutGroup layoutGroupObject, HomeClientSettings.Room.Visibility visibility)
        {
            var setterControl = new IndicatorControl
            {
                Caption = visibility.Name
            };
            layoutGroupObject.Add(setterControl);
            if (visibility.CurrentTime != null)
            {
                var simpleSetter = new SimpleSetter { TbName = { Content = "Установить текущее время" } };
                simpleSetter.BMain.Click += (sender, args) =>
                {
                    _client.SendMessage($"{HsEnvelope.ControllersSetValue}/{visibility.SetterId}", DateTime.Now.ToString(HsEnvelope.DateTimeFormat));
                };
                _client.SetResultAction(visibility.SetterId, status =>
                {
                    simpleSetter.Result(status);
                });
                setterControl.SpMain.Children.Add(simpleSetter);
            }
            var sendCommandSetting = visibility.SendCommand;
            if (sendCommandSetting != null)
            {
                var sendCommandSetter = new CommandSetter();
                setterControl.SpMain.Children.Add(sendCommandSetter);
                sendCommandSetter.BMain.Click += (sender, args) =>
                {
                    _client.SendMessage($"{HsEnvelope.ControllersSetValue}/{visibility.SetterId}",
                        $"{sendCommandSetter.IudCommand.Value},{sendCommandSetter.IudCommandData.Value}," +
                        $"{sendCommandSetter.IudAdditionalData1.Value >> 8},{sendCommandSetter.IudAdditionalData1.Value & 0xff}," +
                        $"{sendCommandSetter.IudAdditionalData2.Value >> 8},{sendCommandSetter.IudAdditionalData2.Value & 0xff}," +
                        $"{sendCommandSetter.IudAdditionalData3.Value >> 8},{sendCommandSetter.IudAdditionalData3.Value & 0xff}");
                };
                _client.SetResultAction(visibility.SetterId, status =>
                {
                    sendCommandSetter.Result(status);
                });
            }
        }


        private void CreateVisibilityParameter(LayoutGroup layoutGroupObject,
            HomeClientSettings.Room.Visibility visibility)
        {
            var indicatorControl = new IndicatorControl
            {
                Caption = visibility.Name
            };
            //                        roomTab.MainPanel.Items = new SerializableItemCollection();
            layoutGroupObject.Items.Add(indicatorControl);
            if (visibility.Icon != null)
            {
                indicatorControl.CaptionImage = GetImageSource(visibility.Icon);
                indicatorControl.ShowCaptionImage = true;
            }

            ObjectButton binaryIndicator = null;
            FancyBalloon dutyBalloon = null;
            var showWhileParameterSet = false;

            SimpleIndicator simpleIndicator = null;
            StringIndicator stringIndicator = null;
            AnalogIndicator analogControl = null;
            BarometerIndicator barometerControl = null;
            LastTimeIndicator lastTimeControl = null;
            DoubleIndicator doubleControl = null;

            Series chartSerie = null;// MainChartLog.Diagram.Series[0];


            if (visibility.ShowBalloon != null)
            {
                var balloonSettings = visibility.ShowBalloon;
                if (balloonSettings.ShowWhileParameterSet)
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

            var analogIndicator = visibility.AnalogIndicator;
            if (analogIndicator != null)
            {
                analogControl = new AnalogIndicator();
                indicatorControl.SpMain.Children.Add(analogControl);
                var scale = analogControl.MainGauge.Scales[0];
                scale.StartValue = analogIndicator.Scale.Min;
                scale.EndValue = analogIndicator.Scale.Max;
                if (analogIndicator.Scale.MajorCount != null)
                    scale.MajorIntervalCount = analogIndicator.Scale.MajorCount.Value;
                if (analogIndicator.Scale.MinorCount != null)
                    scale.MinorIntervalCount = analogIndicator.Scale.MinorCount.Value;

                if (analogIndicator.Scale.GoodValue != null)
                    scale.Markers.Add(new ArcScaleMarker()
                    {
                        Value = analogIndicator.Scale.GoodValue.Value
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
            var barometerIndicator = visibility.BarometerIndicator;
            if (barometerIndicator != null)
            {
                barometerControl = new BarometerIndicator();
                indicatorControl.SpMain.Children.Add(barometerControl);

                _client.SetAction(barometerIndicator.TemperatureParameterId, (resetAction, value) =>
                {
                    ExecDispatched(() =>
                    {
                        barometerControl.Temperature = Convert.ToDouble(value);
                    });
                });
                _client.SetAction(barometerIndicator.HymidityParameterId, (resetAction, value) =>
                {
                    ExecDispatched(() =>
                    {
                        barometerControl.Hymidity = Convert.ToDouble(value);
                    });
                });
            }

            var digitalIndicatorSettings = visibility.DigitalIndicator;
            if (digitalIndicatorSettings != null)
            {
                simpleIndicator = new SimpleIndicator();
                indicatorControl.SpMain.Children.Add(simpleIndicator);
            }
            var stringIndicatorSettings = visibility.StringIndicator;
            if (stringIndicatorSettings != null)
            {
                stringIndicator = new StringIndicator();
                indicatorControl.SpMain.Children.Add(stringIndicator);
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
            var doubleIndicator = visibility.DoubleIndicator;
            if (doubleIndicator != null)
            {
                doubleControl = new DoubleIndicator();
                indicatorControl.SpMain.Children.Add(doubleControl);
            }
            var chartSettings = visibility.Chart;
            if (chartSettings != null)
            {
                chartSerie = new LineSeries2D();
                MainChartLog.Diagram.Series.Add(chartSerie);
                chartSerie.DisplayName = visibility.Name;
            }

            _client.SetAction(visibility.ParameterId, (resetAction, value) =>
            {
                ExecDispatched(() =>
                {
                    if (binaryIndicator != null)
                    {
                        if (value.GetType().IsPrimitive)
                            binaryIndicator.IsChecked = (bool)value;
                    }
                    if (stringIndicator != null)
                    {
                        var s = value as string;
                        if (s != null)
                            stringIndicator.Value = s;
                    }
                    if (visibility.ShowBalloon != null)
                    {
                        if (value.GetType().IsPrimitive)
                            if ((bool)value)
                            {
                                var balloonSettings = visibility.ShowBalloon;
                                var balloon = new FancyBalloon(balloonSettings.Text,
                                    GetBalloonStyle(balloonSettings.Type));
                                ShowBalloon(balloon);
                                if (balloonSettings.OnClose != null)
                                {
                                    balloon.Closing += (sender, args) =>
                                    {
                                        if (balloonSettings.OnClose.ResetParameter != null)
                                        {
                                            resetAction?.Invoke(false);
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


                    if (analogControl != null)
                    {
                        if (value.GetType().IsPrimitive)
                            analogControl.Value = Convert.ToDouble(value);
                    }
                    if (barometerControl != null)
                    {
                        if (value.GetType().IsPrimitive)
                            barometerControl.Value = Convert.ToDouble(value);
                    }
                    if (lastTimeControl != null)
                    {
                        if (value is DateTime)
                            lastTimeControl.Value = (DateTime)value;
                    }
                    if (doubleControl != null)
                    {
                        if (value.GetType().IsPrimitive)
                        {
                            doubleControl.HiValue = (((ushort)value & 0xFF00) >> 8).ToString(doubleIndicator.IsHex ? "X2" : "D");
                            doubleControl.LoValue = ((ushort)value & 0xFF).ToString(doubleIndicator.IsHex ? "X2" : "D");
                        }
                    }

                    if (simpleIndicator != null)
                    {
                        if (value.GetType().IsPrimitive)
                            simpleIndicator.Value = Convert.ToDouble(value);
                    }
                    if (chartSerie != null)
                    {
                        var curTime = DateTime.Now;
                        if (value.GetType().IsPrimitive)
                            chartSerie.Points.Add(new SeriesPoint(curTime, Convert.ToDouble(value)));

                    }

                });
            });
        }

        private void ProcessLayoutGroup(LayoutGroup layoutGroupObject, HomeClientSettings.Room.LayoutGroup layoutGroup)
        {
            var newLayoutGroupObject = new LayoutGroup();
            layoutGroupObject.Items.Add(newLayoutGroupObject);
            newLayoutGroupObject.Orientation = layoutGroup.Orientation == HomeClientSettings.Room.LayoutGroup.Orientations.Horizontal
                ? Orientation.Horizontal
                : Orientation.Vertical;
            if (layoutGroup.LayoutGroups != null)
                foreach (var layoutGroup1 in layoutGroup.LayoutGroups)
                {
                    ProcessLayoutGroup(newLayoutGroupObject, layoutGroup1);
                }
            if (layoutGroup.Visibilities != null)
                foreach (var visibility in layoutGroup.Visibilities)
                {
                    ProcessVisibility(newLayoutGroupObject, visibility);
                }
        }


        private FancyBalloon.BaloonStyles GetBalloonStyle(HomeClientSettings.Room.Visibility.ShowBalloonClass.BalloonTypes balloonType)
        {
            switch (balloonType)
            {
                case HomeClientSettings.Room.Visibility.ShowBalloonClass.BalloonTypes.Normal:
                    return FancyBalloon.BaloonStyles.Normal;
                case HomeClientSettings.Room.Visibility.ShowBalloonClass.BalloonTypes.Info:
                    return FancyBalloon.BaloonStyles.Info;
                case HomeClientSettings.Room.Visibility.ShowBalloonClass.BalloonTypes.Warning:
                    return FancyBalloon.BaloonStyles.Warning;
                case HomeClientSettings.Room.Visibility.ShowBalloonClass.BalloonTypes.Exclamation:
                    return FancyBalloon.BaloonStyles.Exclamation;
                case HomeClientSettings.Room.Visibility.ShowBalloonClass.BalloonTypes.Alarm:
                    return FancyBalloon.BaloonStyles.Alarm;
                case HomeClientSettings.Room.Visibility.ShowBalloonClass.BalloonTypes.Error:
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
                _homeSettings = JsonConvert.DeserializeObject<HomeClientSettings>(File.ReadAllText(SettingsFileName));
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return false;
            }
        }

        private void StartWatchingClick(object sender, RoutedEventArgs e)
        {
            _client.SendMessage($"{HsEnvelope.HomeServerCommands}", HsEnvelope.StartListening);
        }

        private void ConnectToServer()
        {
            //AsynchronousClient.Start();
            _client.ConnectToServer(ServerName);
            // Восстановление плагинов
            if(_plugins != null)
                foreach (var plugin in _plugins)
                {
                    plugin.ResumeFromSleep();
                }

            _skype.Attach();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //            _modbusMasterThread.StopListening();
            //            _modbusMasterThread.Close();
            _client?.Disconnect();
        }

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

            //            _modbusMasterThread.SetCurrentTime();

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
            {
                e.Cancel = true;
                Hide();
            }
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
            //            _modbusMasterThread.StopListening();
            //StopListening();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //            _modbusMasterThread.GetTempAndHym();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void WriteBiSettingsClick(object sender, RoutedEventArgs e)
        {
            if (_client == null || IudAlarmPeriod.Value == null || IudInfoPeriod.Value == null)
                return;
            //_client.SendMessage(HsEnvelope.ControllersSetValue + "/bolid_bi_info_alarm_period", ((IudInfoPeriod.Value.Value << 8) | IudAlarmPeriod.Value.Value).ToString());
            //Thread.Sleep(100);

            var bytes = File.ReadAllBytes("eeprom.bin");
            var strBytes = bytes.Select(b => $"{b:X}").ToList();

            /*

                        var now = DateTime.Now.Subtract(TimeSpan.FromMinutes(30));
                        for (var i = 0; i < 45; i++)
                        {
                            var h = now.Hour;
                            if (i%2 == 0)
                                h |= 0x20;
                            strBytes.Add($"{h:X}");
                            var m = now.Minute;
                            strBytes.Add($"{m:X}");
                            now = now.AddMinutes(2);
                            //_client.SendMessage(HsEnvelope.ControllersSetValue + "/bolid_bi_add_event", 0x3344.ToString());
                            //Thread.Sleep(1500);
                        }
            */
            var msg = string.Join(",", strBytes);
            //_client.SendMessage(HsEnvelope.ControllersSetValue + "/bolid_bi_write_all_events", msg);
            _client.SendMessage(HsEnvelope.ControllersSetValue + "/bolid_bi_write_all_sounds", msg);

        }

        private void AddSoundBytes(List<byte> bytes)
        {
            try
            {
                var soundObjs = JsonConvert.DeserializeObject<TabloSounds[]>(TbSoundJson.Text);
                File.WriteAllText(SoundJsonFile, TbSoundJson.Text);
                bytes.Add((byte)soundObjs.Length);

                var curOffset = 0;
                for (var i = 0; i < soundObjs.Length; i++)
                {
                    bytes.Add((byte)curOffset);
                    curOffset += 1 + soundObjs[i].Sequense.Length * 3;
                }

                for (var i = 0; i < soundObjs.Length; i++)
                {
                    var curObj = soundObjs[i];
                    bytes.Add((byte)curObj.Sequense.Length);
                    foreach (var soundAtom in curObj.Sequense)
                    {
                        bytes.AddRange(soundAtom.ToBytes());
                    }
                }

                //                var strBytes = bytes.Select(b => $"{b:X}").ToList();
                //                var msg = string.Join(",", strBytes);
                //                _client.SendMessage(HsEnvelope.ControllersSetValue + "/bolid_bi_write_all_sounds", msg);
                //                MessageBox.Show("Звуки отправлены!");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void UploadDiaryClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var diaryObjs = JsonConvert.DeserializeObject<TabloDiaryItem[]>(TbDiaryJson.Text);
                File.WriteAllText(DiaryJsonFile, TbDiaryJson.Text);
                var bytes = new List<byte>
                {
                    0xff, // 2  EE_EVENT_ACCEPT_TIME
                    12, // 3 EE_MAX_EVENTS
                    0xff, // 4 Reserved
                    0xff, // 5 Reserved
                    0xff, // 6 Reserved
                    0xff, // 7 Reserved
                    0xff, // 8 Reserved
                    0xff, // 9 Reserved
                    (byte)diaryObjs.Length
                };


                for (var i = 0; i < diaryObjs.Length; i++)
                {
                    var curObj = diaryObjs[i];

                    var tmmpByte = (byte)((int)curObj.PlayDuration << 5);

                    tmmpByte |= (byte)curObj.Time.Hour;
                    bytes.Add(tmmpByte);
                    tmmpByte = (byte)((int)curObj.SoundId << 6);
                    tmmpByte |= (byte)curObj.Time.Minute;
                    bytes.Add(tmmpByte);

                }

                // sounds
                AddSoundBytes(bytes);



                var strBytes = bytes.Select(b => $"{b:X}").ToList();
                var msg = string.Join(",", strBytes);
                _client.SendMessage(HsEnvelope.ControllersSetValue + "/bolid_bi_write_settings", msg);
                MessageBox.Show("Расписание отправлено!");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }

        private void BGmail_Click(object sender, RoutedEventArgs e)
        {
            //var gn = new GmailNotify.GmailNotify.GmailNotify();

        }

        //инициализируем объект класса Skype, с ним в дальнейшем и будем работать
        /*
                private void button_Click_2(object sender, RoutedEventArgs e)
                {
                    try
                    {
                        //подписываемся на новые сообщения
                        skype.MessageStatus += OnMessageReceived;

                        //пытаемся присоединиться к скайпу. В данный момент вылезет окошко, где он у вас спросит разрешения на открытие доступа программе.
                        //5 это версия протокола (идёт по-умолчанию), true - отключить ли отваливание по таймауту для запроса к скайпу.
                        skype.Attach();
                        Console.WriteLine("skype attached");
                    }
                    catch (Exception ex)
                    {
                        //выводим в консольку, если что-то не так
                        Console.WriteLine("top lvl exception : " + ex.ToString());
                    }
                }

                private void OnMessageReceived(ChatMessage pmessage, TChatMessageStatus status)
                {
                    //суть такова, что для каждого сообщения меняется несколько статусов, поэтому мы ловим только те, у которых статус cmsReceived + это не позволит в будущем реагировать нашему боту на свои же сообщения
                    //if (status == TChatMessageStatus.cmsReceived)
                    {
                        Console.WriteLine($"{status}: {pmessage.Body}");
                    }
                    Console.WriteLine($"Missed {skype.MissedMessages.Count}");
                }
        */

        private void ClearLogClick(object sender, RoutedEventArgs e)
        {
            LbLog.Items.Clear();
        }
    }
}
