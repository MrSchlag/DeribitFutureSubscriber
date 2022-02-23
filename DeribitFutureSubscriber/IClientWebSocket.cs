using System;
using System.Threading.Tasks;
using Models.DeribitFutureSubscriber;

namespace DeribitFutureSubscriber.Models
{
    public interface IClientWebSocket
    {
        Task Connect(Uri uri);

        Task Send<T>(JsonRfcRequest<T> jsonRfcRequest) where T : new();

        Task<string> Receive();
    }
}
