using Newtonsoft.Json;

namespace Serein.Plugins.WebConsole.Core.Packets.DataBody
{
    internal sealed class VerifyBody
    {
        [JsonProperty(PropertyName = "md5")]
        public string MD5;

        [JsonProperty(PropertyName = "custom_name")]
        public string CustomName;

        [JsonProperty(PropertyName = "client_type")]
        public string ClientType;
    }
}