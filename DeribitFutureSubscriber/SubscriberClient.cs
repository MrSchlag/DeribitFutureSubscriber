using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DeribitFutureSubscriber.DbModels;
using DeribitFutureSubscriber.RequestActions;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber
{
    public class SubscriberClient
    {
        private readonly IClientWebSocket _clientWebSocket;
        private readonly IDbAccess<InstrumentTicker> _dbAccess;
        private int _requestId = 1;

        List<IRequestAction> _requestActions = new List<IRequestAction>();

        private readonly Timer _dbInsertionTimer;

        public SubscriberClient(IClientWebSocket clientWebSocket, IDbAccess<InstrumentTicker> dbAccess)
        {
            _clientWebSocket = clientWebSocket;
            _dbAccess = dbAccess;

            _dbInsertionTimer = new Timer(Insert, null, TimeSpan.Zero, new TimeSpan(0, 0, 2));
        }

        public void Run()
        {
            _clientWebSocket.Connect(new Uri("wss://test.deribit.com/ws/api/v2")).Wait();
            
            var response = string.Empty;

            var authRequestAction = new AuthenticationRequestAction(_clientWebSocket);

            _requestActions.Add(new SetHeartbeatRequestAction(_clientWebSocket));
            _requestActions.Add(new LoadChannelsRequestAction(_clientWebSocket, _requestActions));

            while (true) //TODO : use cancellation token
            {
                if (!string.IsNullOrEmpty(response))
                {
                    var jobject = JObject.Parse(response);

                    if (jobject.TryGetValue("error", out var jTokenError))
                    {
                        Console.WriteLine("error received");
                    }

                    if (jobject.ContainsKey("result"))
                    {
                        var result = authRequestAction.RequestHandler(jobject).Result;
                        var actionToRemove = new List<IRequestAction>();
                        foreach (var action in _requestActions.ToList())
                        {
                            if (action.RequestHandler(jobject).Result)
                            {
                                actionToRemove.Add(action);
                            }
                        }
                        actionToRemove.ForEach(i => _requestActions.Remove(i));
                    }

                    if (jobject.TryGetValue("params", out var jTokenParams))
                    {
                        HeartbeatNotificationHandler(jobject);
                        TickerNotificationHandler(jobject);
                    }
                }

                _requestId = authRequestAction.Request(_requestId).Result;

                foreach (var action in _requestActions)
                {
                    _requestId = action.Request(_requestId).Result;
                }

                response = _clientWebSocket.Receive().Result;
            }
        }

        /* Heart Beat */
        private HashSet<string> _heartbeatResponseTokens = new() { "type" };
        private void HeartbeatNotificationHandler(JObject jobject)
        {
            var token = jobject["params"];
            if (token.Children<JProperty>().Any() && !token.Children<JProperty>().Any(i => !_heartbeatResponseTokens.Contains(i.Name))) //TODO : create requester
            {
                var type = (string)token["type"];

                if (type == "test_request")
                {
                    _requestActions.Add(new TestHeartbeatRequestAction(_clientWebSocket));
                }
            }
        }

        /* Ticker */
        private object _insturmentTickersToInsertLock = new object();
        private List<InstrumentTicker> _insturmentTickersToInsert = new List<InstrumentTicker>();
        private void TickerNotificationHandler(JObject jobject)
        {
            jobject.TryGetValue("method", out var method);
            method ??= string.Empty;
            if (method.ToString() == "subscription")
            {
                var test = jobject.ToObject<JsonRfcNotification<Ticker>>();
                var instrumentTicker = new InstrumentTicker
                {
                    InstrumentName = test.Params.Data.InstrumentName,
                    SettlementPrice = test.Params.Data.SettlementPrice
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
            List<InstrumentTicker> toInsert;
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
    }
}
