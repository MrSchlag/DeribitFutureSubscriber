using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber.RequestActions
{
    public interface IRequestAction
    {
        public Task<int> Request(int requestId);
        public Task<bool> RequestHandler(JObject jObject);
        public Task<bool> ErrorRequestHander(JObject jObject);
    }


}
