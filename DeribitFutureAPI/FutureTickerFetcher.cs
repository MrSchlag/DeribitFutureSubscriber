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

        public IEnumerable<FutureTicker> GetFutureTickerFilter(string name,
            DateTime? startDateIncluded,
            DateTime? endDateExcluded,
            int? limit)
        {
            var result = _dbAccess.GetRecords();

            if (!string.IsNullOrEmpty(name))
                result = result.Where(i => i.Name == name);
            if (startDateIncluded != null)
                result = result.Where(i => i.Timestamp >= startDateIncluded);
            if (endDateExcluded != null)
                result = result.Where(i => i.Timestamp < endDateExcluded);
            if (limit != null)
                result = result.Take(limit.Value);

            return result;
        }
    }
}
