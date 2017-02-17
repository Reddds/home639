using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using HomeServer.Models;
using HomeServer.Objects;
using Microsoft.Win32;
using Modbus.Device;

namespace HomeServer
{
    static class ModbusMasterThread
    {
        private static string _portName;
        private static int _baudRate;
        private static int _heartBeatMs;
        private static Thread _mainThread;
        private static ManualResetEvent _isListening;
        public static ManualResetEvent ThreadStopped;

        private static SerialPort _port;
        private static ModbusSerialMaster _modbus;

        /// <summary>
        /// Поток работает
        /// </summary>
        private static bool IsWorked;

        public static event EventHandler PortChanged;
        public static event EventHandler<bool> ListeningChanged;


        public static event EventHandler<string> WriteToLog;
        public static event EventHandler<Tuple<string, bool>> SendControllerStatus;


        private static bool _setCurrentTime;

        public static List<ControllerGroup> ControolerObjects = new List<ControllerGroup>();
        public static Dictionary<string, HomeServerSettings.ActiveValue> ActiveValues;
        public static void Init(string portName, int baudRate, int heartBeatMs)
        {
            _portName = portName;
            _baudRate = baudRate;
            _heartBeatMs = heartBeatMs;
            _isListening = new ManualResetEvent(false);
            ThreadStopped = new ManualResetEvent(false);
            SystemEvents.PowerModeChanged += OnPowerChanged;

            _mainThread = new Thread(ThreadProc);
        }


        public static bool IsPortOpen => _port != null && _port.IsOpen;

        /// <summary>
        /// Начало опроса всех датчиков
        /// </summary>
        public static bool StartListening()
        {
            if (!CreatePort())
                return false;
            ListeningChanged?.Invoke(null, true);
            WriteToLog?.Invoke(null, "Начало опросов");

            _modbus = ModbusSerialMaster.CreateRtu(_port);
            _modbus.Transport.Retries = 1;
            //            _readRs485Timer.Start();
            IsWorked = true;
            if (!_mainThread.IsAlive)
                _mainThread.Start();
            _isListening.Set();
            Console.WriteLine(@"Start thread");

            

            return true;
        }

        /// <summary>
        /// Конец опроса всех датчиков
        /// </summary>
        public static void StopListening()
        {
            WriteToLog?.Invoke(null, "Конец опросов");
            _isListening.Reset();


            ClosePort();
            ListeningChanged?.Invoke(null, false);
        }

        private static bool CreatePort()
        {
            if (string.IsNullOrEmpty(_portName))
            {
                return false;
            }
            if (_port == null)
            {
                if(Options.Current.Verbose)
                    Console.WriteLine($"Opening port {_portName} {_baudRate}...");
                _port = new SerialPort(_portName)
                {
                    BaudRate = _baudRate,
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
                    PortChanged?.Invoke(null, EventArgs.Empty);
                    return true;
                }
                catch (Exception ee)
                {
                    Console.WriteLine(Options.Current.Verbose? ee.ToString(): ee.Message);
                    WriteToLog?.Invoke(null, ee.Message);
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        private static void ClosePort()
        {
            if (_port != null && _port.IsOpen)
            {
                _port.Close();
                PortChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static void Start()
        {
            IsWorked = true;
            _mainThread.Start();
        }

        /// <summary>
        /// Останавливаем поток
        /// </summary>
        /// <param name="wait">Ждять завершения</param>
        public static void Close(bool wait = false)
        {
            // Если на паузе, то можно и абортнуть, никто не пострадает
            if (IsPaused)
                _mainThread.Abort();
            // А если работает, то выключаем безопасно
            else
                IsWorked = false;
            if(wait)
                ThreadStopped.WaitOne(10000);
        }

        public static bool IsPaused => !_isListening.WaitOne(0);

        public static void SetCurrentTime()
        {
            if (IsPaused)
            {
                SendTime();
            }
            else
                _setCurrentTime = true;
        }

        private static void OnPowerChanged(object sender, PowerModeChangedEventArgs e)
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

        private static void SendTime()
        {
            var curTime = DateTime.Now;
            var timeData = new ushort[3];
            timeData[0] = (ushort)((curTime.Hour << 8) | curTime.Minute);
            timeData[1] = (ushort)((curTime.Day << 8) | curTime.Second);
            timeData[2] = (ushort)((curTime.Year << 8) | (curTime.Month % 100));

            //            _modbus.WriteMultipleRegisters(2, 8, timeData);
            _modbus.WriteMultipleRegisters(2, 0, timeData);
            Thread.Sleep(500);

            var setTimeRes = _modbus.ReadInputRegisters(2, 1, 1);
            if (setTimeRes.Length > 0 && setTimeRes[0] == 0xffff)
                WriteToLog?.Invoke(null, "Время установлено успешно!");
            else
            {
                WriteToLog?.Invoke(null, $"Ошибка установки времени! ({setTimeRes[0]:X4})");
                //MessageBox.Show($"Ошибка установки времени! ({setTimeRes[0]:X4})");
            }

        }

        private static void ThreadProc()
        {
            ThreadStopped.Reset();
            do
            {
                var currentControllerAddress = 0;
                var curAction = string.Empty;
                var now = DateTime.Now;
                _isListening.WaitOne();
                if (!IsWorked)
                    break;
                try
                {
                    // Проверяем, если порт закрыт, ждём открытия
                    if (!_port.IsOpen)
                    {
                        WriteToLog?.Invoke(null, "Порт неожиданно закрылся. Ждём открытия...");
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
                        WriteToLog?.Invoke(null, "Порт открылся! Продолжаем.");
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

                    //Thread.Sleep(200);

//                    var startDt = DateTime.Now;
                    if (ControolerObjects != null && ControolerObjects.Count > 0)
                    {
                        foreach (var room in ControolerObjects)
                        {
                            foreach (var controller in room.ShControllers)
                            {
//                                Console.WriteLine($"{controller.Id}: Start: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");
                                if (controller.ErrorCount >= 30)
                                {
                                    // теперь опрашиваем текущий контроллер только раз в 30 минут
                                    if (now.Subtract(controller.LastAccess).TotalMinutes < 30)
                                        continue;
                                }
                                if (controller.ErrorCount >= 5)
                                {
                                    // теперь опрашиваем текущий контроллер только раз в минуту
                                    if (now.Subtract(controller.LastAccess).TotalMinutes < 1)
                                        continue;
                                }
                                controller.LastAccess = now;
                                currentControllerAddress = controller.SlaveAddress;

                                try
                                {
                                    curAction = "GetStatus";
                                    if (!controller.GetStatus(_modbus))
                                    {
                                        BanController(controller, currentControllerAddress);
                                        continue;
                                    }
                                    if (controller.State == null)
                                    {
                                        controller.State = true;
                                        SendControllerStatus?.Invoke(null, new Tuple<string, bool>(controller.Id, true));
                                    }
//                                    Console.WriteLine($"{controller.Id}: After GetStatus: {DateTime.Now.Subtract(startDt).TotalMilliseconds}");
                                    
                                    if (controller.ErrorCount > 0)
                                    {
                                        Console.WriteLine($"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress} now work!");
                                        WriteToLog?.Invoke(null, $"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress} now work!");
                                        controller.State = true;
                                        SendControllerStatus?.Invoke(null, new Tuple<string, bool>(controller.Id, true));
                                        controller.ErrorCount = 0;
                                    }
                                }
                                catch (Exception eee)
                                {
                                    Console.WriteLine($"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress}; Action: {curAction}; {eee}");
                                    WriteToLog?.Invoke(null, $"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress}; Action: {curAction}; {eee}");
                                    BanController(controller, currentControllerAddress);
                                }
//                                Console.WriteLine($"{controller.Id}: End: {DateTime.Now.Subtract(startDt).TotalMilliseconds}\n");
                                //Console.WriteLine("Iteration Controller Address: " + controller.SlaveAddress);
                            }
                        }

                        // Отправка значений
                        foreach (var room in ControolerObjects)
                        {
                            foreach (var controller in room.ShControllers)
                            {

                                if (controller.ErrorCount >= 30)
                                {
                                    // теперь опрашиваем текущий контроллер только раз в 30 минут
                                    if (now.Subtract(controller.LastAccess).TotalMinutes < 30)
                                        continue;
                                }
                                if (controller.ErrorCount >= 5)
                                {
                                    // теперь опрашиваем текущий контроллер только раз в минуту
                                    if (now.Subtract(controller.LastAccess).TotalMinutes < 1)
                                        continue;
                                }
                                controller.LastAccess = now;
                                currentControllerAddress = controller.SlaveAddress;

                                try
                                {
                                    curAction = "SetCoils";
                                    controller.SetCoils(_modbus);
                                    curAction = "SetSetters";
                                    controller.SetSetters(_modbus);
                                    Thread.Sleep(10);
                                    if (controller.ErrorCount > 0)
                                    {
                                        Console.WriteLine($"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress} now work!");
                                        WriteToLog?.Invoke(null, $"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress} now work!");
                                        controller.State = true;
                                        SendControllerStatus?.Invoke(null, new Tuple<string, bool>(controller.Id, true));
                                        controller.ErrorCount = 0;
                                    }
                                }
                                catch (Exception eee)
                                {
                                    Console.WriteLine($"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress}; Action: {curAction}; {eee}");
                                    WriteToLog?.Invoke(null, $"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress}; Action: {curAction}; {eee}");
                                    BanController(controller, currentControllerAddress);
                                }
                                //Console.WriteLine("Iteration Controller Address: " + controller.SlaveAddress);
                            }
                        }

                        // После цикла сбрасываем флаги изменений 
                        foreach (var activeValue in ActiveValues)
                        {
                            activeValue.Value.ResetChange();
                        }
                        
                    }

                    Thread.Sleep(_heartBeatMs);
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception ee)
                {
                    Console.WriteLine($"Address = {currentControllerAddress}; Action: {curAction}; {ee}");
                    WriteToLog?.Invoke(null, $"Address = {currentControllerAddress}; Action: {curAction}; {ee}");
                }
            } while (true);
            ThreadStopped.Set();
        }

        private static void BanController(ShController controller, int currentControllerAddress)
        {
            controller.ErrorCount++;
            if (controller.ErrorCount == 5)
            {
                controller.State = false;
                SendControllerStatus?.Invoke(null, new Tuple<string, bool>(controller.Id, false));
                Console.WriteLine($"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress} Now access every minute");
                WriteToLog?.Invoke(null, $"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress} Now access every minute");
            }
            if (controller.ErrorCount == 20)
            {
                Console.WriteLine($"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress} Now access every 30 minutes");
                WriteToLog?.Invoke(null, $"{controller.ControllerGroupName} / {controller.Name} : {currentControllerAddress} Now access every 30 minutes");
            }

        }

    }
}
