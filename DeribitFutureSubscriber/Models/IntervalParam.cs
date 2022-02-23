using System;
using Newtonsoft.Json;

namespace DeribitFutureSubscriber.Models
{
    [JsonObject]
    public class IntervalParam
    {
        [JsonProperty("interval")]
        public int Interval { get; set; }
    }
}
