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
    [Route("futureticker")]
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
        public IEnumerable<FutureTicker> Get(string name, DateTime? startDateIncluded, DateTime? endDateExcluded, int? limit)
        {
            var fetcher = new FutureTickerFetcher(_futureTickerAccess);
            var result = fetcher.GetFutureTickerFilter(name, startDateIncluded, endDateExcluded, limit).ToList();

            return result.Select(i => new FutureTicker
            {
                Name = i.Name,
                OpenInterest = i.OpenInterest,
                SettlementPrice = i.SettlementPrice,
                Timestamp = i.Timestamp,
                BestAskAmount = i.BestAskAmount,
                BestAskPrice = i.BestAskPrice,
                BestBidAmount = i.BestBidAmount,
                BestBidPrice = i.BestBidPrice,
                EstimatedDeliveryPrice = i.EstimatedDeliveryPrice,
                High = i.High,
                IndexPrice = i.IndexPrice,
                LastPrice = i.LastPrice,
                Low = i.Low,
                MarkPrice = i.MarkPrice,
                MaxPrice = i.MaxPrice,
                MinPrice = i.MinPrice,
                PriceChange = i.PriceChange,
                Volume = i.Volume,
                VolumeUsd = i.VolumeUsd
            });
        }
    }
}
