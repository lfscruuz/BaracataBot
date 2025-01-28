using DotNetEnv;

using System;

namespace Baracata
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Env.Load("E:\\Users\\lfscr\\source\\repos\\Baracata\\Baracata\\.env");
            Console.WriteLine("Environment variables loaded.");
            string botToken = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
            Console.WriteLine($"Token: {botToken ?? "No token found"}");
        }
    }
}
