using Newtonsoft.Json;

namespace Serein.Plugins.WebConsole.Core.Packets.DataBody
{
    internal struct VerifyRequest
    {
        [JsonProperty(PropertyName = "timeout")]
        public int Timeout;

        [JsonProperty(PropertyName = "random_key")]
        public string RandomKey;

        public VerifyRequest(int timeout, string randomKey)
        {
            Timeout = timeout;
            RandomKey = randomKey;
        }
    }
}