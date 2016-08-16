using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using HomeServer;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace HomeModbus
{
    class HsClient
    {
        private readonly Action<string> _writeToLog;
        private MqttClient _mqttClient;

        public Action<bool> OnServerListeningStatusChange;
        public Action UpdateLastTime;

        Timer CheckConnectionTimer;

        public class ActionEventArgs
        {
            public ActionEventArgs(object v) { Value = v; }
            public object Value { get; private set; } // readonly
        }

        // Declare the delegate (if using non-generic pattern).
        public delegate void ActionEventHandler(object sender, ActionEventArgs e);

        public class MyActionContainer
        {
            public event ActionEventHandler OnActionEvent;

            public void RaiseOnAction(object v)
            {
                OnActionEvent?.Invoke(this, new ActionEventArgs(v));
            }

        }

        //        private readonly ConcurrentQueue<HsEnvelope> _messageQueue;
        //private readonly Dictionary<string, Action<object>> _registeredActions;
        private readonly Dictionary<string, MyActionContainer> _registeredActions;

        private readonly Dictionary<string, Action<bool>> _statusChangeActions; 
        /// <summary>
        /// Вызывается при получении результата изменения значения
        /// </summary>
        private readonly Dictionary<string, Action<bool>> _setterResultActions;

        public HsClient(string address, Action<string> writeToLog)
        {
            _writeToLog = writeToLog;

            _registeredActions = new Dictionary<string, MyActionContainer>();
            _statusChangeActions = new Dictionary<string, Action<bool>>();
            _setterResultActions = new Dictionary<string, Action<bool>>();
            //            _messageQueue = new ConcurrentQueue<HsEnvelope>();

            CheckConnectionTimer = new Timer((state) =>
            {
                if (_mqttClient == null || !_mqttClient.IsConnected)
                {
                    try
                    {
                        ConnectToServer(address);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }, null, Timeout.Infinite, 60000);
        }

        public void ConnectToServer(string address)
        {
            // Соединяемся с удаленным устройством
            // create client instance 
            _mqttClient = new MqttClient(address);
            // register to message received 
            _mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            var clientId = Guid.NewGuid().ToString();
            _mqttClient.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
            _mqttClient.Subscribe(new[]
            {
                $"/{HsEnvelope.HomeServerTopic}/{HsEnvelope.ControllersResult}/#",
                $"/{HsEnvelope.HomeServerTopic}/{HsEnvelope.ControllersSetterResult}/#",
                $"/{HsEnvelope.HomeServerTopic}/{HsEnvelope.ServerStatus}/#",
                $"/{HsEnvelope.HomeServerTopic}/{HsEnvelope.ControllerStatus}/#",
            }, new[]
            {
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
            });

            CheckConnectionTimer.Change(0, 60000);
        }

        private bool? MessageToBool(string strMessage)
        {
            bool boolValue;
            if (bool.TryParse(strMessage, out boolValue))
                return boolValue;
            return null;
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {

            var strMessage = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);
            Console.WriteLine($"MQTT: {e.Topic} {strMessage}");

            var topicSplit = e.Topic.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (topicSplit.Length < 3)
            {
                return;
            }

            var resultKind = topicSplit[1];
            var parameterId = topicSplit[2];

            try
            {
                UpdateLastTime?.Invoke();
                switch (resultKind)
                {
                    case HsEnvelope.ServerStatus:
                        switch (parameterId)
                        {
                            case HsEnvelope.LogMessage:
                                _writeToLog?.Invoke(strMessage);
                                break;
                            case HsEnvelope.ListeningStatus:

                                var boolVal = MessageToBool(strMessage);
                                if (boolVal != null)
                                    OnServerListeningStatusChange?.Invoke(boolVal.Value);
                                break;
                                //OnServerListeningStatusChange 
                        }
                        break;

                    case HsEnvelope.ControllerStatus:
                        if (_statusChangeActions.ContainsKey(parameterId))
                        {
                            var boolVal = MessageToBool(strMessage);
                            if (boolVal != null)
                                _statusChangeActions[parameterId]?.Invoke(boolVal.Value);
                        }
                        break;
                    case HsEnvelope.ControllersResult:
                        if (topicSplit.Length < 4)
                            return;

                        var dataType = topicSplit[3];

                        var action = FindActionByParameterId(parameterId);
                        switch (dataType)
                        {
                            case HsEnvelope.BoolResult:
                                var boolVal = MessageToBool(strMessage);
                                if (boolVal != null)

                                    action?.RaiseOnAction(boolVal.Value);
                                break;
                            case HsEnvelope.UInt16Result:
                                ushort uint16Value;
                                if (ushort.TryParse(strMessage, out uint16Value))
                                    action?.RaiseOnAction(uint16Value);
                                break;
                            case HsEnvelope.DoubleResult:
                                double doubleValue;


                                if (double.TryParse(strMessage, NumberStyles.Number, CultureInfo.InvariantCulture, out doubleValue))
                                    action?.RaiseOnAction(doubleValue);
                                break;
                            case HsEnvelope.StringResult:
                                action?.RaiseOnAction(strMessage);
                                break;
                            case HsEnvelope.DateTimeResult:
                                DateTime dateTimeValue;
                                if (DateTime.TryParseExact(strMessage, HsEnvelope.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
                                    action?.RaiseOnAction(dateTimeValue);
                                break;
                        }

                        break;
                    case HsEnvelope.ControllersSetterResult:
                        var resultAction = FindResultActionByParameterId(parameterId);
                        bool resultBoolValue;
                        if (bool.TryParse(strMessage, out resultBoolValue))
                            resultAction?.Invoke(resultBoolValue);
                        break;
                }


                //Console.WriteLine(envelope.EnvelopeType.ToString());

            }
            catch (Exception ee)
            {
                Console.WriteLine("Не распознан!");
            }
        }

        public void Disconnect()
        {
            if (_mqttClient == null)
                return;
            /*
                        if (_sender == null || !_sender.Connected)
                            return;
                        // Освобождаем сокет
                        _sender.Shutdown(SocketShutdown.Both);
                        _sender.Close();
            */
            if (_mqttClient.IsConnected)
                _mqttClient.Disconnect();
            _mqttClient = null;
        }

        ~HsClient()
        {
            Disconnect();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic">Без начального слеша</param>
        /// <param name="message"></param>
        public void SendMessage(string topic, string message)
        {
            var msgId = _mqttClient.Publish($"/{HsEnvelope.HomeServerTopic}/{topic}", Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Console.WriteLine($"MQTT sent/ Message Id = {msgId}");
        }

        MyActionContainer FindActionByParameterId(string actionId)
        {
            if (_registeredActions == null)
                return null;
            return !_registeredActions.ContainsKey(actionId) ? null : _registeredActions[actionId];
        }
        Action<bool> FindResultActionByParameterId(string actionId)
        {
            if (_setterResultActions == null)
                return null;
            return !_setterResultActions.ContainsKey(actionId) ? null : _setterResultActions[actionId];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="callback">ResetAction, newValue</param>
        public void SetAction(string parameterId,
            Action<Action<object>, object> callback)
        {
            MyActionContainer newEventHandler;
            if (_registeredActions.ContainsKey(parameterId))
                newEventHandler = _registeredActions[parameterId];
            else
            {
                newEventHandler = new MyActionContainer();
                _registeredActions.Add(parameterId, newEventHandler);
            }
            newEventHandler.OnActionEvent += (sender, obj) =>
            {
                callback((newVal) =>
                {
                    SendMessage($"{HsEnvelope.ResetParameter}/{parameterId}", newVal?.ToString());
                }, obj.Value);
            };
            //            _registeredActions.Add(parameterId, (sender, obj) =>
            //            {
            //                callback((newVal) =>
            //                {
            //                    SendMessage($"{HsEnvelope.ResetParameter}/{parameterId}", newVal?.ToString());
            //                }, obj);
            //            });
            //            var registerActionOnRegister = new SetAction()
            //            {
            //                ActionId = actionHash,
            //                CheckBoolStatus = CheckBoolStatus,
            //                ParameterId = parameterId,
            //                RaiseOlwais = raiseOlwais,
            //                CheckInterval = interval
            //            };
            //            SendMessage($"{HsEnvelope.ControllersSettings}/{HsEnvelope.SetAction}", JsonConvert.SerializeObject(registerActionOnRegister));
        }
        public void SetResultAction(string parameterId,
            Action<bool> callback)
        {
            _setterResultActions.Add(parameterId, callback);
        }

        public void OnStatusChanged(string controllerId, Action<bool> action)
        {
            if(!_statusChangeActions.ContainsKey(controllerId))
                _statusChangeActions.Add(controllerId, action);
        }
    }
}

