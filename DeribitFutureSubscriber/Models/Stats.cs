using Newtonsoft.Json;

namespace Models.DeribitFutureSubscriber
{
    [JsonObject]
    public class Stats
    {
        [JsonProperty("volume_usd")]
        public decimal? VolumeUsd { get; set; }

        [JsonProperty("volume")]
        public decimal? Volume { get; set; }

        [JsonProperty("price_change")]
        public decimal? PriceChange { get; set; }

        [JsonProperty("low")]
        public decimal? Low { get; set; }

        [JsonProperty("high")]
        public decimal? High { get; set; }
    }
}
