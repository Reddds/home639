using System;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Threading;
using Chip45ProgrammerLib;
using Microsoft.Win32;

namespace Chip45Programmer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private SerialPort _port;
        private Chip45 _chip45;
        BackgroundWorker _backgroundWorker;


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


        public MainWindow()
        {
            InitializeComponent();

            BiMain.DataContext = this;
            DataContext = this;

            Maximum = 100;

            LbBaudRates.ItemsSource = _shortBaudrates;
            LbBaudRates.SelectedItem = Properties.Settings.Default.SelectedBaudrate;

            FillPorts();

            var comPortTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 5) };
            comPortTimer.Tick += (sender, args) =>
            {
                FillPorts();
            };
            comPortTimer.Start();


            CbSendPreString.IsChecked = Properties.Settings.Default.IsSendPreString;
            TbPreString.Text = Properties.Settings.Default.PreString;
            RbPreStringHex.IsChecked = Properties.Settings.Default.IsPreStringHex;
            IudPreStringTimeoutAfter.Value = Properties.Settings.Default.PreStringTimeoutAfterMs;

            IudEepromBytesToRead.Value = Properties.Settings.Default.EepromReadBytes;
            CbEepromWriteDelay.IsChecked = Properties.Settings.Default.IsEepromWriteDelay;
            IudEepromWriteDelay.Value = Properties.Settings.Default.EepromWriteDelayMs;

            RbTerminalSendHex.IsChecked = Properties.Settings.Default.IsTerminalSendHex;
            CbTerminalSendEndLine.IsChecked = Properties.Settings.Default.IsTerminalSendEndLine;



            /*
                        var svAutomation = (ListBoxAutomationPeer)UIElementAutomationPeer.CreatePeerForElement(LbLog);

                        var scrollInterface = (IScrollProvider)svAutomation.GetPattern(PatternInterface.Scroll);
                        const ScrollAmount scrollVertical = ScrollAmount.LargeIncrement;
                        const ScrollAmount scrollHorizontal = ScrollAmount.NoAmount;
                        //If the vertical scroller is not available, the operation cannot be performed, which will raise an exception. 
                        if (scrollInterface != null && scrollInterface.VerticallyScrollable)
                            scrollInterface.Scroll(scrollHorizontal, scrollVertical);
            */
        }

        private void FillPorts()
        {
            var ports = SerialPort.GetPortNames();
            LbPorts.ItemsSource = ports;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.SelectedCom))
                LbPorts.SelectedItem = Properties.Settings.Default.SelectedCom;
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            if (_port != null && _port.IsOpen)
                _port.Close();
        }


        private static byte[] HexStringToBytes(string hexStr)
        {
            hexStr = hexStr.Trim();
            // Удаляем пробелы
            while (hexStr.Contains(" "))
                hexStr = hexStr.Replace(" ", "");

            if ((hexStr.Length % 2) != 0)
            {
                MessageBox.Show("Нечётное количество символов в HEX");
                return null;
            }

            var arr = new byte[hexStr.Length / 2];
            try
            {
                for (var i = 0; i < hexStr.Length / 2; i++)
                {
                    arr[i] = Convert.ToByte(hexStr.Substring(i * 2, 2), 16);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                return null;
            }
            return arr;
        }

        private void BTerminalSendClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IsTerminalSendHex = RbTerminalSendHex.IsChecked == true;
            Properties.Settings.Default.IsTerminalSendEndLine = CbTerminalSendEndLine.IsChecked == true;
            Properties.Settings.Default.Save();

            var sendText = TbSend.Text;
            if (string.IsNullOrEmpty(sendText))
                return;


            byte[] arr = null;
            var preStringSrc = TbSend.Text.Trim();
            if (!string.IsNullOrEmpty(preStringSrc))
            {
                if (RbTerminalSendHex.IsChecked == true)
                {
                    if (Properties.Settings.Default.IsTerminalSendEndLine)
                        preStringSrc += "0A";
                    arr = HexStringToBytes(preStringSrc);
                }
                else
                {
                    if (Properties.Settings.Default.IsTerminalSendEndLine)
                        preStringSrc += "\n";
                    arr = Encoding.ASCII.GetBytes(preStringSrc.ToCharArray());
                }
            }

            if (arr == null)
                return;

            if (!CreatePort())
                return;

            _port.DataReceived += OnPortOnDataReceived;
            _port.Write(arr, 0, arr.Length);
        }

        private void OnPortOnDataReceived(object o, SerialDataReceivedEventArgs args)
        {
            var sp = (SerialPort)o;
            // Wait all symbols
            Thread.Sleep(200);
            if (_port.IsOpen)
            {
                if (Properties.Settings.Default.IsTerminalSendHex)
                {
                    var outArr = new byte[sp.BytesToRead];
                    sp.Read(outArr, 0, outArr.Length);
                    var outStr = string.Empty;
                    foreach (var b in outArr)
                    {
                        outStr += b.ToString("X2") + " ";
                    }
                    WriteLog(outStr);
                }
                else
                {
                    var received = sp.ReadExisting();
                    var outStr = Chip45.FormatControlChars(received);
                    WriteLog(outStr);
                }
            }
        }


        private bool CreatePort()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.SelectedCom))
            {
                MessageBox.Show("Выберите порт");
                return false;
            }
            if (_port == null)
            {
                _port = new SerialPort(Properties.Settings.Default.SelectedCom)
                {
                    BaudRate = Properties.Settings.Default.SelectedBaudrate,
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                };
            }
            else
            {
                _port.DataReceived -= OnPortOnDataReceived;
            }
            if (!_port.IsOpen)
            {
                try
                {
                    _port.Open();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                    return false;
                }
                OnPropertyChanged("PortOpened");
            }
            return true;
        }

        private void WriteLog(string msg)
        {
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

        private void BConnectClick(object sender, RoutedEventArgs e)
        {
            // Сохраняем значения PreString
            if (CbSendPreString.IsChecked != null)
                Properties.Settings.Default.IsSendPreString = CbSendPreString.IsChecked.Value;
            Properties.Settings.Default.PreString = TbPreString.Text;
            if (RbPreStringHex.IsChecked != null)
                Properties.Settings.Default.IsPreStringHex = RbPreStringHex.IsChecked.Value;
            if (IudPreStringTimeoutAfter.Value != null)
                Properties.Settings.Default.PreStringTimeoutAfterMs = IudPreStringTimeoutAfter.Value.Value;
            Properties.Settings.Default.Save();

            if (!CreatePort())
                return;

            /*            _port.DataReceived += (o, args) =>
                        {
                            var sp = (SerialPort)o;
            //                var outArr = new byte[sp.BytesToRead];
            //                sp.Read(outArr, 0, outArr.Length);
                            var outStr = sp.ReadExisting();
            //                foreach (var b in outArr)
            //                {
            //                    outStr += b.ToString("X2") + " ";
            //                }
                            Application.Current.Dispatcher.BeginInvoke(
                              DispatcherPriority.Background,
                              new Action(() => TbResponse.Text += outStr + Environment.NewLine));

                        };*/


            if (_chip45 == null)
                _chip45 = new Chip45(_port, true, true, WriteLog);

            byte[] preString = null;
            var preStringSrc = TbPreString.Text.Trim();
            if (!string.IsNullOrEmpty(preStringSrc))
            {
                if (RbPreStringHex.IsChecked == true)
                    preString = HexStringToBytes(preStringSrc);
                else
                    preString = Encoding.ASCII.GetBytes(preStringSrc.ToCharArray());
            }

            var arg = new Chip45.ConnectOptions
            {
                BytesBeforeConnect = preString,
                PreStringTimeoutAfterMs = IudPreStringTimeoutAfter.Value ?? 1000
            };

            RunWorker(_chip45.ConnectBootloader, true, arg, complete: (o, args) =>
             {
                 Connected = _chip45.Connected;
             });
        }

        private void RunWorker(DoWorkEventHandler runCommand, bool canCancel = false, object arg = null, RunWorkerCompletedEventHandler complete = null)
        {

            BiMain.IsBusy = true;

            _backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true };
            if (canCancel)
            {
                CancelVisible = Visibility.Visible;
                _backgroundWorker.WorkerSupportsCancellation = true;
            }
            else CancelVisible = Visibility.Collapsed;

            _backgroundWorker.RunWorkerCompleted += (o, args) =>
            {
                BiMain.IsBusy = false;
            };
            if (complete != null)
                _backgroundWorker.RunWorkerCompleted += complete;

            _backgroundWorker.ProgressChanged += (sender, args) =>
            {
                Value = args.ProgressPercentage;
                if (args.UserState != null)
                    BusyText = args.UserState as string;
            };
            _backgroundWorker.DoWork += runCommand;
            BiMain.IsBusy = true;
            _backgroundWorker.RunWorkerAsync(arg);
        }

        private void BDisconnect_Click(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }

        private void Disconnect()
        {
            RunWorker(_chip45.DisconnectBootloader, arg: null, complete: (o, args) => { Connected = _chip45.Connected; ClosePort(); });
        }

        public bool Connected
        {
            get { return _connected; }
            set
            {
                _connected = value;
                //                CbConnected.IsChecked = _connected;
                if (_connected)
                {
                    //                    BConnect.IsEnabled = false;
                    //                    BDisconnect.IsEnabled = true;
                    if (!string.IsNullOrWhiteSpace(FpFlash.FileName))
                        BProgramFlash.IsEnabled = true;
                }
                else
                {
                    //                    BDisconnect.IsEnabled = false;
                    BProgramFlash.IsEnabled = false;
                    //                    BConnect.IsEnabled = true;
                }
                OnPropertyChanged("Connected");
            }
        }

        public bool PortOpened => _port != null && _port.IsOpen;


        private void BProgramFlashClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FpFlash.FileName))
            {
                MessageBox.Show("Выберите файл с прошивкой");
                return;
            }
            var filePath = FpFlash.FileName.Trim();
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл с прошивкой не найден");
                return;
            }


            var arg = new Chip45.ProgramOptions
            {
                ProgramType = Chip45.ProgramTypes.Flash,
                ProgramFile = filePath
            };

            RunWorker(_chip45.Program, arg: arg);

        }

        private void FpFlash_FileSelected(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(FpFlash.FileName))
                BProgramFlash.IsEnabled = true;
            else
                BProgramFlash.IsEnabled = false;
        }

        private void BHexFileTest_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FpFlash.FileName))
            {
                MessageBox.Show("Выберите файл с прошивкой");
                return;
            }
            var filePath = FpFlash.FileName.Trim();
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл с прошивкой не найден");
                return;
            }

            HexFileTester.Test(filePath, WriteLog);
        }

        #region BusyIndicator
        private double _value;
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                this.OnPropertyChanged("Value");
            }
        }


        private double _maximum;
        public double Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
                this.OnPropertyChanged("Maximum");
            }
        }


        private string _busyText;
        public string BusyText
        {
            get
            {
                return _busyText;
            }
            set
            {
                _busyText = value;
                this.OnPropertyChanged("BusyText");
            }
        }

        private Visibility _cancelVisible;
        public Visibility CancelVisible
        {
            get
            {
                return _cancelVisible;
            }
            set
            {
                _cancelVisible = value;
                this.OnPropertyChanged("CancelVisible");
            }
        }

        private bool _connected;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion BusyIndicator

        private void BCancel(object sender, RoutedEventArgs e)
        {
            if (_backgroundWorker.WorkerSupportsCancellation)
            {
                if (
                    MessageBox.Show("Really cancel operation?", "Cancellation", MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    return;
                _backgroundWorker.CancelAsync();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_backgroundWorker != null && _backgroundWorker.IsBusy)
            {
                MessageBox.Show("Wait current operation");
                e.Cancel = true;
            }
        }

        private void SelectPort(object sender, SelectionChangedEventArgs e)
        {
            var lb = (ListBox)sender;

            var selectedPort = lb.SelectedItem as string;
            if (selectedPort == null)
                return;

            Properties.Settings.Default.SelectedCom = selectedPort;
            Properties.Settings.Default.Save();
        }

        private void SelectBaudrate(object sender, SelectionChangedEventArgs e)
        {
            var lb = (ListBox)sender;

            if (lb.SelectedItem == null)
                return;

            Properties.Settings.Default.SelectedBaudrate = (int)lb.SelectedItem;
            Properties.Settings.Default.Save();
        }

        private void TogglePortClick(object sender, RoutedEventArgs e)
        {
            if (PortOpened)
            {
                if (Connected)
                {
                    if (MessageBox.Show("Disconnect from bootloader?", "Connected to bootloader", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        Disconnect();
                    else
                    {
                        Connected = false;
                        ClosePort();
                    }
                }
                else
                {
                    ClosePort();
                }
            }
            else
            {
                CreatePort();
            }
        }

        private void ClosePort()
        {
            if (_port != null && _port.IsOpen)
            {
                _port.Close();
                OnPropertyChanged("PortOpened");
            }
        }

        private void FpFlashEepromSelected(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(FpEeprom.FileName))
                BProgramEeprom.IsEnabled = true;
            else
                BProgramEeprom.IsEnabled = false;
        }

        private void BProgramEepromClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FpEeprom.FileName))
            {
                MessageBox.Show("Please choose Eeprom file");
                return;
            }
            var filePath = FpEeprom.FileName.Trim();
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Eeprom file not found!");
                return;
            }

            Properties.Settings.Default.IsEepromWriteDelay = CbEepromWriteDelay.IsChecked == true;
            if (IudEepromWriteDelay.Value != null)
                Properties.Settings.Default.EepromWriteDelayMs = IudEepromWriteDelay.Value.Value;
            Properties.Settings.Default.Save();

            var arg = new Chip45.ProgramOptions
            {
                ProgramType = Chip45.ProgramTypes.Eeprom,
                ProgramFile = filePath
            };
            if (CbEepromWriteDelay.IsChecked == true && IudEepromWriteDelay.Value != null)
                arg.EepromWriteDelay = IudEepromWriteDelay.Value.Value;

            RunWorker(_chip45.Program, arg: arg);
        }

        private void BReadEepromClick(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Eeprom|*.eep;*.hex",
                Title = "Choose filename to save Eeprom",
                DefaultExt = ".eep"
            };
            if (sfd.ShowDialog() != true)
                return;

            Properties.Settings.Default.EepromReadBytes = IudEepromBytesToRead.Value ?? 512;
            Properties.Settings.Default.Save();

            var arg = new Chip45.ReadEepromOptions
            {
                EepromSaveFileName = sfd.FileName,
                BytesToRead = IudEepromBytesToRead.Value ?? 512
            };

            RunWorker(_chip45.ReadEeprom, true, arg, complete: (o, args) =>
            {
                if (args.Result != null && (bool)args.Result)
                {
                    MessageBox.Show("Read Eeprom completed");
                }
                else
                {
                    MessageBox.Show("Error reading Eeprom");
                }
            });

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != true)
                return;

            var sfd = new SaveFileDialog
            {
                Filter = "Hex|*.hex;*.eep",
                DefaultExt = ".hex"
            };
            if(sfd.ShowDialog() != true)
                return;

            var hexFile = new HexFile(WriteLog, true);
            hexFile.AddRange(File.ReadAllBytes(ofd.FileName));
            if (!string.IsNullOrEmpty(hexFile.ErrorString))
            {
                MessageBox.Show(hexFile.ErrorString);
                return;
            }
            HexUtils.WriteHexfile(sfd.FileName, hexFile);
        }

        private void ConvertToUnfragmentedHexClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Hex|*.hex;*.eep",
                DefaultExt = ".hex"
            };

            if (ofd.ShowDialog() != true)
                return;

            var sfd = new SaveFileDialog
            {
                Filter = "Hex|*.hex;*.eep",
                DefaultExt = ".hex"
            };
            if(sfd.ShowDialog() != true)
                return;

            var hexFile = new HexFile(WriteLog, true, true, 0xFF);
            hexFile.Load(ofd.FileName);
            if (!string.IsNullOrEmpty(hexFile.ErrorString))
            {
                MessageBox.Show(hexFile.ErrorString);
                return;
            }
            HexUtils.WriteHexfile(sfd.FileName, hexFile);
        }


        private void ClearLogClick(object sender, RoutedEventArgs e)
        {
            LbLog.Items.Clear();
        }
    }
}
