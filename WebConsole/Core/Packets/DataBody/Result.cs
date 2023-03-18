using Newtonsoft.Json;

namespace Serein.Plugins.WebConsole.Core.Packets.DataBody
{
    internal struct Result
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success;

        [JsonProperty(PropertyName = "reason")]
        public string Reason;

        public Result(bool success, string reason)
        {
            Success = success;
            Reason = reason;
        }
    }
}