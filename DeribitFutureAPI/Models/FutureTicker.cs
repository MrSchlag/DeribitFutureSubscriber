using System;
namespace DeribitFutureAPI.Models
{
    public class FutureTicker
    {
        public DateTime Timestamp { get; set; }

        public string Name { get; set; }

        public decimal SettlementPrice { get; set; }

        public decimal? OpenInterest { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public decimal? MarkPrice { get; set; }

        public decimal? LastPrice { get; set; }

        public decimal? IndexPrice { get; set; }

        public decimal? EstimatedDeliveryPrice { get; set; }

        public decimal? BestBidPrice { get; set; }

        public decimal? BestBidAmount { get; set; }

        public decimal? BestAskPrice { get; set; }

        public decimal? BestAskAmount { get; set; }

        public decimal? VolumeUsd { get; set; }

        public decimal? Volume { get; set; }

        public decimal? PriceChange { get; set; }

        public decimal? Low { get; set; }

        public decimal? High { get; set; }
    }
}
