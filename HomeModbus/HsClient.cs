using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using HomeServer;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace HomeModbus
{
    class HsClient
    {
        private MqttClient _mqttClient;

        //        private readonly ConcurrentQueue<HsEnvelope> _messageQueue;
        private readonly Dictionary<string, Action<object>> _registeredActions;
        /// <summary>
        /// Вызывается при получении результата изменения значения
        /// </summary>
        private readonly Dictionary<string, Action<bool>> _setterResultActions;

        public HsClient()
        {

            _registeredActions = new Dictionary<string, Action<object>>();
            _setterResultActions = new Dictionary<string, Action<bool>>();
            //            _messageQueue = new ConcurrentQueue<HsEnvelope>();
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
            }, new[]
            {
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
            });
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {

            var strMessage = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);
            Console.WriteLine($"MQTT: {e.Topic} {strMessage}");

            var topicSplit = e.Topic.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (topicSplit.Length < 3)
                return;

            var resultKind = topicSplit[1];
            var parameterId = topicSplit[2];

            try
            {

                switch (resultKind)
                {
                    case HsEnvelope.ControllersResult:
                        if (topicSplit.Length < 4)
                            return;

                        var dataType = topicSplit[3];

                        var action = FindActionByParameterId(parameterId);
                        switch (dataType)
                        {
                            case HsEnvelope.BoolResult:
                                bool boolValue;
                                if (bool.TryParse(strMessage, out boolValue))
                                    action?.Invoke(boolValue);
                                break;
                            case HsEnvelope.UInt16Result:
                                ushort uint16Value;
                                if (ushort.TryParse(strMessage, out uint16Value))
                                    action?.Invoke(uint16Value);
                                break;
                            case HsEnvelope.StringResult:
                                action?.Invoke(strMessage);
                                break;
                            case HsEnvelope.DateTimeResult:
                                DateTime dateTimeValue;
                                if (DateTime.TryParseExact(strMessage, HsEnvelope.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
                                    action?.Invoke(dateTimeValue);
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
            if(_mqttClient.IsConnected)
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

        Action<object> FindActionByParameterId(string actionId)
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
            _registeredActions.Add(parameterId, (obj) =>
            {
                callback((newVal) =>
                {
                    SendMessage($"{HsEnvelope.ResetParameter}/{parameterId}", newVal?.ToString());
                }, obj);
            });
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
    }
}

