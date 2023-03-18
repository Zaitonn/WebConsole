using Newtonsoft.Json;
using Serein.Plugins.WebConsole.Base;
using Serein.Plugins.WebConsole.Core;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Serein.Plugins.WebConsole
{
    static class Program
    {
        public const string VERSION = "1.3";
        const int STD_INPUT_HANDLE = -10;
        const int STD_OUTPUT_HANDLE = -11;
        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        const uint ENABLE_INSERT_MODE = 0x0020;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        public static Setting Setting;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// <param name="args">启动参数</param>
        [STAThread]
        private static void Main(string[] args)
        {
            Init();
            if (!File.Exists("setting.json"))
            {
                File.WriteAllText("setting.json", JsonConvert.SerializeObject(new Setting(), Formatting.Indented));
                Logger.Warn("配置文件已生成，请修改后重新启动");
                Logger.Normal("\r\n\r\n请按任意键退出...");
                Console.ReadKey(true);
            }
            else
            {
                try
                {
                    Setting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText("setting.json"));
                    if (string.IsNullOrEmpty(Setting.Password))
                    {
                        Logger.Error("密码不可为空");
                        Logger.Normal("\r\n\r\n请按任意键退出...");
                        Console.ReadKey(true);
                        Environment.Exit(-1);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error($"读取文件出现错误: {e}");
                    Logger.Normal("\r\n\r\n请按任意键退出...");
                    Console.ReadKey(true);
                }
            }
            if (args.Length != 0 && args[0] != null)
            {
                Setting.Password = args[0];
            }
            Connections.Start();
            ReadLine();
        }

        private static void Init()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var Handle = GetStdHandle(STD_OUTPUT_HANDLE);
                GetConsoleMode(Handle, out uint OutputMode);
                OutputMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
                SetConsoleMode(Handle, OutputMode);
                Handle = GetStdHandle(STD_INPUT_HANDLE);
                GetConsoleMode(Handle, out uint InputMode);
                InputMode &= ~ENABLE_QUICK_EDIT_MODE;
                InputMode &= ~ENABLE_INSERT_MODE;
                SetConsoleMode(Handle, InputMode);
                IntPtr windowHandle = FindWindow(null, System.Console.Title);
                IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
                uint SC_CLOSE = 0xF060;
                RemoveMenu(closeMenu, SC_CLOSE, 0x0);
                Console.Title = "WebConsole " + VERSION;
            }
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.OutputEncoding = Encoding.UTF8;
        }

        private static void ReadLine()
        {
            while (true)
            {
                if (System.Console.ReadLine() == "list")
                {
                    Logger.Info($"当前有{Connections.Consoles.Count}个控制台和{Connections.Instances.Count}个面板在线");
                    Connections.Consoles.Keys.ToList().ForEach((key) =>
                    {
                        Logger.Info($"控制台\t{key,-18}\t{Connections.Consoles[key].CustomName}");
                    });
                    Connections.Instances.Keys.ToList().ForEach((key) =>
                    {
                        Logger.Info($"面板\t{key,-18}\t{Connections.Instances[key].CustomName}");
                    });
                    Logger.Info("");
                }
                else
                {
                    Logger.Warn("未知的命令");
                }
            }
        }
    }
}