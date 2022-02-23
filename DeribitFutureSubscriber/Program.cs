using System.Threading;

namespace DeribitFutureSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var cancelationTokenSource = new CancellationTokenSource();
            var client = new SubscriberClient(new Models.ClientWebSocket(cancelationTokenSource.Token));
            client.Run();
        }
    }
}
