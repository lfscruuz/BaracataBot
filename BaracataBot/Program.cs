using Baracata.Commands.Truco;
using Baracata.Truco.Models;
using BaracataBot;
using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

namespace Baracata
{
    internal class Program
    {
        public static DiscordClient Client { get; set; }
        public static Settings Settings { get; set; }
        static async Task Main(string[] args)
        {
            Settings = new Configuration().Settings;
            var botToken = Settings.DiscordBotToken;

            DiscordConfiguration discordConfiguration = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = botToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };
            Client = new DiscordClient(discordConfiguration);

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            Client.Ready += Client_Ready;

            SlashCommandsExtension slashCommandsConfig = Client.UseSlashCommands();
            slashCommandsConfig.RegisterCommands<TrucoCommands>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
