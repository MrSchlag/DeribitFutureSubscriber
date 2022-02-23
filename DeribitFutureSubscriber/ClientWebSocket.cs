using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json;

namespace DeribitFutureSubscriber.Models
{
    public class ClientWebSocket : IClientWebSocket
    {
        private System.Net.WebSockets.ClientWebSocket _clientWebSocket;
        private CancellationToken _cancellationToken;

        public ClientWebSocket(CancellationToken cancellationToken)
        {
            _clientWebSocket = new System.Net.WebSockets.ClientWebSocket();
            _cancellationToken = cancellationToken;
        }

        public async Task Connect(Uri uri)
        {
            await _clientWebSocket.ConnectAsync(uri, _cancellationToken);
        }

        public async Task<string> Receive()
        {
            var response = new ArraySegment<byte>(new byte[1024 * 10]);//TODO: buffer size management

            var receiveResult = await _clientWebSocket.ReceiveAsync(response, _cancellationToken);
            var msgBytes = response.Skip(response.Offset).Take(receiveResult.Count).ToArray();
            var receivedMessage = Encoding.UTF8.GetString(msgBytes);

            return receivedMessage;
        }

        public async Task Send<T>(JsonRfcRequest<T> jsonRfcRequest) where T : new()
        {
            var output = JsonConvert.SerializeObject(jsonRfcRequest);
            var outputAsByte = Encoding.UTF8.GetBytes(output);

            await _clientWebSocket.SendAsync(new ArraySegment<byte>(outputAsByte), WebSocketMessageType.Text, true, _cancellationToken);
        }
    }
}
