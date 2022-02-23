using System;
using Newtonsoft.Json;

namespace DeribitFutureSubscriber.Models
{
    [JsonObject]
    public class TypeParam
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
