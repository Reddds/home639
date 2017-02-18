using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HomeServer.Models
{
    public enum WeatherCodes
    {
        //Group 2xx: Thunderstorm
        ThunderstormWithLightRain = 200,
        ThunderstormWithRain = 201,
        ThunderstormWithHeavyRain = 202,
        LightThunderstorm = 210,
        Thunderstorm = 211,
        HeavyThunderstorm = 212,
        RaggedThunderstorm = 221,
        ThunderstormWithLightDrizzle = 230,
        ThunderstormWithDrizzle = 231,
        ThunderstormWithHeavyDrizzle = 232,

        //Group 3xx: Drizzle
        LightIntensityDrizzle = 300,
        Drizzle = 301,
        HeavyIntensityDrizzle = 302,
        LightIntensityDrizzleRain = 310,
        DrizzleRain = 311,
        HeavyIntensityDrizzleRain = 312,
        ShowerRainAndDrizzle = 313,
        HeavyShowerRainAndDrizzle = 314,
        ShowerDrizzle = 321,

        //Group 5xx: Rain
        LightRain = 500,
        ModerateRain = 501,
        HeavyIntensityRain = 502,
        VeryHeavyRain = 503,
        ExtremeRain = 504,
        FreezingRain = 511,
        LightIntensityShowerRain = 520,
        ShowerRain = 521,
        HeavyIntensityShowerRain = 522,
        RaggedShowerRain = 531,

        //Group 6xx: Snow
        LightSnow = 600,
        Ssnow = 601,
        HeavySnow = 602,
        Sleet = 611,
        ShowerSleet = 612,
        LightRainAndSnow = 615,
        RainAndSnow = 616,
        LightShowerSnow = 620,
        ShowerSnow = 621,
        HeavyShowerSnow = 622,

        //Group 7xx: Atmosphere
        Mist = 701,
        Smoke = 711,
        Haze = 721,
        SandDustWhirls = 731,
        Fog = 741,
        Sand = 751,
        Dust = 761,
        VolcanicAsh = 762,
        Squalls = 771,
        Tornado = 781,

        //Group 800: Clear
        ClearSky = 800,

        //Group 80x: Clouds
        FewClouds = 801,
        ScatteredClouds = 802,
        BrokenClouds = 803,
        OvercastClouds = 804,

        //Group 90x: Extreme
        TornadoEx = 900,
        TropicalStorm = 901,
        Hurricane = 902,
        Cold = 903,
        Hot = 904,
        Wndy = 905,
        Hail = 906,

        //Group 9xx: Additional
        Calm = 951,
        LightBreeze = 952,
        GentleBreeze = 953,
        ModerateBreeze = 954,
        FreshBreeze = 955,
        StrongBreeze = 956,
        HighWindNearGale = 957,
        Gale = 958,
        SevereGale = 959,
        Storm = 960,
        ViolentStorm = 961,
        HurricaneAd = 962,
    }


    public class OpenWeatherMapResult
    {



        public class CityItem
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class ListItem
        {
            public class MainDataItem
            {
                [JsonProperty("temp")]
                public double Temp { get; set; }
                /// <summary>
                /// Atmospheric pressure on the sea level by default, hPa
                /// </summary>
                [JsonProperty("pressure")]
                public double Pressure { get; set; }
                [JsonProperty("humidity")]
                public double Humidity { get; set; }

            }

            public class WeatherItem
            {
                [JsonProperty("id")]
                public WeatherCodes Id { get; set; }
            }

            public class WindItem
            {
                [JsonProperty("speed")]
                public double Speed { get; set; }
                /// <summary>
                /// Wind direction, degrees (meteorological)
                /// </summary>
                [JsonProperty("deg")]
                public double Direction { get; set; }
            }
                

            [JsonProperty("dt")]
            [JsonConverter(typeof(UnixDateTimeConverter))]
            public DateTime Time { get; set; }

            [JsonProperty("main")]
            public MainDataItem MainData { get; set; }
            [JsonProperty("weather")]
            public WeatherItem[] Weather { get; set; }

            [JsonProperty("wind")]
            public WindItem Wind { get; set; }
            
        }


        [JsonProperty("city")]
        public CityItem City { get; set; }

        [JsonProperty("list")]
        public ListItem[] ForeList { get; set; }
    }

    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
    JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception($"Unexpected token parsing date. Expected Integer, got {reader.TokenType}.");
            }

            var seconds = (long)reader.Value;

            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);//DateTime(1970, 1, 1)

            date = date.AddSeconds(seconds).ToLocalTime();
            return date;
        }

        public override void WriteJson(JsonWriter writer, object value,
    JsonSerializer serializer)
        {
            long ticks;
            if (value is DateTime)
            {
                var epoc = new DateTime(1970, 1, 1);
                var delta = ((DateTime)value) - epoc;
                if (delta.TotalSeconds < 0)
                {
                    throw new ArgumentOutOfRangeException("Unix epoc starts January 1st, 1970");
                }
                ticks = (long)delta.TotalSeconds;
            }
            else
            {
                throw new Exception("Expected date object value.");
            }
            writer.WriteValue(ticks);
        }
    }
}
