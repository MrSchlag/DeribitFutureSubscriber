using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeribitFutureSubscriber;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json;

namespace DeribitFutureTests
{
    public class ClientWebSocketMock : IClientWebSocket
    {
        public List<string> Sent = new();
        public Queue<string> ReceiveMock = new Queue<string>();

        private bool _isConnected = false;

        public ClientWebSocketMock()
        {
        }

        public async Task Connect(Uri uri)
        {
            _isConnected = true;
        }

        public async Task<string> Receive()
        {
            if (!_isConnected)
                throw new Exception("not connected");

            if (ReceiveMock.Any())
                return ReceiveMock.Dequeue();
            return string.Empty;
        }

        public async Task Send<T>(JsonRfcRequest<T> jsonRfcRequest) where T : new()
        {
            if (!_isConnected)
                throw new Exception("not connected");

            var output = JsonConvert.SerializeObject(jsonRfcRequest);
            Sent.Add(output);

            return;
        }
    }
}
