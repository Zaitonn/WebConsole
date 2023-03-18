using Newtonsoft.Json;

namespace Serein.Plugins.WebConsole.Core.Packets.DataBody
{
    internal struct Reason
    {
        [JsonProperty(PropertyName = "reason")]
        public string DetailReason;

        public Reason(string reason)
        {
            DetailReason = reason;
        }
    }
}