using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DeribitFutureSubscriber.DbModels;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber.NotificationHandlers
{
    public class SubscribeNotificationHandler : INotificationHandler
    {
        private readonly object _insturmentTickersToInsertLock = new object();
        private readonly List<FutureTicker> _insturmentTickersToInsert = new();

        private readonly IDbAccess<FutureTicker> _dbAccess;
        private readonly Timer _dbInsertionTimer;

        public SubscribeNotificationHandler(IDbAccess<FutureTicker> dbAccess)
        {
            _dbInsertionTimer = new Timer(Insert, null, TimeSpan.Zero, new TimeSpan(0, 0, 2));
            _dbAccess = dbAccess;
        }

        public void NotificationHandler(JObject jobject)
        {
            jobject.TryGetValue("method", out var method);
            method ??= string.Empty;
            if (method.ToString() == "subscription")
            {
                var test = jobject.ToObject<JsonRfcNotification<Ticker>>();
                var instrumentTicker = new FutureTicker
                {
                    Name = test.Params.Data.InstrumentName,
                    Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(test.Params.Data.Timestamp),
                    SettlementPrice = test.Params.Data.SettlementPrice,
                    OpenInterest = test.Params.Data.OpenInterest,
                    BestAskAmount = test.Params.Data.BestAskAmount,
                    BestAskPrice = test.Params.Data.BestAskPrice,
                    BestBidAmount = test.Params.Data.BestBidAmount,
                    BestBidPrice = test.Params.Data.BestBidPrice,
                    EstimatedDeliveryPrice = test.Params.Data.EstimatedDeliveryPrice,
                    IndexPrice = test.Params.Data.IndexPrice,
                    LastPrice = test.Params.Data.LastPrice,
                    MarkPrice = test.Params.Data.MarkPrice,
                    MaxPrice = test.Params.Data.MaxPrice,
                    MinPrice = test.Params.Data.MinPrice,
                    High = test.Params.Data.Stats.High,
                    Low = test.Params.Data.Stats.Low,
                    PriceChange = test.Params.Data.Stats.PriceChange,
                    Volume = test.Params.Data.Stats.Volume,
                    VolumeUsd = test.Params.Data.Stats.VolumeUsd
                };

                lock (_insturmentTickersToInsertLock)
                {
                    _insturmentTickersToInsert.Add(instrumentTicker);
                }

                Console.WriteLine("Received : " + test.Params.Data.InstrumentName);
            }
        }

        private void Insert(object stateInfo)
        {
            if (!_insturmentTickersToInsert.Any())
            {
                return;
            }
            List<FutureTicker> toInsert;
            lock (_insturmentTickersToInsertLock)
            {
                toInsert = _insturmentTickersToInsert.ToList();
            }
            _dbAccess.Insert(toInsert);
            lock (_insturmentTickersToInsertLock)
            {
                _insturmentTickersToInsert.Clear();
            }
        }

        public void Dispose() => _dbInsertionTimer.Dispose();
    }
}
