using System.Linq;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Plugins.WebConsole.Base;
using Serein.Plugins.WebConsole.Core.Client;
using Serein.Plugins.WebConsole.Core.Packets;
using Serein.Plugins.WebConsole.Core.Packets.DataBody;
using Serein.Plugins.WebConsole.Core.Service;
using Sys = System;
using System.Collections.Generic;
using System.Timers;

namespace Serein.Plugins.WebConsole.Core
{
    internal static class Connections
    {
        private static WebSocketServer Server;

        public static readonly Dictionary<string, string> Clients = new();

        public static readonly Dictionary<string, Console> Consoles = new();

        public static readonly Dictionary<string, Instance> Instances = new();

        private static readonly Timer HeartbeatTimer = new(10000)
        {
            AutoReset = true
        };

        /// <summary>
        /// 启动
        /// </summary>
        public static void Start()
        {
            FleckLog.LogAction = (level, message, e) =>
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        // Console.WriteLine($"\x1b[95m[Debug]\x1b[0m{Message} {e}");
                        break;
                    case LogLevel.Info:
                        Logger.Info($"{message} {e}");
                        break;
                    case LogLevel.Warn:
                        Logger.Warn($"{message} {e}");
                        break;
                    case LogLevel.Error:
                        Logger.Error($"{message} {e}");
                        break;
                    default:
                        throw new Sys.NotSupportedException();
                }
            };
            Server = new(Program.Setting.Addr)
            {
                RestartAfterListenError = true
            };
            Server.Start(socket =>
            {
                socket.OnOpen = () => OnOpen(socket);
                socket.OnClose = () => OnClose(socket);
                socket.OnMessage = (message) => OnReceive(socket, message);
            });
            HeartbeatTimer.Elapsed += Heartbeat;
            HeartbeatTimer.Start();
        }

        /// <summary>
        /// 发送心跳
        /// </summary>
        private static void Heartbeat(object sender, ElapsedEventArgs e)
        {
            lock (Instances)
            {
                Instances.Values.ToList().ForEach((instance) => instance?.Send(new SentPacket("action", "heartbeat").ToString()).Await());
            }
        }

        /// <summary>
        /// 连接处理
        /// </summary>
        /// <param name="client">客户端</param>
        private static void OnOpen(IWebSocketConnection client)
        {
            if (client is null)
            {
                return;
            }
            string clientUrl = client.GetFullUrl();
            string guid = Sys.Guid.NewGuid().ToString("N").Substring(0, 10);
            Clients.Add(clientUrl, Utils.GetMD5(guid + Program.Setting.Password));
            System.Console.Title = $"WebConsole - 连接数：{Clients.Count}";
            Logger.Info($"<{clientUrl}> 尝试连接，预期MD5值：{Utils.GetMD5(guid + Program.Setting.Password)}");
            client.Send(new SentPacket("action", "verify_request", new VerifyRequest(5000, guid)).ToString()).Await();
            Timer verifyTimer = new(5000) { AutoReset = false, };
            verifyTimer.Start();
            verifyTimer.Elapsed += (_, _) =>
            {
                if (!Consoles.ContainsKey(clientUrl) && !Instances.ContainsKey(clientUrl))
                {
                    verifyTimer.Stop();
                    client.Send(new SentPacket("event", "disconnection", new Reason("验证超时")).ToString()).Await();
                    client.Close();
                }
                verifyTimer.Dispose();
            };
        }

        /// <summary>
        /// 关闭处理
        /// </summary>
        /// <param name="client">客户端</param>
        private static void OnClose(IWebSocketConnection client)
        {
            if (client is null)
            {
                return;
            }
            string clientUrl = client.GetFullUrl();
            Logger.Info($"<{clientUrl}> 断开了连接");
            Clients.Remove(clientUrl);
            Instances.Remove(clientUrl);
            Consoles.Remove(clientUrl);
            System.Console.Title = $"WebConsole - 连接数：{Clients.Count}";
        }

        /// <summary>
        /// 接收处理
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="message">接收信息</param>
        private static void OnReceive(IWebSocketConnection client, string message)
        {
            System.Console.Title = $"WebConsole - 连接数：{Clients.Count}";
            if (client is null)
            {
                return;
            }
            string clientUrl = client.GetFullUrl();
            bool isConsole = Consoles.TryGetValue(clientUrl, out Console console) && console is not null,
                 isInstance = Instances.TryGetValue(clientUrl, out Instance instance) && instance is not null;
            ReceivedPacket packet = null;
            try
            {
                packet = JsonConvert.DeserializeObject<ReceivedPacket>(message);
            }
            catch (Sys.Exception e)
            {
                Logger.Warn($"<{clientUrl}>发送的数据包存在问题：{e}");
                client.Send(new SentPacket("event", (isConsole || isInstance) ? "invalid_packet" : "disconnection", new Reason($"发送的数据包存在问题：{e.Message}")).ToString()).Await();
                if (!isConsole && !isInstance)
                {
                    client.Close();
                }
                return;
            }

            if (!isConsole && !isInstance) // 对未记录的的客户端进行校验
            {
                if (packet.Type != "action" ||
                    packet.SubType != "verify")
                {
                    client.Send(new SentPacket("event", "disconnection", new Reason("你还未通过验证")).ToString()).Await();
                    client.Close();
                    return;
                }
                if (!Verify(client, packet.Data))
                {
                    client.Send(new SentPacket("event", "disconnection", new Reason("验证失败，请稍后重试")).ToString()).Await();
                    client.Close();
                }
            }
            else if (isConsole)
            {
                Handle(console, packet);
            }
            else
            {
                Handle(instance, packet);
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="data">验证主体</param>
        /// <returns>验证结果</returns>
        private static bool Verify(IWebSocketConnection client, JToken data)
        {
            if (data is null || !Clients.TryGetValue(client.GetFullUrl(), out string md5) || string.IsNullOrEmpty(md5))
            {
                client.Send(new SentPacket("event", "verify_result", new Result(false, "数据异常")).ToString()).Await();
                Logger.Info($"<{client.GetFullUrl()}> 验证失败：数据异常");
                return false;
            }
            VerifyBody verifyBody = data.ToObject<VerifyBody>() ?? throw new Sys.ArgumentNullException(nameof(data));
            if (verifyBody.MD5 != md5)
            {
                client.Send(new SentPacket("event", "verify_result", new Result(false, "MD5校验失败")).ToString()).Await();
                Logger.Info($"<{client.GetFullUrl()}> 验证失败：MD5校验失败");
                return false;
            }
            if (verifyBody.ClientType?.ToLowerInvariant() == "instance")
            {
                Instances.Add(client.GetFullUrl(), new()
                {
                    WebSocketConnection = client,
                    CustomName = verifyBody.CustomName ?? "Unknown",
                });
                client.Send(new SentPacket("event", "verify_result", new Result(true, null)).ToString()).Await();
                Logger.Info($"<{client.GetFullUrl()}> 验证成功（实例），自定义名称为：{verifyBody.CustomName ?? "Unknown"}");
                return true;
            }
            if (verifyBody.ClientType?.ToLowerInvariant() == "console")
            {
                Consoles.Add(client.GetFullUrl(), new()
                {
                    WebSocketConnection = client,
                    CustomName = verifyBody.CustomName ?? "Unknown",
                });
                client.Send(new SentPacket("event", "verify_result", new Result(true, null)).ToString()).Await();
                Logger.Info($"<{client.GetFullUrl()}> 验证成功（控制台），自定义名称为：{verifyBody.CustomName ?? "Unknown"}");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 处理数据包（控制台）
        /// </summary>
        /// <param name="console">控制台客户端</param>
        /// <param name="packet">数据包</param>
        private static void Handle(Console console, ReceivedPacket packet)
        {
            switch (packet.Type)
            {
                case "action":
                    Actions.Handle(console, packet);
                    break;
                default:
                    console.Send(new SentPacket("event", "invalid_param", new Reason($"所请求的“{packet.Type}”类型不存在或无法调用")).ToString());
                    break;
            }
        }

        /// <summary>
        /// 处理数据包（实例）
        /// </summary>
        /// <param name="console">实例客户端</param>
        /// <param name="packet">数据包</param>
        private static void Handle(Instance instance, ReceivedPacket packet)
        {
            instance.LastTime = Sys.DateTime.Now;
            switch (packet.Type)
            {
                case "event":
                    Events.Handle(instance, packet);
                    break;
                default:
                    instance.Send(new SentPacket("event", "invalid_param", new Reason($"所请求的“{packet.Type}”类型不存在或无法调用")).ToString());
                    break;
            }
        }
    }
}
