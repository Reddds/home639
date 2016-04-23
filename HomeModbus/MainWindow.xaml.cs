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
using Chip45Programmer;
using Chip45ProgrammerLib;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Gauges;
using HomeModbus.Controls;
using HomeModbus.Models;
using HomeModbus.Models.Base;
using HomeModbus.Tooltip;
using HomeServer;
using log4net;
using log4net.Config;
using Microsoft.Win32;
using LayoutGroup = DevExpress.Xpf.Docking.LayoutGroup;
using Visibility = HomeModbus.Models.Visibility;

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

        private const string SettingsFileName = "HomeSettings.xml";
        private HomeSettings _homeSettings;

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
            _client = new HsClient();

            if (LoadSettings())
                ApplySettings();

            ConnectToServer();

            SystemEvents.PowerModeChanged += OnPowerChanged;
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
            foreach (var room in _homeSettings.Rooms)
            {
                var roomTab = new RoomTab { Header = room.Name };
                MainTabs.Items.Add(roomTab);

                foreach (var layoutGroup in room.Layout.LayoutGroup)
                {
                    ProcessLayoutGroup(roomTab.MainPanel, layoutGroup);
                }
                foreach (var visibility in room.Layout.Visibility)
                {
                    ProcessVisibility(roomTab.MainPanel, visibility);
                }
            }
        }

        private void ProcessVisibility(LayoutGroup layoutGroupObject, Visibility visibility)
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


        private void CreateVisibilitySetter(LayoutGroup layoutGroupObject, Visibility visibility)
        {
            var setterControl = new IndicatorControl
            {
                Caption = visibility.Name
            };
            layoutGroupObject.Add(setterControl);
            if (visibility.CurrentTime != null)
            {
                var simpleSetter = new SimpleSetter {TbName = {Content = "Установить текущее время"}};
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
        }


        private void CreateVisibilityParameter(LayoutGroup layoutGroupObject,
            Visibility visibility)
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
            AnalogIndicator analogControl = null;
            LastTimeIndicator lastTimeControl = null;
            Series chartSerie = null;// MainChartLog.Diagram.Series[0];


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
                    if (visibility.ShowBalloon != null)
                    {
                        if (value.GetType().IsPrimitive)
                            if ((bool)value)
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
                            analogControl.Value = (ushort)value;
                    }
                    if (lastTimeControl != null)
                    {
                        if (value is DateTime)
                            lastTimeControl.Value = (DateTime)value;
                    }
                    if (simpleIndicator != null)
                    {
                        if (value.GetType().IsPrimitive)
                            simpleIndicator.Value = (ushort)value;
                    }
                    if (chartSerie != null)
                    {
                        var curTime = DateTime.Now;
                        if (value.GetType().IsPrimitive)
                            chartSerie.Points.Add(new SeriesPoint(curTime, (ushort)value));

                    }

                });
            });
        }

        private void ProcessLayoutGroup(LayoutGroup layoutGroupObject, Models.LayoutGroup layoutGroup)
        {
            var newLayoutGroupObject = new LayoutGroup();
            layoutGroupObject.Items.Add(newLayoutGroupObject);
            newLayoutGroupObject.Orientation = layoutGroup.Orientation == Orientations.Horizontal
                ? Orientation.Horizontal
                : Orientation.Vertical; foreach (var layoutGroup1 in layoutGroup.LayoutGroup1)
            {
                ProcessLayoutGroup(newLayoutGroupObject, layoutGroup1);
            }
            foreach (var visibility in layoutGroup.Visibility)
            {
                ProcessVisibility(newLayoutGroupObject, visibility);
            }
        }


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

        private void StartWatchingClick(object sender, RoutedEventArgs e)
        {
            _client.SendMessage($"{HsEnvelope.HomeServerCommands}", HsEnvelope.StartListening);
        }

        private void ConnectToServer()
        {
            //AsynchronousClient.Start();
            _client.ConnectToServer(ServerName);


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
    }
}
