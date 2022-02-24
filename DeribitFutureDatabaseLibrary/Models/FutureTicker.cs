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
        public decimal OpenInterest { get; set; }
    }
}