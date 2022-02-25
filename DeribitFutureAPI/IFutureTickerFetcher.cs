using System;
using System.Collections.Generic;
using DeribitFutureSubscriber.DbModels;

namespace DeribitFutureAPI
{
    public interface IFutureTickerFetcher
    {
        IEnumerable<FutureTicker> GetFutureTickerFilter(string name, DateTime? startDateIncluded, DateTime? endDateExcluded, int? limit);
    }
}