using System.Collections.Generic;
using Newtonsoft.Json;

namespace Models.DeribitFutureSubscriber
{
    [JsonObject]
    public class ChannelParams
    {
        [JsonProperty("channels")]
        public List<string> Channels { get; set; }
    }
}
