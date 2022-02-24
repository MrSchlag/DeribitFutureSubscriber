using System;
using System.Collections.Generic;
using System.Linq;
using DeribitFutureAPI.Models;
using DeribitFutureSubscriber;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DeribitFutureAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FutureTickerController : ControllerBase
    {
        private readonly ILogger<FutureTickerController> _logger;
        private readonly FutureTickerAccess _futureTickerAccess;

        public FutureTickerController(ILogger<FutureTickerController> logger)
        {
            var dbContext = new DeribitFutureSubscriberDbContext();
            _futureTickerAccess = new FutureTickerAccess(dbContext);
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<FutureTicker> Get(string name, DateTime? startIncluded, DateTime? endExcluded, int? limit = 100)
        {
            var fetcher = new FutureTickerFetcher(_futureTickerAccess);

            var result = fetcher.GetFutureTickersWithName(name).ToList();

            return result.Select(i => new FutureTicker
            {
                Name = i.Name,
                OpenInterest = i.OpenInterest,
                SettlementPrice = i.SettlementPrice,
                Timestamp = i.Timestamp
            });
        }
    }
}
