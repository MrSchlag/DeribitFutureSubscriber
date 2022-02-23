using System.Collections.Generic;
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

        protected abstract Task<int> RequestAction(int requestId);

        public async Task<bool> RequestHandler(JObject jObject)
        {
            var id = (int)jObject["id"];

            bool result = false;
            if (_requestIdsWaited.Contains(id))
            {
                result = await HandlerAction(jObject);
                _waitingResponse = false;
            }
            return result;
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
    }
}
