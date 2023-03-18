using Newtonsoft.Json;

namespace Serein.Plugins.WebConsole.Core.Client
{
    internal class Instance : Client
    {
        [JsonIgnore]
        public Infos Info;

        [JsonIgnore]
        public new ClientType Type => ClientType.Instance;
    }
}