using System.Threading;

namespace DeribitFutureSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbAccess = new FutureTickerAccess(new DeribitFutureSubscriberDbContext());
            var cancelationTokenSource = new CancellationTokenSource();
            var client = new SubscriberClient(new ClientWebSocket(cancelationTokenSource.Token), dbAccess);
            client.Run();
        }
    }
}
