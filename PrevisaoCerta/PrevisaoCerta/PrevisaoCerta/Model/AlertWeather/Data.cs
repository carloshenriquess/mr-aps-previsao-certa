using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PrevisaoCerta.Model.AlertWeather
{
    public class Data
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }

        [JsonProperty("effective_utc")]
        public DateTime EffectiveUtc { get; set; }

        [JsonProperty("effective_local")]
        public DateTime EffectiveLocal { get; set; }

        [JsonProperty("expires_utc")]
        public DateTime ExpiresUtc { get; set; }

        [JsonProperty("expires_local")]
        public DateTime ExpiresLocal { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("alerts")]
        public List<string> Alerts { get; set; }
    }
}
