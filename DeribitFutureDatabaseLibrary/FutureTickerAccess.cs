using System;
using System.Collections.Generic;
using System.Linq;
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
                _dbContext.FutureTickers.AddRange(records);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<FutureTicker> GetRecords()
        {
            return _dbContext.FutureTickers;
        }

        
        
    }
}
