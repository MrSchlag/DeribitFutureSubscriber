using System;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber.NotificationHandlers
{
    public interface INotificationHandler : IDisposable
    {
        void NotificationHandler(JObject jobject);
    }
}
