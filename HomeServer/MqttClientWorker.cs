using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using HomeServer.Models;
using HomeServer.Objects;
using Newtonsoft.Json.Serialization;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace HomeServer
{
    class MqttClienWorker
    {
        private readonly MqttClient _mqttClient;
//        private readonly List<ShController> _allControllers;
//        private readonly HomeServerSettings.ControllerGroup[] _controllerGroups;
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

        public MqttClienWorker(string address, List<ShController> allControllers, HomeServerSettings.ControllerGroup[] controllerGroups)
        {
//            _allControllers = allControllers;
//            _controllerGroups = controllerGroups;
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
                $"/{HsEnvelope.HomeServerTopic}/{HsEnvelope.ControllersIdRequest}/#",
            }, new[]
            {
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
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

                case HsEnvelope.ControllersIdRequest:
                    //!!!
                    return;

                case HsEnvelope.ControllersSetValue:
                    ProceedSetValue(topicSplit, strMessage);
                    
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

        public static void ProceedSetValue(string[] topicSplit, string strMessage)
        {
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
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.RealDateTime:
                        DateTime newVal;
                        if (DateTime.TryParseExact(strMessage, HsEnvelope.DateTimeFormat, CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out newVal))
                        {
                            setter.PendingSet(newVal);
                        }
                        break;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.UInt16:
                        ushort newUint16;
                        if (ushort.TryParse(strMessage, out newUint16))
                        {
                            setter.PendingSet(newUint16);
                        }

                        break;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.MultipleUInt16:
                        var strValues = strMessage.Split(',');
                        var newValues = new ushort[strValues.Length];
                        for (var i = 0; i < strValues.Length; i++)
                        {
                            ushort tmpUInt16;
                            if (ushort.TryParse(strValues[i], out tmpUInt16))
                            {
                                newValues[i] = tmpUInt16;
                            }
                            else
                            {
                                return;
                            }
                        }
                        setter.PendingSet(newValues);
                        break;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.File:
                        var strInts = strMessage.Split(',');
                        var bytes = new List<byte>();
                        foreach (var strInt in strInts)
                        {
                            try
                            {
                                bytes.Add(Convert.ToByte(strInt, 16));
                            }
                            catch (Exception)
                            {
                                break;
                            }
                            setter.PendingSet(bytes.ToArray());
                        }
                        break;
                    case HomeServerSettings.ControllerGroup.Controller.Setter.SetterTypes.Command:
                        strValues = strMessage.Split(',');
                        var commandValues = new int[strValues.Length];
                        for (var i = 0; i < strValues.Length; i++)
                        {
                            int tmpint;
                            bool tmpBool;
                            if (int.TryParse(strValues[i], out tmpint))
                            {
                                commandValues[i] = tmpint;
                            }
                            else if (bool.TryParse(strValues[i], out tmpBool))
                            {
                                commandValues[i] = tmpBool ? 1 : 0;
                            }
                            else
                            {
                                return;
                            }
                        }
                        setter.PendingSet(commandValues);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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
