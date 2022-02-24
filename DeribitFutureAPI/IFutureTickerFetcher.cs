using System;
using System.Collections.Generic;
using DeribitFutureSubscriber.DbModels;

namespace DeribitFutureAPI
{
    public interface IFutureTickerFetcher
    {
        IEnumerable<FutureTicker> GetFutureTickerBetweenWithName(DateTime startDateIncluded, DateTime endDateExcluded, string name);
        IEnumerable<FutureTicker> GetFutureTickersBetween(DateTime startDateIncluded, DateTime endDateExcluded);
        IEnumerable<FutureTicker> GetFutureTickersWithName(string name);
    }
}