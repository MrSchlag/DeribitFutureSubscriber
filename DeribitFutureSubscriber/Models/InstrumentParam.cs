using Newtonsoft.Json;

namespace Models.DeribitFutureSubscriber
{
    [JsonObject]
    public class InstrumentParam
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("expired")]
        public bool Expired { get; set; }
    }
}

