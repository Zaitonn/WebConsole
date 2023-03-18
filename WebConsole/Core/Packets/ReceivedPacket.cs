using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Serein.Plugins.WebConsole.Core.Packets
{
    internal sealed class ReceivedPacket : Packet
    {
        [JsonProperty(PropertyName = "data")]
        public JToken Data;
    }
}