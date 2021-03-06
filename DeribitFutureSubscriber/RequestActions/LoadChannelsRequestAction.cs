using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber.RequestActions
{
    public class LoadChannelsRequestAction : AbstractRequestAction
    {
        private bool _isChannelsLoaded = false;
        private readonly List<IRequestAction> _requestActions;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly List<string> _currencies = new() { "BTC", "ETH", "USDC", "SOL" }; //TODO : get via /public/get_currency

        public LoadChannelsRequestAction(IClientWebSocket clientWebSocket, List<IRequestAction> requestActions, CancellationTokenSource cancellationTokenSource) : base(clientWebSocket)
        {
            _requestActions = requestActions;
            _cancellationTokenSource = cancellationTokenSource;
        }

        protected override async Task<int> RequestAction(int requestId)
        {
            if (!_isChannelsLoaded)
            {
                foreach (var currency in _currencies)
                {
                    var request = new JsonRfcRequest<InstrumentParam>
                    {
                        Method = Methods.GetInstruments,
                        JsonRpc = "2.0",
                        Id = requestId,
                        Params = new InstrumentParam
                        {
                            Kind = "future",
                            Currency = currency,
                            Expired = false
                        }
                    };
                    _requestIdsWaited.Add(requestId++);
                    await _clientWebSocket.Send(request);
                }
            }
            return requestId;
        }

        protected override Task<bool> HandlerAction(JObject jObject)
        {
            var channels = jObject["result"].Children()
                                    .Select(token => token.ToObject<InstrumentData>())
                                    .Select(instrument => "ticker." + instrument.InstrumentName + ".100ms")
                                    .ToList();


            _requestActions.Add(new SubscribeRequestAction(_clientWebSocket, channels));
            _isChannelsLoaded = true;

            return Task.FromResult(true);
        }

        protected override Task<bool> ErrorHandlerAction(JObject jObject)
        {
            Console.Error.WriteLine("LoadChannelRequestAction error fatal");
            _cancellationTokenSource.Cancel();
            return Task.FromResult(true);
        }
    }
}
