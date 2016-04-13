﻿using System;
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

        public HsClient()
        {

            _registeredActions = new Dictionary<string, Action<object>>();
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
            }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {

            var strMessage = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);
            Console.WriteLine($"MQTT: {e.Topic} {strMessage}");

            var topicSplit = e.Topic.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (topicSplit.Length < 4)
                return;

            var parameterId = topicSplit[2];
            var dataType = topicSplit[3];

            var action = FindActionByParameterId(parameterId);

            try
            {
                switch (dataType)
                {
                    case HsEnvelope.BoolResult:
                        bool boolValue;
                        if(bool.TryParse(strMessage, out boolValue))
                            action?.Invoke(boolValue);
                        break;
                    case HsEnvelope.UInt16Result:
                        ushort uint16Value;
                        if (ushort.TryParse(strMessage, out uint16Value))
                            action?.Invoke(uint16Value);
                        break;
                    case HsEnvelope.DateTimeResult:
                        DateTime dateTimeValue;
                        if (DateTime.TryParseExact(strMessage, HsEnvelope.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
                            action?.Invoke(dateTimeValue);
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
            _mqttClient.Publish($"/{HsEnvelope.HomeServerTopic}/{topic}", Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        Action<object> FindActionByParameterId(string actionId)
        {
            if (_registeredActions == null)
                return null;
            return !_registeredActions.ContainsKey(actionId) ? null : _registeredActions[actionId];
        }

/*
        public void SetActionOnRegister(string parameterId, Action<ushort> action, bool raiseOlwais, TimeSpan? interval)
        {
            var actionHash = Guid.NewGuid().ToString();
            _registeredActions.Add(actionHash, (obj) =>
            {
                if (!obj.GetType().IsPrimitive)
                    return;
                action((ushort)obj);
            });

            var registerActionOnRegister = new SetActionOnRegisterData()
            {
                ActionId = actionHash,
                CheckInterval = interval,
                ParameterId = parameterId,
                RaiseOlwais = raiseOlwais
            };
            SendMessage($"{HsEnvelope.ControllersSettings}/{HsEnvelope.SetAction}", JsonConvert.SerializeObject(registerActionOnRegister));
        }
*/

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
//                CheckCoilStatus = checkCoilStatus,
//                ParameterId = parameterId,
//                RaiseOlwais = raiseOlwais,
//                CheckInterval = interval
//            };
//            SendMessage($"{HsEnvelope.ControllersSettings}/{HsEnvelope.SetAction}", JsonConvert.SerializeObject(registerActionOnRegister));
        }
    }
}

