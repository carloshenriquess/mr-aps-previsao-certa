using Newtonsoft.Json;

namespace PrevisaoCerta.Model.CurrentWeather
{
    public class WeatherCondition
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
