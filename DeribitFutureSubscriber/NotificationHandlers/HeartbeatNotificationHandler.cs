using System;
using System.Collections.Generic;
using System.Linq;
using DeribitFutureSubscriber.RequestActions;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber.NotificationHandlers
{
    public class HeartbeatNotificationHandler : INotificationHandler
    {
        private readonly List<IRequestAction> _requestActions;
        private readonly IClientWebSocket _clientWebSocket;

        public HeartbeatNotificationHandler(List<IRequestAction> requestAction, IClientWebSocket clientWebSocket)
        {
            _requestActions = requestAction;
            _clientWebSocket = clientWebSocket;
        }
       
        public void NotificationHandler(JObject jobject)
        {
            var token = jobject["params"];
            if (token.Children<JProperty>().Any() && token.Children<JProperty>().Any(i => i.Name == "type"))
            {
                var type = (string)token["type"];

                if (type == "test_request")
                {
                    _requestActions.Add(new TestHeartbeatRequestAction(_clientWebSocket));
                }
            }
        }

        public void Dispose()
        {
        }
    }   
}
