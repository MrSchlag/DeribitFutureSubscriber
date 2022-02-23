using System;
using System.Threading.Tasks;
using DeribitFutureSubscriber.Models;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber.RequestActions
{
    public class TestHeartbeatRequestAction : AbstractRequestAction
    {
        public TestHeartbeatRequestAction(IClientWebSocket clientWebSocket) : base(clientWebSocket)
        {
        }

        protected override async Task<int> RequestAction(int requestId)
        {
            var request = new JsonRfcRequest<IntervalParam>
            {
                Method = "public/test",
                JsonRpc = "2.0",
                Id = requestId
            };

            _requestIdsWaited.Add(requestId++);
            await _clientWebSocket.Send(request); //TODO: send in abstract and manage id in abstract

            Console.WriteLine("heartbeat test sent");

            return requestId;
        }

        protected override Task<bool> HandlerAction(JObject jObject)
        {
            Console.WriteLine("heartbeat test response received");
            return Task.FromResult(true);
        }
    }
}
