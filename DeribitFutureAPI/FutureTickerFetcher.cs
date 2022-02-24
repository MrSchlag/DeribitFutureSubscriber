using System;
using System.Collections.Generic;
using System.Linq;
using DeribitFutureSubscriber;
using DeribitFutureSubscriber.DbModels;

namespace DeribitFutureAPI
{
    public class FutureTickerFetcher : IFutureTickerFetcher
    {
        private readonly IDbAccess<FutureTicker> _dbAccess;

        public FutureTickerFetcher(IDbAccess<FutureTicker> dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public IEnumerable<FutureTicker> GetFutureTickersBetween(DateTime startDateIncluded, DateTime endDateExcluded)
        {
            return _dbAccess.GetRecords().Where(i => i.Timestamp >= startDateIncluded && i.Timestamp < endDateExcluded);
        }

        public IEnumerable<FutureTicker> GetFutureTickerBetweenWithName(DateTime startDateIncluded, DateTime endDateExcluded, string name)
        {
            return GetFutureTickersBetween(startDateIncluded, endDateExcluded).Where(i => i.Name == name);
        }

        public IEnumerable<FutureTicker> GetFutureTickersWithName(string name)
        {
            return _dbAccess.GetRecords().Where(i => i.Name == name);
        }
    }
}
