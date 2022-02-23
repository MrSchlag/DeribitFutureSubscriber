using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeribitFutureSubscriber.Models;
using DeribitFutureSubscriber.RequestActions;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber
{
    public class SubscriberClient
    {
        private IClientWebSocket _clientWebSocket;

        private int _requestId = 1;

        List<IRequestAction> _requestActions = new List<IRequestAction>();

        public SubscriberClient(IClientWebSocket clientWebSocket)
        {
            _clientWebSocket = clientWebSocket;
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
                        Console.WriteLine("request response");
                    }

                    if (jobject.TryGetValue("params", out var jTokenParams))
                    {
                        HeartbeatNotificationHandler(jTokenParams).Wait();
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
        private async Task HeartbeatNotificationHandler(JToken token)
        {
            if (token.Children<JProperty>().Any() && !token.Children<JProperty>().Any(i => !_heartbeatResponseTokens.Contains(i.Name))) //TODO : create requester
            {
                var type = (string)token["type"];
                if (type == "test_request")
                {
                    var request = new JsonRfcRequest<IntervalParam>
                    {
                        Method = "public/test",
                        JsonRpc = "2.0",
                        Id = _requestId++
                    };

                    Console.WriteLine("heartbeat test sent");
                    await _clientWebSocket.Send(request);
                }
            }
        }

        /* Ticker */
        private void TickerNotificationHandler(JObject jobject)
        {
            jobject.TryGetValue("method", out var method);
            method = method ?? string.Empty;
            if (method.ToString() == "subscription")
            {
                var test = jobject.ToObject<JsonRfcNotification<Ticker>>();
                Console.WriteLine("Received : " + test.Params.Data.InstrumentName);
            }
        }
    }
}
