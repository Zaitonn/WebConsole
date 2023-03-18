using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Serein.Plugins.WebConsole.Core.Client;
using Serein.Plugins.WebConsole.Core.Packets;

namespace Serein.Plugins.WebConsole.Core
{
    internal static class Utils
    {
        /// <summary>
        /// 获取MD5
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>MD5文本</returns>
        public static string GetMD5(string text)
        {
            byte[] targetData = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(text));
            string result = string.Empty;
            for (int i = 0; i < targetData.Length; i++)
            {
                result += targetData[i].ToString("x2");
            }
            return result;
        }

        public static string GetFullUrl(this IWebSocketConnection client)
            => $"{client.ConnectionInfo.ClientIpAddress}:{client.ConnectionInfo.ClientPort}";

        public static Console AsConsole(this IWebSocketConnection client)
        {
            Connections.Consoles.TryGetValue(client.GetFullUrl(), out Console webConsole);
            return webConsole;
        }

        public static Instance AsInstance(this IWebSocketConnection client)
        {
            Connections.Instances.TryGetValue(client.GetFullUrl(), out Instance instance);
            return instance;
        }

        public static Sender AsSender(this Client.Client client)
            => new Sender()
            {
                Name = client.CustomName,
                Type = client.Type.ToString().ToLowerInvariant(),
                Address = client.Address
            };


        public static void Await(this Task task) => task.GetAwaiter().GetResult();
    }
}