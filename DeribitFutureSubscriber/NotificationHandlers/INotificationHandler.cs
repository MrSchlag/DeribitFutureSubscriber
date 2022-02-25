using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber.NotificationHandlers
{
    public interface INotificationHandler
    {
        void NotifiactionHandler(JObject jobject);
    }
}
