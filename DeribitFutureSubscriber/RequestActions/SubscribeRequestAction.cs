﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DeribitFutureSubscriber.Models;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber.RequestActions
{
    public class SubscribeRequestAction : AbstractRequestAction
    {
        private readonly List<string> _channels;

        public SubscribeRequestAction(IClientWebSocket clientWebSocket, List<string> channels) : base(clientWebSocket)
        {
            _channels = channels;
        }

        protected override async Task<int> RequestAction(int requestId)
        {
            var request = new JsonRfcRequest<ChannelParams>
            {
                Method = "private/subscribe",
                JsonRpc = "2.0",
                Id = requestId,
                Params = new ChannelParams
                {
                    Channels = _channels
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
