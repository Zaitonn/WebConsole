using System;
using System.Linq;

namespace Serein.Plugins.WebConsole.Base
{
    internal static class Logger
    {
        public static void Info(string line)
        {
            if (line.Contains('\n'))
            {
                line.Split('\n').ToList().ForEach((singleLine) => Info(singleLine.Trim('\r')));
                return;
            }
            Console.WriteLine($"{DateTime.Now:T} \x1b[96m[Info]\x1b[0m {line}");
        }

        public static void Warn(string line)
        {
            if (line.Contains('\n'))
            {
                line.Split('\n').ToList().ForEach((singleLine) => Warn(singleLine.Trim('\r')));
                return;
            }
            Console.WriteLine($"{DateTime.Now:T} \x1b[33m[Warn] {line}\x1b[0m");
        }

        public static void Error(string line)
        {
            if (line.Contains('\n'))
            {
                line.Split('\n').ToList().ForEach((singleLine) => Error(singleLine.Trim('\r')));
                return;
            }
            Console.WriteLine($"{DateTime.Now:T} \x1b[91m[Error] {line}\x1b[0m");
        }

        public static void Normal(string line)
            => Console.WriteLine(line);

        public static void Recieve(string line)
            => Info($"\x1b[92m[↓]\x1b[0m{line}");

        public static void Send(string line)
            => Info($"\x1b[96m[↑]\x1b[0m{line}");
    }
}
