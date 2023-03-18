using Newtonsoft.Json;

namespace Serein.Plugins.WebConsole.Core.Packets
{
    internal abstract class Packet
    {
        [JsonProperty(PropertyName = "type")]
        public string Type = string.Empty;

        [JsonProperty(PropertyName = "sub_type")]
        public string SubType = string.Empty;

        [JsonProperty(PropertyName = "sender", NullValueHandling = NullValueHandling.Ignore)]
        public object Sender;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
