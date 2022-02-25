using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeribitFutureSubscriber;
using NUnit.Framework;

namespace DeribitFutureTests
{
    [TestFixture]
    public class SubscriberClientTests
    {
        private SubscriberClient _subscriber;
        private CancellationTokenSource _cancelationTokenSource;
        private ClientWebSocketMock _clientWebSocket;
        private FutureTickerAccessMock _futureDbAccess;

        [SetUp]
        public void Setup()
        {
            _cancelationTokenSource = new CancellationTokenSource();
            _clientWebSocket = new ClientWebSocketMock();
            _futureDbAccess = new FutureTickerAccessMock();
            _subscriber = new SubscriberClient(_clientWebSocket, _futureDbAccess, _cancelationTokenSource);
        }

        [Test]
        public void CheckSendInitTest()
        {
            Task.Run(() => _subscriber.Run());
            Thread.Sleep(new TimeSpan(0, 0, 0, 0, 100));
            _cancelationTokenSource.Cancel();
            Assert.That(_clientWebSocket.Sent.Count, Is.EqualTo(6));
            Assert.That(_clientWebSocket.Sent[0].Contains("auth"), Is.True);
            Assert.That(_clientWebSocket.Sent[1].Contains("set_heartbeat"), Is.True);
        }

        [Test]
        public void AuthErrorTest()
        {
            _clientWebSocket.ReceiveMock.Enqueue("{\"jsonrpc\": \"2.0\",\"id\": 1,\"error\": {\"message\": \"invalid_credentials\",\"code\": 13004},\"usIn\": 1645800142612746,\"usOut\": 1645800142612808,\"usDiff\": 62,\"testnet\": true}");
            _subscriber.Run();
            Assert.Pass("Self cancel on fatal error");
        }

        [Test]
        public void SaveTickerTest()
        {
            string subscriptionMessage = @"{
  'jsonrpc': '2.0',
  'method': 'subscription','params': {
    'channel': 'ticker.BTC-11MAR22.100ms',
    'data': {
      'timestamp': 1645801269204,
      'stats': {
        'volume_usd': 337980,
        'volume': 8.66058664,
        'price_change': 1.2862,
        'low': 38320.77,
        'high': 39523
      },
      'state': 'open',
      'open_interest': 229650,
      'min_price': 38369.5,
      'max_price': 39538.5,
      'mark_price': 38925.01,
      'last_price': 38836,
      'instrument_name': 'BTC-11MAR22',
      'index_price': 38916.98,
      'estimated_delivery_price': 38916.98,
      'best_bid_price': 38888.5,
      'best_bid_amount': 77770,
      'best_ask_price': 38945.5,
      'best_ask_amount': 1670
    }
  }}".Replace("'", "\"");
            
            _clientWebSocket.ReceiveMock.Enqueue(subscriptionMessage);
            _clientWebSocket.ReceiveMock.Enqueue("{\"jsonrpc\": \"2.0\",\"id\": 1,\"error\": {\"message\": \"invalid_credentials\",\"code\": 13004},\"usIn\": 1645800142612746,\"usOut\": 1645800142612808,\"usDiff\": 62,\"testnet\": true}");
            _subscriber.Run();
            Thread.Sleep(new TimeSpan(0, 0, 3)); //Wait for thread to insert
            Assert.That(_futureDbAccess.Records.First().Name, Is.EqualTo("BTC-11MAR22"));
            Assert.That(_futureDbAccess.Records.First().PriceChange, Is.EqualTo(1.2862));
        }

    }
}
