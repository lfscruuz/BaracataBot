using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;

namespace Baracata.Commands.Truco
{
    public class TrucoCommands : ApplicationCommandModule
    {
        [SlashCommand("truco", "iniciar um jogo de truco")]
        public async Task InitializeTrucoCommand(InteractionContext ctx, [Option("player1","primeiro jogador")] DiscordUser player1, [Option("player2", "segundo jogador")] DiscordUser player2, [Option("player3", "terceiro jogador")] DiscordUser player3, [Option("player4", "quarto jogador")] DiscordUser player4) 
        { 
            await ctx.DeferAsync();
            var players = new[] { (DiscordMember)player1, (DiscordMember)player2, (DiscordMember)player3, (DiscordMember)player4 };

            foreach (DiscordMember player in players)
            {
                var DMChannel = await player.CreateDmChannelAsync();
                await DMChannel.SendMessageAsync($"hello there {player.Username}");
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"{player1}, {player2}, {player3}, {player4}"));

        }
    }
}
