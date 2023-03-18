using Newtonsoft.Json;

namespace Serein.Plugins.WebConsole.Core.Packets
{
    internal struct Sender
    {
        [JsonProperty(PropertyName = "name")]
        public string Name;

        [JsonProperty(PropertyName = "type")]
        public string Type;

        [JsonProperty(PropertyName = "address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address;

        public Sender(string name, string type, string address)
        {
            Name = name;
            Type = type;
            Address = address;
        }
    }
}