using Newtonsoft.Json;

namespace Models.DeribitFutureSubscriber
{
    [JsonObject]
    public class Ticker
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("data")]
        public InstrumentData Data { get; set; }


    }
}
