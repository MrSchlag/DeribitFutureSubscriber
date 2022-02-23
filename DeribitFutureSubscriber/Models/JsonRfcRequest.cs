using Newtonsoft.Json;

namespace Models.DeribitFutureSubscriber
{
    [JsonObject]
    public class JsonRfcRequest<T> where T : new()
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public T Params { get; set; }

        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
