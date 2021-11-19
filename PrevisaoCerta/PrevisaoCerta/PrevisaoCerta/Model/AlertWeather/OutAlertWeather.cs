using Newtonsoft.Json;
using System.Collections.Generic;

namespace PrevisaoCerta.Model.AlertWeather
{
    public class OutAlertWeather
    {
        [JsonProperty("city_name")]
        public string CityName { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("alerts")]
        public List<Data> Data { get; set; }
    }
}
