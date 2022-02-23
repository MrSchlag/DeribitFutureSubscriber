using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeribitFutureSubscriber.Models;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var cancelationTokenSource = new CancellationTokenSource();
            var client = new SubscriberClient(new Models.ClientWebSocket(cancelationTokenSource.Token));
            client.Run();
            /*
            var cancelationTokenSource = new CancellationTokenSource();
            ClientWebSocket clientWebSocket = null;
            var task = Task.Run(async () =>
            {
                clientWebSocket = await CreateAndConnect(cancelationTokenSource.Token);
                await HeartbeatSubscribe(clientWebSocket, cancelationTokenSource.Token);
                await TickerSubscribe(clientWebSocket, cancelationTokenSource.Token);
            });
            task.Wait();
            ListenNotifications(clientWebSocket, cancelationTokenSource.Token);

            Console.WriteLine("Hello World!");
            */
        }

        private static async Task<System.Net.WebSockets.ClientWebSocket> CreateAndConnect(CancellationToken token)
        {
            var webSocket = new System.Net.WebSockets.ClientWebSocket();

            await webSocket.ConnectAsync(new Uri("wss://test.deribit.com/ws/api/v2"), token);

            var auth = new JsonRfcRequest<AuthRequestParams>
            {
                Method = "public/auth",
                Params = new AuthRequestParams
                {
                    GrantType = "client_credentials",
                    ClientId = "vrIlT3qo",
                    ClientSecret = "rmhiqP72b2LuLULwzcQ0EnoXRs024zRq6pz1fBIkM8w"
                },
                JsonRpc = "2.0",
                Id = 2
            };

            var output = JsonConvert.SerializeObject(auth);
            var outputAsByte = Encoding.UTF8.GetBytes(output);
            
            await webSocket.SendAsync(new ArraySegment<byte>(outputAsByte), WebSocketMessageType.Text, true, token);

            var response = new ArraySegment<byte>(new byte[1024 * 4]);
            await webSocket.ReceiveAsync(response, token);

            Console.WriteLine(Encoding.UTF8.GetString(response));
            return webSocket;
        }

        private static async Task TickerSubscribe(System.Net.WebSockets.ClientWebSocket clientWebSocket, CancellationToken token)
        {
            var request = new JsonRfcRequest<ChannelParams>
            {
                Method = "private/subscribe",
                JsonRpc = "2.0",
                Id = 4,
                Params = new ChannelParams
                {
                    Channels = new List<string> { "ticker.BTC-24JUN22.100ms" }
                }
            };

            var output = JsonConvert.SerializeObject(request);
            var outputAsByte = Encoding.UTF8.GetBytes(output);

            await clientWebSocket.SendAsync(new ArraySegment<byte>(outputAsByte), WebSocketMessageType.Text, true, token);

            var response = new ArraySegment<byte>(new byte[1024 * 4]);
            await clientWebSocket.ReceiveAsync(response, token);

            Console.WriteLine(Encoding.UTF8.GetString(response));
        }

        private static async Task HeartbeatSubscribe(System.Net.WebSockets.ClientWebSocket clientWebSocket, CancellationToken token)
        {
            var request = new JsonRfcRequest<IntervalParam>
            {
                Method = "public/set_heartbeat",
                JsonRpc = "2.0",
                Id = 4,
                Params = new IntervalParam
                {
                    Interval = 10
                }
            };

            var output = JsonConvert.SerializeObject(request);
            var outputAsByte = Encoding.UTF8.GetBytes(output);

            await clientWebSocket.SendAsync(new ArraySegment<byte>(outputAsByte), WebSocketMessageType.Text, true, token);

            var response = new ArraySegment<byte>(new byte[1024 * 4]);
            await clientWebSocket.ReceiveAsync(response, token);

            Console.WriteLine(Encoding.UTF8.GetString(response));
        }

        private static async Task HeartbeatTest(System.Net.WebSockets.ClientWebSocket clientWebSocket, CancellationToken token)
        {
            Console.WriteLine("Responding...");
            var request = new JsonRfcRequest<IntervalParam>
            {
                Method = "public/test",
                JsonRpc = "2.0",
                Id = 4
            };

            var output = JsonConvert.SerializeObject(request);
            var outputAsByte = Encoding.UTF8.GetBytes(output);

            await clientWebSocket.SendAsync(new ArraySegment<byte>(outputAsByte), WebSocketMessageType.Text, true, token);

            var response = new ArraySegment<byte>(new byte[1024 * 4]);
            await clientWebSocket.ReceiveAsync(response, token);

            Console.WriteLine("response : " + Encoding.UTF8.GetString(response));
        }

        private static void ListenNotifications(System.Net.WebSockets.ClientWebSocket clientWebSocket, CancellationToken token)
        {
            var response = new ArraySegment<byte>(new byte[1024 * 10]);
            while (true)
            {
                WebSocketReceiveResult rcvResult = clientWebSocket.ReceiveAsync(response, token).Result;
                byte[] msgBytes = response.Skip(response.Offset).Take(rcvResult.Count).ToArray();
                string rcvMsg = Encoding.UTF8.GetString(msgBytes);
                //Console.WriteLine("Received: {0}", rcvMsg);

                var jobject = JObject.Parse(rcvMsg);
                jobject.TryGetValue("method", out var method);
                method = method ?? string.Empty;
                if (method.ToString() == "subscription")
                {
                    var test = jobject.ToObject<JsonRfcNotification<Ticker>>();
                    Console.WriteLine("Received : " + test.Params.Data.InstrumentName);
                }
                else if (method.ToString() == "heartbeat")
                {
                    var test = jobject.ToObject<JsonRfcNotification<TypeParam>>();
                    Console.WriteLine("Received heartbeat : " + test.Params.Type);
                    if (test.Params.Type == "test_request")
                        HeartbeatTest(clientWebSocket, token).Wait();
                }
                else
                {
                    Console.WriteLine("unknown");
                }
            }
        }
    }
}
