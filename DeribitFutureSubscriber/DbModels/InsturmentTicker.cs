namespace DeribitFutureSubscriber.DbModels
{
    public class InstrumentTicker
    {
        public int Id { get; set; }

        public string InstrumentName { get; set; }

        public decimal SettlementPrice { get; set; }
    }
}
