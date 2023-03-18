using Fleck;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Serein.Plugins.WebConsole.Core.Client
{
    internal class Client
    {
        [JsonIgnore]
        public IWebSocketConnection WebSocketConnection;

        [JsonProperty(PropertyName = "address")]
        public string Address => WebSocketConnection?.GetFullUrl();

        [JsonIgnore]
        public DateTime LastTime;

        [JsonProperty(PropertyName = "custom_name")]
        public string CustomName;

        [JsonProperty(PropertyName = "guid")]
        public string GUID;

        [JsonIgnore]
        public ClientType Type => ClientType.Unknown;

        public void Close() => WebSocketConnection?.Close();

        public Task Send(string text) => WebSocketConnection?.Send(text);

        internal enum ClientType
        {
            Unknown,
            Instance,
            Console
        }
    }
}
