using System.Collections.Generic;
using DeribitFutureSubscriber.DbModels;

namespace DeribitFutureSubscriber
{
    public class PostgresDbAccess : IDbAccess<InstrumentTicker>
    {
        private object _lock = new object();
        private DeribitFutureSubscriberDbContext _dbContext;

        public PostgresDbAccess(DeribitFutureSubscriberDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Insert(IList<InstrumentTicker> records)
        {
            lock (_lock)
            {
                _dbContext.InstrumentTikers.AddRange(records);
                _dbContext.SaveChanges();
            }
        }
    }
}
