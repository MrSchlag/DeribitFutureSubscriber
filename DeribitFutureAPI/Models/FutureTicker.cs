using System;
namespace DeribitFutureAPI.Models
{
    public class FutureTicker
    {
        public DateTime Timestamp { get; set; }

        public string Name { get; set; }

        public decimal? SettlementPrice { get; set; }

        public decimal? OpenInterest { get; set; }
    }
}
