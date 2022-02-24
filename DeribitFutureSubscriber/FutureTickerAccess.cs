using System.Collections.Generic;
using DeribitFutureSubscriber.DbModels;

namespace DeribitFutureSubscriber
{
    public class FutureTickerAccess : IDbAccess<FutureTicker>
    {
        private object _lock = new object();
        private DeribitFutureSubscriberDbContext _dbContext;

        public FutureTickerAccess(DeribitFutureSubscriberDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
            _dbContext.SaveChanges();
        }

        public void Insert(IList<FutureTicker> records)
        {
            lock (_lock)
            {
                //_dbContext.InstrumentTikers.AddRange(records);
                _dbContext.FutureTickers.AddRange(records);
                _dbContext.SaveChanges();
            }
        }
    }
}
