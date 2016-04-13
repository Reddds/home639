using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using HomeServer.Models;
using HomeServer.Objects;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace HomeServer
{
    class ClienSocketWorker
    {
        private readonly MqttClient _mqttClient;
        private readonly List<ShController> _allControllers;
        private readonly List<HomeSettingsControllerGroup> _controllerGroups;
        //        private readonly ConcurrentQueue<HsEnvelope> _messageQueue = new ConcurrentQueue<HsEnvelope>();

        /// <summary>
        /// Функции сброса параметров
        /// Key = parameterId
        /// </summary>
        public static Dictionary<string, Action> ParameterResets = new Dictionary<string, Action>();
        /// <summary>
        /// Функции сброса параметров
        /// Key = parameterId
        /// </summary>
        public static Dictionary<string, ShController.ModbusSetter> Setters = new Dictionary<string, ShController.ModbusSetter>();

        public ClienSocketWorker(string address, List<ShController> allControllers, List<HomeSettingsControllerGroup> controllerGroups)
        {
            _allControllers = allControllers;
            _controllerGroups = controllerGroups;
            //var ipHost = Dns.GetHostEntry("tor");
            //var ipAddr = ipHost.AddressList[0];//IPAddress.Parse("192.168.88.240");// ipHost.AddressList[0];
            // create client instance 
            _mqttClient = new MqttClient(address);
            // register to message received 
            _mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            var clientId = Guid.NewGuid().ToString();
            _mqttClient.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
            _mqttClient.Subscribe(new[]
            {
                $"/{HsEnvelope.HomeServerTopic}/{HsEnvelope.HomeServerCommands}/#",
                $"/{HsEnvelope.HomeServerTopic}/{HsEnvelope.ControllersSettings}/#",
                $"/{HsEnvelope.HomeServerTopic}/{HsEnvelope.ControllersSetValue}/#",
                $"/{HsEnvelope.HomeServerTopic}/{HsEnvelope.ResetParameter}/#",
            }, new[]
            {
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
            });

            //mqttClient.Publish("/homeserver/temperature", Encoding.UTF8.GetBytes("sdklfhifweuihfweihfi hwehfiwh ifhwie hfi"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Console.WriteLine(@"Connected to broker");
        }

        public void SendMessage(string topic, string message, bool retain)
        {
            _mqttClient.Publish($"/{HsEnvelope.HomeServerTopic}/{topic}", Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, retain);
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {

            var strMessage = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);
            Console.WriteLine($"MQTT: {e.Topic} {strMessage}");

            var topicSplit = e.Topic.Split(new []{'/'}, StringSplitOptions.RemoveEmptyEntries);
            if (topicSplit.Length < 2)
                return;

            switch (topicSplit[1])
            {
                case HsEnvelope.HomeServerCommands:
                    switch (strMessage)
                    {
                        case HsEnvelope.StartListening:
                            ModbusMasterThread.StartListening();
                            Console.WriteLine("Starting");
                            return;

                        case HsEnvelope.StopListening:
                            ModbusMasterThread.StopListening();
                            Console.WriteLine("Stopping");
                            return;
                    }
                    return;

                case HsEnvelope.ControllersSettings:
                    if (topicSplit.Length < 3)
                        return;
//                    ParseControllerSetting(topicSplit[2], strMessage);
                    return;

                case HsEnvelope.ControllersSetValue:
                    if (topicSplit.Length < 3)
                        return;
                    var setterId = topicSplit[2];
                    if (Setters.ContainsKey(setterId))
                    {
                        var setter = Setters[setterId];
                        if (setter == null)
                            return;
                        switch (setter.SetterType)
                        {
                            case SetterTypes.RealDateTime:
                                DateTime newVal;
                                if (DateTime.TryParseExact(strMessage, HsEnvelope.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,   out newVal))
                                {
                                    setter.PendingSet(newVal);
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        
                    }
                    return;
                //"{HsEnvelope.ControllersRequest}/{HsEnvelope.ResetParameter}/{parameterId}", newVal 
                case HsEnvelope.ResetParameter: 
                    if (topicSplit.Length < 3)
                        return;
                    var parameterId = topicSplit[2];
                    if(ParameterResets.ContainsKey(parameterId))
                        ParameterResets[parameterId]?.Invoke();
                    return;
            }
        }

        /// <summary>
        /// Берём из данных первый конверт
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private object GetEnvelopeStr(string data)
        {
            throw new NotImplementedException();
        }
    }

}
