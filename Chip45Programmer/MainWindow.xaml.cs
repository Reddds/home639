using System;
using System.Collections.Generic;
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
using Chip45Programmer.Windows;
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

            InitDs30();

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

        private void InitDs30()
        {
            CbDs30RowSize.ItemsSource = new[] { 8, 16, 32, 64, 128 };
            CbDs30RowSize.SelectedIndex = 0;
            CbDs30PageSize.ItemsSource = new[] { 64, 128 };
            CbDs30PageSize.SelectedIndex = 0;
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
            if (_port != null && _port.PortName != Properties.Settings.Default.SelectedCom)
            {
                if (_port.IsOpen)
                    _port.Close();
                _port = null;
            }
            if (_port == null)
            {
                _port = new SerialPort(Properties.Settings.Default.SelectedCom)
                {
                    BaudRate = Properties.Settings.Default.SelectedBaudrate,
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    ReadTimeout = 2000,
                    Encoding = Encoding.GetEncoding("Windows-1251")
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

            Ds30.Port = _port;
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

            if (Equals(TcBootloaders.SelectedItem, TiChip45))
            {
                if (!Connected)
                {
                    MessageBox.Show("Please connect to bootloader first");
                    return;
                }
                var arg = new Chip45.ProgramOptions
                {
                    ProgramType = Chip45.ProgramTypes.Flash,
                    ProgramFile = filePath
                };

                RunWorker(_chip45.Program, arg: arg);
            }
            else if (Equals(TcBootloaders.SelectedItem, TiDs30))
            {
                Ds30.Ds30ProgramFlash(WriteLog, FpFlash.FileName, (int)CbDs30RowSize.SelectionBoxItem, (int)CbDs30PageSize.SelectionBoxItem);
            }
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

        private void BFillEepromClick(object sender, RoutedEventArgs e)
        {
            if (IudEepromBytesToRead.Value == null)
            {
                MessageBox.Show("Enter fbll size!");
                return;
            }
            var arg = new Chip45.ProgramOptions
            {
                ProgramType = Chip45.ProgramTypes.FillEeprom,
                EepromFillByte = 0xff,
                EepromFillCount = IudEepromBytesToRead.Value.Value
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

            if (Equals(TcBootloaders.SelectedItem as TabItem, TiChip45))
            {
                var arg = new Chip45.ReadEepromOptions
                {
                    EepromSaveFileName = sfd.FileName,
                    BytesToRead = IudEepromBytesToRead.Value ?? 512
                };

                RunWorker(_chip45.ReadEeprom, true, arg, complete: (o, args) =>
                {
                    if (args.Result != null && (bool) args.Result)
                    {
                        MessageBox.Show("Read Eeprom completed");
                    }
                    else
                    {
                        MessageBox.Show("Error reading Eeprom");
                    }
                });
            }
            else if (Equals(TcBootloaders.SelectedItem as TabItem, TiDs30))
            {
                if (!CreatePort())
                    return;

                var helloResult = Ds30.Ds30FirstConnect();
                if (helloResult == null || helloResult.Item5 != Ds30.Ds30Ok)
                {
                    MessageBox.Show("Status is not Ok!");
                    return;
                }


            }

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
            if (sfd.ShowDialog() != true)
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
            if (sfd.ShowDialog() != true)
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




        private void Ds30TestConnect(object sender, RoutedEventArgs e)
        {
            if (!CreatePort())
                return;

            var helloResult = Ds30.Ds30FirstConnect();
            if (helloResult == null || helloResult.Item5 != Ds30.Ds30Ok)
                MessageBox.Show("Status is not Ok!");
            else
                MessageBox.Show($"Connected!\nDeviceId={helloResult.Item1}\nVersion={helloResult.Item2}.{helloResult.Item3}.{helloResult.Item4}");
        }



/*
        Tuple<int, byte, byte, byte, int> Ds30FirstConnect()
        {
            if (!PortOpened)
                return null;
            var buf = new byte[1];
            buf[0] = Ds30Hello;
            try
            {
                _port.DiscardInBuffer();
                var startTime = DateTime.Now;
                do
                {
                    _port.Write(buf, 0, buf.Length);
                    Thread.Sleep(5);
                    if (_port.BytesToRead > 0)
                        break;
                    Thread.Sleep(20);
                } while (DateTime.Now.Subtract(startTime) < _helloWaitTime);


                ushort deviceId = 0;
                var deviceLowId = _port.ReadByte();
                deviceId = (ushort)deviceLowId;
                var tmp = _port.ReadByte();
                deviceId |= (ushort)((ushort)(tmp & 0x80) << 1);
                var verMajor = (byte)(tmp & 0x7F);
                tmp = _port.ReadByte();
                deviceId |= (ushort)((ushort)(tmp & 0x80) << 2);
                var verMinor = (byte)((tmp >> 4) & 0x0f);
                var verRev = (byte)(tmp & 0x0f);
                var status = _port.ReadByte();
                Thread.Sleep(20);
                return new Tuple<int, byte, byte, byte, int>(deviceId, verMajor, verMinor, verRev, status);
            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось установить связь с загрузчиком\n" + ee);
            }
            return null;
        }
*/

/*
        void Ds30ProgramFlash()
        {//FpFlash
            var curPos = 0;
            try
            {

                const byte emptyByte = 0xff;
                var hexFile = new HexFile(WriteLog, true, true, emptyByte);
                hexFile.Load(FpFlash.FileName);

                //var hexLines = hexFile.GetHexFile((int) CbDs30RowSize.SelectedValue);
                var rowSize = (int)CbDs30RowSize.SelectionBoxItem;
                var pageSize = (int)CbDs30PageSize.SelectionBoxItem;


                var writeBuf = new byte[rowSize];
                var allLen = hexFile.Count;
                var needWrite = false;

                var helloResult = Ds30FirstConnect();
                if (helloResult == null || helloResult.Item5 != Ds30Ok)
                {
                    MessageBox.Show("Status is not Ok!");
                    return;
                }

                var lastPageErased = -1;

                do
                {
                    var i = 0;
                    for (; i < rowSize && curPos + i < allLen; i++)
                    {
                        writeBuf[i] = hexFile[curPos + i];
                        if (!needWrite && writeBuf[i] != emptyByte)
                            needWrite = true;
                    }
                    if (needWrite)
                    {
                        var curPage = curPos / pageSize;

                        if (curPage != lastPageErased)// || curPos %pageSize == 0
                        {
                            if (Ds30WriteCommand(Ds30Commands.ErasePage, curPos, null).Item1 != Ds30Ok)
                            {
                                MessageBox.Show($"Error erasing page at address {curPos}!");
                                break;
                            }
                            lastPageErased = curPage;
                        }
                        if (Ds30WriteCommand(Ds30Commands.WriteInCode, curPos, writeBuf).Item1 != Ds30Ok)
                        {
                            MessageBox.Show($"Error writing row at address {curPos}!");
                            break;
                        }
                    }
                    curPos += i;
                } while (curPos < allLen);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
*/

/*
        byte[] Ds30ReadCodeMem(int startAddress, int length)
        {
            var rowSize = (int) CbDs30RowSize.SelectionBoxItem;
            var retBuf =  new byte[length];
            var rowsCount = length/rowSize + (length%rowSize > 0 ? 1 : 0);
            var curAddress = startAddress;
            
            for (var i = 0; i < rowsCount; i++, curAddress += rowSize)
            {
                var readResult = Ds30WriteCommand(Ds30Commands.ReadCodeMem, curAddress, null);
                if (readResult.Item1 != Ds30Ok)
                {
                    return null;
                }
                for (var j = 0; j < rowSize; j++)
                {
                    retBuf[i*rowSize + j] = readResult.Item2[j];
                }
            }
            return retBuf;
            
        }
*/

/*        Tuple<int, byte[]> Ds30WriteCommand(Ds30Commands command, int address, byte[] bufBytes)
        {

            byte[] buf = null;
            byte checkSum = 0;
            switch (command)
            {
                case Ds30Commands.ErasePage:
                    buf = new byte[3 + 1 + 1 + 1];
                    buf[3] = 0x01;
                    checkSum += buf[3];
                    buf[4] = 0x01;
                    checkSum += buf[4];
                    //buf[5] = 0x00;
                    break;
                case Ds30Commands.WriteInCode:
                case Ds30Commands.WriteInEeprom:
                    buf = new byte[3 + 1 + 1 + bufBytes.Length + 1];
                    buf[3] = (byte)(command == Ds30Commands.WriteInCode ? 0x02 : 0x04);
                    checkSum += buf[3];
                    buf[4] = (byte)(bufBytes.Length + 1);
                    checkSum += buf[4];
                    for (var i = 0; i < bufBytes.Length; i++)
                    {
                        buf[5 + i] = bufBytes[i];
                        checkSum += buf[5 + i];
                    }
                    break;
                case Ds30Commands.WriteConfig:
                    buf = new byte[3 + 1 + 1 + 1 + 1];
                    buf[3] = 0x08;
                    checkSum += buf[3];
                    buf[4] = 0x02;
                    checkSum += buf[4];
                    buf[5] = bufBytes[0];
                    checkSum += buf[5];
                    break;
                case Ds30Commands.ReadCodeMem:
                    buf = new byte[3 + 1 + 1 + 1];
                    buf[3] = 0x10;
                    checkSum += buf[3];
                    buf[4] = 0x01;
                    checkSum += buf[4];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);

            }

            buf[0] = (byte)((address >> 16) & 0xff);
            checkSum += buf[0];
            buf[1] = (byte)((address >> 8) & 0xff);
            checkSum += buf[1];
            buf[2] = (byte)(address & 0xff);
            checkSum += buf[2];

            buf[buf.Length - 1] = (byte)(0xFF - checkSum + 1);

            //_port.Write(buf, 0, buf.Length);

            foreach (var b in buf)
            {
                _port.BaseStream.WriteByte(b);
                Thread.Sleep(1);
            }


            //Thread.Sleep(10);
            if (command == Ds30Commands.ReadCodeMem)
            {
                var checkSumForTest = (byte)0;

                var bytesCount = (byte)_port.ReadByte();
                checkSumForTest += bytesCount;
                var readbuf = new byte[bytesCount];

                for (var i = 0; i < readbuf.Length; i++)
                {
                    readbuf[i] = (byte)_port.ReadByte();
                    checkSumForTest += readbuf[i];
                }
                var readCheckSum = (byte)_port.ReadByte();
                if (readCheckSum == checkSumForTest)
                    return new Tuple<int, byte[]>(Ds30Ok, readbuf);
                else
                {
                    return new Tuple<int, byte[]>(Ds30CheckSumErr, null);
                }
            }
            return new Tuple<int, byte[]>(_port.ReadByte(), null);
        }*/

        private void Ds30TestRead(object sender, RoutedEventArgs e)
        {
            var helloResult = Ds30.Ds30FirstConnect();
            if (helloResult == null || helloResult.Item5 != Ds30.Ds30Ok)
            {
                MessageBox.Show("Status is not Ok!");
                return;
            }
            var readed = Ds30.Ds30ReadCodeMem(0, 0x1000, (int)CbDs30RowSize.SelectionBoxItem);

            if (readed == null)
            {
                MessageBox.Show("Reading is not Ok!");
                return;
            }
            var hexFile = new HexFile(WriteLog, true, true, 0xff);
            hexFile.AddRange(readed);
            
            MessageBox.Show(string.Join("\n", hexFile.GetHexFile()));
        }

        private void Ds30ReadConfigs(object sender, RoutedEventArgs e)
        {
            Ds30.Ds30ReadConfigs((int)CbDs30RowSize.SelectionBoxItem);
        }



        private void Ds30SendConfigByteClick(object sender, RoutedEventArgs e)
        {
            Ds30.Ds30SendConfigByte();
        }

        private void Ds30ReadByteClick(object sender, RoutedEventArgs e)
        {
            if (IudByteAddress.Value == null)
            {
                MessageBox.Show("Please enter valid address!");
                return;
            }
            Ds30.Ds30ReadByte(IudByteAddress.Value.Value, (int)CbDs30RowSize.SelectionBoxItem);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            // Flash
            if (string.IsNullOrWhiteSpace(FpFlash.FileName))
            {
                LFlashFileModified.Content = null;
                return;
            }
            if (!File.Exists(FpFlash.FileName))
            {
                LFlashFileModified.Content = null;
                return;
            }
            var fi = new FileInfo(FpFlash.FileName);
            LFlashFileModified.Content = fi.LastWriteTime.ToString("F");
            
        }
    }
}
