using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using HomeServer.Models;
using Newtonsoft.Json;

namespace HomeServer.Plugins
{
    class ForecatPlugin : PluginBase
    {
        private OpenWeatherMapResult.ListItem _curretnForecast = null;

        private System.Threading.Timer _getWeatherTimer;

        private readonly string _temperatureId;
        private readonly string _humidityId;
        private readonly string _pressureId;
        private readonly string _windId;
        private readonly string _weatherId;



        public ForecatPlugin(Dictionary<string, string> parameters) : base(parameters)
        {
            // 25 минут
            _getWeatherTimer = new System.Threading.Timer(state => { GetForecast(); }, null, 0, 25*60*1000);

            if (parameters != null)
            {
                if (parameters.ContainsKey("Temperature"))
                    _temperatureId = parameters["Temperature"];
                if (parameters.ContainsKey("Humidity"))
                    _humidityId = parameters["Humidity"];
                if (parameters.ContainsKey("Pressure"))
                    _pressureId = parameters["Pressure"];
                if (parameters.ContainsKey("Wind"))
                    _windId = parameters["Wind"];
                if (parameters.ContainsKey("Weather"))
                    _weatherId = parameters["Weather"];

            }
            
        }

        /* Ветер HI - направление, LO - скорость       
             * 0 - '? ю' 
             * 1 - '? с'
             * 2 - '? з',
             * 3 - '? в',
             * 4 - ' юз'
             * 5 - ' юв'
             * 6 - ' сз'
             * 7 - ' св'
             */
        private static byte ConvertWindDirection(double dir)
        {
            //N = 0º or 360º
            //NNE = 22.5º
            //NE = 45º
            //ENE = 67.5º
            //E = 90º
            //ESE = 112.5º
            //SE = 135º
            //SSE = 157.5º
            //S = 180º
            //SSW = 202.5º
            //SW = 225º
            //WSW = 247.5º
            //W = 270º
            //WNW = 292.5º
            //NW = 315º
            //NNW = 337.5º
            //N = 360º or 0º
            if (dir <= 22.5 || dir >= 337.5)
                return 1;
            if (dir > 22.5 && dir < 67.5)
                return 7;
            if (dir >= 67.5 && dir <= 112.5)
                return 3;
            if (dir > 112.5 && dir < 157.5)
                return 5;
            if (dir >= 157.5 && dir <= 202.5)
                return 0;
            if (dir > 202.5 && dir < 247.5)
                return 4;
            if (dir >= 247.5 && dir <= 292.5)
                return 2;
            if (dir > 292.5 && dir < 337.5)
                return 6;
            return 0;
        }

        public override void GetValues(List<HomeServerSettings.ActiveValue> activeValues)
        {
            if(_curretnForecast == null)
                return;


            if (!string.IsNullOrEmpty(_temperatureId))
            {
                activeValues.Find(_temperatureId)?.SetNewValue(_curretnForecast.MainData.Temp);               
            }

            if (!string.IsNullOrEmpty(_humidityId))
            {
                activeValues.Find(_humidityId)?.SetNewValue(_curretnForecast.MainData.Humidity);               
            }

            if (!string.IsNullOrEmpty(_pressureId))
            {
                activeValues.Find(_pressureId)?.SetNewValue(_curretnForecast.MainData.Pressure);               
            }

            /* Ветер HI - направление, LO - скорость       
             * 0 - '? ю' 
             * 1 - '? с'
             * 2 - '? з',
             * 3 - '? в',
             * 4 - ' юз'
             * 5 - ' юв'
             * 6 - ' сз'
             * 7 - ' св'
             */
            if (!string.IsNullOrEmpty(_windId))
            {
                ushort val = ConvertWindDirection(_curretnForecast.Wind.Direction);
                val <<= 8;
                val |= (byte)Math.Round(_curretnForecast.Wind.Speed);
                activeValues.Find(_windId)?.SetNewValue(val);               
            }

            /*
             * Тип погоды, осадки
             * 0 - ясно
             * 1 - облачно
             * 2 - пасмурно
             * 3 - дождь
             * 4 - снег
             * 
            */
            if (!string.IsNullOrEmpty(_weatherId))
            {
                var val = 0;
                var weatherId = _curretnForecast.Weather[0].Id;
                if (weatherId >= (WeatherCodes) 500 && weatherId < (WeatherCodes) 600)
                    val = 3;
                else if (weatherId >= (WeatherCodes)600 && weatherId < (WeatherCodes)700)
                    val = 4;
                else if (weatherId >= (WeatherCodes)801 && weatherId <= (WeatherCodes)802)
                    val = 1;
                else if (weatherId >= (WeatherCodes)803 && weatherId <= (WeatherCodes)804)
                    val = 2;


                activeValues.Find(_weatherId)?.SetNewValue(val);               
            }



            _curretnForecast = null;
        }

        private void GetForecast()
        {
            var forecastUrl =
                "http://api.openweathermap.org/data/2.5/forecast?lat=55.9352591&lon=37.9313304&appid=0434c6d01cbcbdfe5c51f15eb5fe9e5b&units=metric&lang=ru";

            OpenWeatherMapResult weatherForecast = null;

            var wrGETURL = WebRequest.Create(forecastUrl);

            var objStream = wrGETURL.GetResponse().GetResponseStream();
            if (objStream != null)
            {
                using (var objReader = new StreamReader(objStream))
                {
                    var str = objReader.ReadToEnd();
                    weatherForecast = JsonConvert.DeserializeObject<OpenWeatherMapResult>(str);
                }
            }



            if (weatherForecast != null)
            {
                var neededTime = DateTime.Now;
                if (neededTime.Hour > 3) // Если время больше 3 ночи, то начинаем выводить пгоду на следующий день
                    neededTime = neededTime.AddDays(1);
                // Погода на 3 часа дня
                neededTime = new DateTime(neededTime.Year, neededTime.Month, neededTime.Day, 15, 0, 0);

                _curretnForecast = (from fi in weatherForecast.ForeList
                                 where fi.Time >= neededTime && fi.Time < neededTime.AddHours(3)
                                 select fi).FirstOrDefault();


            }
        }
    }
}
