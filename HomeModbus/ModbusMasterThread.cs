using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using HomeModbus.Objects;
using log4net;
using Microsoft.Win32;
using Modbus.Device;

namespace HomeModbus
{
    class ModbusMasterThread
    {
        private readonly Thread _mainThread;
        private readonly ManualResetEvent _isListening;



        private SerialPort _port;
        private ModbusSerialMaster _modbus;




        public event EventHandler PortChanged;
        public event EventHandler<bool> ListeningChanged;


        public event EventHandler<string> WriteToLog;


        private bool _setCurrentTime;
        /// <summary>
        /// Получить температуру и влажность
        /// </summary>
        private bool _getTempAndHym;
        /// <summary>
        /// Сбросить вызов
        /// </summary>
        private bool _resetCall;

        public List<Room> Rooms = new List<Room>(); 

        public ModbusMasterThread()
        {
            _isListening = new ManualResetEvent(false);
            SystemEvents.PowerModeChanged += OnPowerChanged;

            _mainThread = new Thread(ThreadProc);
        }

        public bool IsPortOpen => _port != null && _port.IsOpen;

        /// <summary>
        /// Начало опроса всех датчиков
        /// </summary>
        public void StartListening()
        {
            if(!CreatePort())
                return;
            ListeningChanged?.Invoke(this, true);
            WriteToLog?.Invoke(this, "Начало опросов");

            _modbus = ModbusSerialMaster.CreateRtu(_port);
            _modbus.Transport.Retries = 1;
            //            _readRs485Timer.Start();
            if (!_mainThread.IsAlive)
                _mainThread.Start();
            _isListening.Set();
        }

        /// <summary>
        /// Конец опроса всех датчиков
        /// </summary>
        public void StopListening()
        {
            WriteToLog?.Invoke(this, "Конец опросов");
            _isListening.Reset();


            ClosePort();
            ListeningChanged?.Invoke(this, false);
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
                    ReadTimeout = 600
                };
            }
            if (!_port.IsOpen)
            {
                try
                {
                    _port.Open();
                    PortChanged?.Invoke(this, EventArgs.Empty);
                    return true;
                }
                catch (Exception ee)
                {
                    WriteToLog?.Invoke(this, ee.Message);
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        private void ClosePort()
        {
            if (_port != null && _port.IsOpen)
            {
                _port.Close();
                PortChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Start()
        {
            _mainThread.Start();
        }

        public void Close()
        {
            _mainThread.Abort();
        }

        private bool IsPaused
        {
            get { return _isListening.WaitOne(0); }
        }

        public void SetCurrentTime()
        {
            if (IsPaused)
            {
                SendTime();
            }
            else
                _setCurrentTime = true;
        }

        private void OnPowerChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    if (_mainThread.IsAlive)
                        StartListening();
                    break;
                case PowerModes.Suspend:
                    StopListening();
                    break;
            }
        }

        private void SendTime()
        {
            var curTime = DateTime.Now;
            var timeData = new ushort[3];
            timeData[0] = (ushort)((curTime.Hour << 8) | curTime.Minute);
            timeData[1] = (ushort)((curTime.Day << 8) | curTime.Second);
            timeData[2] = (ushort)((curTime.Month << 8) | (curTime.Year % 100));

            //            _modbus.WriteMultipleRegisters(2, 8, timeData);
            _modbus.WriteMultipleRegisters(2, 0, timeData);
            Thread.Sleep(500);

            var setTimeRes = _modbus.ReadInputRegisters(2, 1, 1);
            if (setTimeRes.Length > 0 && setTimeRes[0] == 0xffff)
                WriteToLog?.Invoke(this, "Время установлено успешно!");
            else
            {
                WriteToLog?.Invoke(this, $"Ошибка установки времени! ({setTimeRes[0]:X4})");
                //MessageBox.Show($"Ошибка установки времени! ({setTimeRes[0]:X4})");
            }

        }


        public void GetTempAndHym()
        {
            _getTempAndHym = true;
        }

        private void ModbusGetTempAndHym()
        {
            var tempAndHym = _modbus.ReadInputRegisters(2, 2, 1)[0];
            var temp = (sbyte)(tempAndHym >> 8);
            var hym = (sbyte)(tempAndHym & 0xFF);

            WriteToLog?.Invoke(this, $"В спальне: Температура: {temp} *С; Влажность: {hym} %");
        }

        public void ResetCall()
        {
            _resetCall = true;
        }
        public void ModbusResetCall()
        {
            _modbus?.WriteSingleCoil(2, 4, false);
        }

        private void ThreadProc()
        {
            do
            {
                var currentControllerAddress = 0;
                _isListening.WaitOne();
                try
                {
                    // Проверяем, если порт закрыт, ждём открытия
                    if (!_port.IsOpen)
                    {
                        WriteToLog?.Invoke(this, "Порт неожиданно закрылся. Ждём открытия...");
                        do
                        {
                            Thread.Sleep(2000);
                            try
                            {
                                _port.Open();
                            }
                            catch (Exception)
                            {
                                //ignored
                            }
                        } while (!_port.IsOpen);
                        WriteToLog?.Invoke(this, "Порт открылся! Продолжаем.");
                    }
                    if (_setCurrentTime)
                    {
                        _setCurrentTime = false;
                        SendTime();
                    }
//                    if (_getTempAndHym)
//                    {
//                        _getTempAndHym = false;
//                        ModbusGetTempAndHym();
//                    }
//                    if (_resetCall)
//                    {
//                        _resetCall = false;
//                        ModbusResetCall();
//                    }

                    Thread.Sleep(200);
                  

                    if (Rooms != null && Rooms.Count > 0)
                    {
                        foreach (var room in Rooms)
                        {
                            foreach (var controller in room.ShControllers)
                            {
                                currentControllerAddress = controller.SlaveAddress;
                                controller.GetStatus(_modbus);
                                Thread.Sleep(20);
                                controller.DoActions(_modbus);
                                Thread.Sleep(20);
                            }
                        }
                    }


                    Thread.Sleep(1000);
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception ee)
                {
                    WriteToLog?.Invoke(this, $"Address = {currentControllerAddress}; {ee.Message}");
                }
            } while (true);

        }

    }
}
