using System;
using System.Collections.Generic;
using System.Linq;
using DeribitFutureSubscriber.DbModels;
using DeribitFutureSubscriber.NotificationHandlers;
using DeribitFutureSubscriber.RequestActions;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber
{
    public class SubscriberClient
    {
        private readonly IClientWebSocket _clientWebSocket;
        private readonly IDbAccess<FutureTicker> _dbAccess;
        private int _requestId = 1;

        List<IRequestAction> _requestActions = new List<IRequestAction>();
        List<INotificationHandler> _notificationHandlers = new List<INotificationHandler>();


        public SubscriberClient(IClientWebSocket clientWebSocket, IDbAccess<FutureTicker> dbAccess)
        {
            _clientWebSocket = clientWebSocket;
            _dbAccess = dbAccess;

        }

        private void SetUpActions()
        {
            _clientWebSocket.Connect(new Uri("wss://test.deribit.com/ws/api/v2")).Wait();

            _requestActions.Add(new SetHeartbeatRequestAction(_clientWebSocket));
            _requestActions.Add(new LoadChannelsRequestAction(_clientWebSocket, _requestActions));

            _notificationHandlers.Add(new HeartbeatNotificationHandler(_requestActions, _clientWebSocket));
            _notificationHandlers.Add(new SubscribeNotificationHandler(_dbAccess));
        }

        public void Run()
        {
            SetUpActions();

            var response = string.Empty;
            var authRequestAction = new AuthenticationRequestAction(_clientWebSocket);

            while (true) //TODO : use cancellation token
            {
                if (!string.IsNullOrEmpty(response))
                {
                    var jobject = JObject.Parse(response);

                    if (jobject.ContainsKey("error"))
                    {
                        var actionToRemove = new List<IRequestAction>();
                        foreach (var action in _requestActions.ToList())
                        {
                            if (action.ErrorRequestHander(jobject).Result)
                            {
                                actionToRemove.Add(action);
                            }
                        }
                        actionToRemove.ForEach(i => _requestActions.Remove(i));
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

                    if (jobject.ContainsKey("params"))
                    {
                        foreach (var handler in _notificationHandlers)
                        {
                            handler.NotifiactionHandler(jobject);
                        }
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
    }
}