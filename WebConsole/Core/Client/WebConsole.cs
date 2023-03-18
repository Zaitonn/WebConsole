using Newtonsoft.Json;

namespace Serein.Plugins.WebConsole.Core.Client
{
    internal class Console : Client
    {
        [JsonIgnore]
        public new ClientType Type => ClientType.Console;

        [JsonProperty(PropertyName = "subscribed_target")]
        public string SubscribedTarget;
    }
}