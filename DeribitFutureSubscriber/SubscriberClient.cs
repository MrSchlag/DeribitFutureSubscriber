using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DeribitFutureSubscriber.DbModels;
using DeribitFutureSubscriber.NotificationHandlers;
using DeribitFutureSubscriber.RequestActions;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber
{
    public class SubscriberClient : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IClientWebSocket _clientWebSocket;
        private readonly IDbAccess<FutureTicker> _dbAccess;
        private readonly List<IRequestAction> _requestActions = new();
        private readonly List<INotificationHandler> _notificationHandlers = new();

        private int _requestId = 1;

        public SubscriberClient(IClientWebSocket clientWebSocket,
            IDbAccess<FutureTicker> dbAccess,
            CancellationTokenSource cancellattionTokenSource)
        {
            _cancellationTokenSource = cancellattionTokenSource;
            _clientWebSocket = clientWebSocket;
            _dbAccess = dbAccess;
        }

        private void SetUpActions()
        {
            _clientWebSocket.Connect(new Uri("wss://test.deribit.com/ws/api/v2")).Wait();

            _requestActions.Add(new SetHeartbeatRequestAction(_clientWebSocket));
            _requestActions.Add(new LoadChannelsRequestAction(_clientWebSocket, _requestActions, _cancellationTokenSource));

            _notificationHandlers.Add(new HeartbeatNotificationHandler(_requestActions, _clientWebSocket));
            _notificationHandlers.Add(new SubscribeNotificationHandler(_dbAccess));
        }

        public void Run()
        {
            SetUpActions();

            var response = string.Empty;
            var authRequestAction = new AuthenticationRequestAction(_clientWebSocket, _cancellationTokenSource);

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (!string.IsNullOrEmpty(response))
                {
                    var jobject = JObject.Parse(response);
                    ErrorHandling(authRequestAction, jobject);
                    ResponseHandling(authRequestAction, jobject);
                    NotificationHandling(jobject);
                }

                RequestActions(authRequestAction);

                response = _clientWebSocket.Receive().Result;
            }
        }

        private void RequestActions(AuthenticationRequestAction authRequestAction)
        {
            _requestId = authRequestAction.Request(_requestId).Result;
            foreach (var action in _requestActions)
            {
                _requestId = action.Request(_requestId).Result;
            }
        }

        private void NotificationHandling(JObject jobject)
        {
            if (jobject.ContainsKey("params"))
            {
                foreach (var handler in _notificationHandlers)
                {
                    handler.NotificationHandler(jobject);
                }
            }
        }

        private void ResponseHandling(AuthenticationRequestAction authRequestAction, JObject jobject)
        {
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
        }

        private void ErrorHandling(AuthenticationRequestAction authRequestAction, JObject jobject)
        {
            if (jobject.ContainsKey("error"))
            {
                var result = authRequestAction.ErrorRequestHander(jobject).Result;
                var actionToRemove = new List<IRequestAction>();
                foreach (var action in _requestActions.ToList())
                {
                    result = action.ErrorRequestHander(jobject).Result;
                    actionToRemove.Add(action);
                }
                actionToRemove.ForEach(i => _requestActions.Remove(i));
            }
        }

        public void Dispose() => _notificationHandlers.ForEach(i => i.Dispose());
    }
}