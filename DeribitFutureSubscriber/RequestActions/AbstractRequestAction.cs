using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeribitFutureSubscriber.Models;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber.RequestActions
{
    public abstract class AbstractRequestAction : IRequestAction
    {
        protected List<int> _requestIdsWaited = new();
        protected IClientWebSocket _clientWebSocket;
        protected bool _waitingResponse = false;

        public AbstractRequestAction(IClientWebSocket clientWebSocket)
        {
            _clientWebSocket = clientWebSocket;
        }

        protected abstract Task<bool> HandlerAction(JObject jObject);

        protected abstract Task<bool> ErrorHandlerAction(JObject jObject);

        protected abstract Task<int> RequestAction(int requestId);

        public async Task<bool> RequestHandler(JObject jObject)
        {
            return await RequestIdMatch(jObject, new Func<Task<bool>>(() => HandlerAction(jObject)));
        }

        public async Task<bool> ErrorRequestHander(JObject jObject)
        {
            return await RequestIdMatch(jObject, new Func<Task<bool>>(() => ErrorHandlerAction(jObject)));
        }

        public async Task<int> Request(int requestId)
        {
            if (!_waitingResponse)
            {
                requestId = await RequestAction(requestId);
                _waitingResponse = true;
            }
            return requestId;
        }

        private async Task<bool> RequestIdMatch(JObject jObject, Func<Task<bool>> funcOnMatch)
        {
            var id = (int)jObject["id"];

            bool result = false;
            if (_requestIdsWaited.Contains(id))
            {
                result = await funcOnMatch();
                _requestIdsWaited.Remove(id);
                _waitingResponse = false;
            }
            return result && !_requestIdsWaited.Any();
        }
    }
}
