using System;
using System.Collections.Generic;
using DeribitFutureSubscriber;
using DeribitFutureSubscriber.DbModels;

namespace DeribitFutureTests
{
    public class FutureTickerAccessMock : IDbAccess<FutureTicker>
    {
        public List<FutureTicker> Records = new();

        public IEnumerable<FutureTicker> GetRecords()
        {
            return Records;
        }

        public void Insert(IList<FutureTicker> records)
        {
            Records.AddRange(records);
        }
    }
}
