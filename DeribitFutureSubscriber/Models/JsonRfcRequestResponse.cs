using Newtonsoft.Json;

namespace Models.DeribitFutureSubscriber
{
    [JsonObject]
    public class JsonRfcRequestResponse<T> where T : new()
    {
        [JsonProperty("jsonrpc")]
        public string jsonRpc { get; set; }

        [JsonProperty("result")]
        public T Result { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
