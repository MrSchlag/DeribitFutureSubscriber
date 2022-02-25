using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json;

namespace DeribitFutureSubscriber
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

            try
            {
                var receiveResult = await _clientWebSocket.ReceiveAsync(response, _cancellationToken);
                var msgBytes = response.Skip(response.Offset).Take(receiveResult.Count).ToArray();
                var receivedMessage = Encoding.UTF8.GetString(msgBytes);

                return receivedMessage;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("received async cancelled");
                return null;
            }
        }

        public async Task Send<T>(JsonRfcRequest<T> jsonRfcRequest) where T : new()
        {
            var output = JsonConvert.SerializeObject(jsonRfcRequest);
            var outputAsByte = Encoding.UTF8.GetBytes(output);
            try
            {
                await _clientWebSocket.SendAsync(new ArraySegment<byte>(outputAsByte), WebSocketMessageType.Text, true, _cancellationToken);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("send async cancelled");
            }
        }
    }
}
