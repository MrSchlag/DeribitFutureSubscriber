using Newtonsoft.Json;

namespace Models.DeribitFutureSubscriber
{
    [JsonObject]
    public class InstrumentData
    {
        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("settlement_price")]
        public decimal SettlementPrice { get; set; }

        [JsonProperty("open_interest")]
        public decimal OpenInterest { get; set; }

        [JsonProperty("min_price")]
        public decimal MinPrice { get; set; }

        [JsonProperty("max_price")]
        public decimal MaxPrice { get; set; }

        [JsonProperty("mark_price")]
        public decimal MarkPrice { get; set; }

        [JsonProperty("last_price")]
        public decimal LastPrice { get; set; }

        [JsonProperty("instrument_name")]
        public string InstrumentName { get; set; }

        [JsonProperty("index_price")]
        public decimal IndexPrice { get; set; }

        [JsonProperty("estimated_delivery_price")]
        public decimal EstimatedDeliveryPrice { get; set; }

        [JsonProperty("best_bid_price")]
        public decimal BestBidPrice { get; set; }

        [JsonProperty("best_bid_amount")]
        public decimal BestBidAmount { get; set; }

        [JsonProperty("best_ask_price")]
        public decimal BestAskPrice { get; set; }

        [JsonProperty("best_ask_amount")]
        public decimal BestAskAmount { get; set; }
    }
}
