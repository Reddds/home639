using System;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace HomeServer
{
    

    public interface IHsResult
    {
        
    }
/*
    public enum EnvelopeTypes
    {
        /// <summary>
        /// Данные получены и обработаны успешно
        /// </summary>
        ReseiveSuccess,
        /// <summary>
        /// Данные получены и обработаны с ошибками. Ошибка в Packet
        /// </summary>
        ReseiveFail,

        StartListening,
        StopListening,

        BoolResult = 200,
        UInt16Result,
        DateTimeResult,

        SetAction = 300,
        SetActionOnRegister,
        SetActionOnRegisterDateTime,

        ResetParameter = 400
    }
*/

    public enum CheckBoolStatus { OnTrue, OnFalse, OnBoth }

    public class HsEnvelope
    {
        /// <summary>
        /// Общий идентификатор темы умного дома
        /// </summary>
        public const string HomeServerTopic = "homebrain";

        /// <summary>
        /// Управляющие комманды
        /// </summary>
        public const string HomeServerCommands = "homeservercommands";
        /// <summary>
        /// Передача настроек
        /// </summary>
        public const string ControllersSettings = "controllerssettings";

        /// <summary>
        /// Передача данных с контроллеров
        /// </summary>
        public const string ControllersResult = "controllersresult";

        /// <summary>
        /// Результат установки значения
        /// </summary>
        public const string ControllersSetterResult = "controllerssetterresult";

        /// <summary>
        /// Передача на контроллер
        /// </summary>
        public const string ControllersSetValue = "controllerssetvalue";

        /// <summary>
        /// Запрос ID устройства
        /// </summary>
        public const string ControllersIdRequest = "controllersidrequest";

        /// <summary>
        /// Возврат ID устройства
        /// </summary>
        public const string ControllersIdResponse = "controllersidresponse";

        /// <summary>
        /// Начало опроса контроллеров
        /// </summary>
        public const string StartListening = "startlistening";
        /// <summary>
        /// Конец опроса контроллеров
        /// </summary>
        public const string StopListening = "stoplistening";


        public const string BoolResult = "boolresult";
        public const string UInt16Result = "uint16result";
        public const string ULongResult = "ulongresult";
        public const string StringResult = "stringresult";

        public const string DateTimeResult = "datetimeresult";

        public const string SetAction = "setaction";
//        public const string SetActionOnRegister = "setactiononregister";
//        public const string SetActionOnRegisterDateTime = "setactiononregisterdatetime";

        public const string ResetParameter = "resetparameter";



        public const string DateTimeFormat = "O";
/*

        public EnvelopeTypes EnvelopeType { get; set; }
        public string Packet { get; set; }

        public HsEnvelope()
        {
            
        }

        public HsEnvelope(IHsResult result)
        {
            var boolResult = result as BoolResultData;
            if (boolResult != null)
            {
                EnvelopeType = EnvelopeTypes.BoolResult;
                Packet = JsonConvert.SerializeObject(boolResult);
                return;
            }
            var uint16Result = result as UInt16ResultData;
            if (uint16Result != null)
            {
                EnvelopeType = EnvelopeTypes.UInt16Result;
                Packet = JsonConvert.SerializeObject(uint16Result);
                return;
            }
            var dateTimeResult = result as DateTimeResultData;
            if (dateTimeResult != null)
            {
                EnvelopeType = EnvelopeTypes.DateTimeResult;
                Packet = JsonConvert.SerializeObject(dateTimeResult);
                return;
            }
        }*/

/*
        /// <summary>
        /// Берём первый конверт из принятых данных и удаляем этот кусок
        /// </summary>
        /// <param name="reseivedData"></param>
        public static HsEnvelope FromString(ref string reseivedData)
        {
            var regResult = EnvelopeRegex.Match(reseivedData);
            if (!regResult.Success)
                return null;
            var envelopeStr = regResult.Groups[1].Value;
            var envelopeLen = regResult.Length;
            try
            {
                return JsonConvert.DeserializeObject<HsEnvelope>(envelopeStr);
            }
            finally
            {
                reseivedData =
                    reseivedData.Substring(envelopeLen);
            }
        }

        public override string ToString()
        {
            var retStr = BeginEnvelopeStr;
            retStr += JsonConvert.SerializeObject(this);
            retStr += EndEnvelopeStr;
            return retStr;
        }

*/
        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }
    }

    /// <summary>
    /// Прописывает метод, который будет вызван после изменения статуса дискретного регистра или катушки
    /// </summary>
    public class SetAction
    {
        /// <summary>
        /// Id события, которое будет вызываться
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// При каком событии вызывать метод
        /// </summary>
        public CheckBoolStatus CheckBoolStatus { get; set; }
        /// <summary>
        /// Id параметра
        /// </summary>
        public string ParameterId { get; set; }
        /// <summary>
        /// Вызывать callback даже когда показания не изменились
        /// </summary>
        public bool RaiseOlwais { get; set; }
        /// <summary>
        /// Интервал опроса датчиков
        /// </summary>
        public TimeSpan? CheckInterval { get; set; }

        //        /// <summary>
        //        /// Начальное состояние
        //        /// </summary>
        //        public bool? InitialState { get; set; }
        /// <summary>
        /// Сброс значения после изменения состояния только для катушек и CheckBoolStatus != OnBoth
        /// </summary>
        public bool ResetAfter { get; set; }
    }

    public class BoolResultData : IHsResult
    {
        /// <summary>
        /// Id события, которое будет вызываться
        /// </summary>
        public string ActionId { get; set; }
        public bool Data { get; set; }
    }

    /// <summary>
    /// Добавить проверку регистра
    /// </summary>
    public class SetActionOnRegisterData
    {
        /// <summary>
        /// Id события, которое будет вызываться
        /// </summary>
        public string ActionId { get; set; }
        /// <summary>
        /// Id параметра
        /// </summary>
        public string ParameterId { get; set; }
        /// <summary>
        /// Вызывать callback даже когда показания не изменились
        /// </summary>
        public bool RaiseOlwais { get; set; }
        /// <summary>
        /// Интервал опроса датчиков
        /// </summary>
        public TimeSpan? CheckInterval { get; set; }
    }

    public class UInt16ResultData : IHsResult
    {
        /// <summary>
        /// Id события, которое будет вызываться
        /// </summary>
        public string ActionId { get; set; }
        public ushort Data { get; set; }
    }


    /// <summary>
    /// Добавить проверку трёх регистров с возвратом даты
    /// </summary>
    public class SetActionOnRegisterDateTimeData
    {
        /// <summary>
        /// Id события, которое будет вызываться
        /// </summary>
        public string ActionId { get; set; }
        /// <summary>
        /// Id параметра
        /// </summary>
        public string ParameterId { get; set; }
        /// <summary>
        /// Вызывать callback даже когда показания не изменились
        /// </summary>
        public bool RaiseOlwais { get; set; }
        /// <summary>
        /// Интервал опроса датчиков
        /// </summary>
        public TimeSpan? CheckInterval { get; set; }
    }

    public class DateTimeResultData : IHsResult
    {
        /// <summary>
        /// Id события, которое будет вызываться
        /// </summary>
        public string ActionId { get; set; }
        public DateTime Data { get; set; }
    }


    class SocketExchangeClasses
    {
    }
}
