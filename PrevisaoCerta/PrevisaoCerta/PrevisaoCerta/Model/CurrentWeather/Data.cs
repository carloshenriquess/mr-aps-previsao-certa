using Newtonsoft.Json;

namespace PrevisaoCerta.Model.CurrentWeather
{
    public class Data
    {
        [JsonProperty("city_name")]
        public string CityName { get; set; }

        [JsonProperty("state_code")]
        public string StateCode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("station")]
        public string Station { get; set; }

        [JsonProperty("vis")]
        public int Vis { get; set; }

        [JsonProperty("rh")]
        public double Rh { get; set; }

        [JsonProperty("dewpt")]
        public double Dewpt { get; set; }

        [JsonProperty("wind_dir")]
        public int WindDir { get; set; }

        [JsonProperty("wind_cdir")]
        public string WindCdir { get; set; }

        [JsonProperty("wind_cdir_full")]
        public string WindCdirFull { get; set; }

        [JsonProperty("wind_speed")]
        public double WindSpeed { get; set; }

        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("app_temp")]
        public double AppTemp { get; set; }

        [JsonProperty("clouds")]
        public int Clouds { get; set; }

        [JsonProperty("weather")]
        public WeatherCondition WeatherCondition { get; set; }

        [JsonProperty("datetime")]
        public string Datetime { get; set; }

        [JsonProperty("ob_time")]
        public string ObTime { get; set; }

        [JsonProperty("ts")]
        public int Ts { get; set; }

        [JsonProperty("sunrise")]
        public string Sunrise { get; set; }

        [JsonProperty("sunset")]
        public string Sunset { get; set; }

        [JsonProperty("slp")]
        public double Slp { get; set; }

        [JsonProperty("pres")]
        public double Pres { get; set; }

        [JsonProperty("aqi")]
        public int Aqi { get; set; }

        [JsonProperty("uv")]
        public double Uv { get; set; }

        [JsonProperty("solar_rad")]
        public double SolarRad { get; set; }

        [JsonProperty("ghi")]
        public double Ghi { get; set; }

        [JsonProperty("dni")]
        public double Dni { get; set; }

        [JsonProperty("dhi")]
        public double Dhi { get; set; }

        [JsonProperty("elev_angle")]
        public double ElevAngle { get; set; }

        [JsonProperty("hour_angle")]
        public double HourAngle { get; set; }

        [JsonProperty("pod")]
        public string Pod { get; set; }

        [JsonProperty("precip")]
        public int Precip { get; set; }

        [JsonProperty("snow")]
        public int Snow { get; set; }
    }
}
