using Newtonsoft.Json;
using System;

namespace Serein.Plugins.WebConsole.Core.Packets
{
    internal sealed class SentPacket : Packet
    {
        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore, Order = 1)]
        public readonly object Data;

        [JsonProperty(PropertyName = "time")]
        public long Time;

        private static readonly Sender SelfSender = new($"webconsole_{Program.VERSION}", "host", null);

        public SentPacket(string type, string sub_type, object data = null)
        {
            Type = type;
            SubType = sub_type;
            Data = data;
            Sender = SelfSender;
            Time = (long)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public SentPacket(string type, string sub_type, object data, Sender sender)
        {
            Type = type;
            SubType = sub_type;
            Data = data;
            Sender = sender;
            Time = (long)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}