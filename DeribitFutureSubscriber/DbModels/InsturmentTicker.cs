namespace DeribitFutureSubscriber.DbModels
{
    public class InsturmentTicker
    {
        public int Id { get; set; }

        public string InstrumentName { get; set; }

        public decimal SettlementPrice { get; set; }
    }
}
