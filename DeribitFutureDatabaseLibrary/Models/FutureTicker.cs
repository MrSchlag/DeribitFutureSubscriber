using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeribitFutureSubscriber.DbModels
{
    public class FutureTicker
    {
        [Key]
        [Column("id")]
        public int Id { get; set; } 

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("settlement_price")]
        public decimal SettlementPrice { get; set; }

        [Column("open_interest")]
        public decimal? OpenInterest { get; set; }

        [Column("min_price")]
        public decimal? MinPrice { get; set; }

        [Column("max_price")]
        public decimal? MaxPrice { get; set; }

        [Column("mark_price")]
        public decimal? MarkPrice { get; set; }

        [Column("last_price")]
        public decimal? LastPrice { get; set; }

        [Column("index_price")]
        public decimal? IndexPrice { get; set; }

        [Column("estimated_delivery_price")]
        public decimal? EstimatedDeliveryPrice { get; set; }

        [Column("best_bid_price")]
        public decimal? BestBidPrice { get; set; }

        [Column("best_bid_amount")]
        public decimal? BestBidAmount { get; set; }

        [Column("best_ask_price")]
        public decimal? BestAskPrice { get; set; }

        [Column("best_ask_amount")]
        public decimal? BestAskAmount { get; set; }

        [Column("volume_usd")]
        public decimal? VolumeUsd { get; set; }

        [Column("volume")]
        public decimal? Volume { get; set; }

        [Column("price_change")]
        public decimal? PriceChange { get; set; }

        [Column("low")]
        public decimal? Low { get; set; }

        [Column("high")]
        public decimal? High { get; set; }
    }
}