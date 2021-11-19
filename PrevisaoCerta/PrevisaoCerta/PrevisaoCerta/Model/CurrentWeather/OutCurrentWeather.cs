using Newtonsoft.Json;
using System.Collections.Generic;

namespace PrevisaoCerta.Model.CurrentWeather
{
    public class OutCurrentWeather
    {
        [JsonProperty("count")]
        public string Count { get; set; }

        [JsonProperty("data")]
        public List<Data> Data { get; set; }
    }
}
