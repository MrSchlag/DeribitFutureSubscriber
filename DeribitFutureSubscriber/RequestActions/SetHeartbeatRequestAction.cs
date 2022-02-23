﻿using System.Threading.Tasks;
using DeribitFutureSubscriber.Models;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber.RequestActions
{
    public class SetHeartbeatRequestAction : AbstractRequestAction
    {
        public SetHeartbeatRequestAction(IClientWebSocket clientWebSocket) : base(clientWebSocket)
        {
        }

        protected override async Task<int> RequestAction(int requestId)
        {
            var request = new JsonRfcRequest<IntervalParam>
            {
                Method = "public/set_heartbeat",
                JsonRpc = "2.0",
                Id = requestId,
                Params = new IntervalParam
                {
                    Interval = 10
                }
            };

            _requestIdsWaited.Add(requestId++);
            await _clientWebSocket.Send(request);

            return requestId;
        }

        protected override Task<bool> HandlerAction(JObject jObject)
        {
            return Task.FromResult(true);
        }
    }
}