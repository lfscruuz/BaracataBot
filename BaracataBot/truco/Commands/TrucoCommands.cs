using Baracata.Truco.Models;
using Baracata.Truco.Services;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Baracata.Commands.Truco
{
    public class TrucoCommands : ApplicationCommandModule
    {
        [SlashCommand("truco", "iniciar um jogo de truco")]
        public async Task InitializeTrucoCommand(InteractionContext ctx, [Option("player1", "primeiro jogador")] DiscordUser player1, [Option("player2", "segundo jogador")] DiscordUser player2)
        {
            var playTime = TimeSpan.FromSeconds(20);
            var playerMessages = new Dictionary<DiscordMember, DiscordMessage>();
            var playerReactions = new Dictionary<DiscordMember, string>();
            var service = new TrucoService(ctx);
            var cards = await service.GetCards();

            DiscordEmoji[] emojiOptions = { DiscordEmoji.FromName(Program.Client, ":one:"),
                                     DiscordEmoji.FromName(Program.Client, ":two:"),
                                     DiscordEmoji.FromName(Program.Client, ":three:"),
                                     DiscordEmoji.FromName(Program.Client, ":four:")
            };
            InteractivityExtension interactivity = Program.Client.GetInteractivity();

            await ctx.DeferAsync();

            //Only making one hand, maybe make into a list of dictionaries?
            var hands = service.AssignHands(emojiOptions, cards);

            DiscordMember[] players = new[] { player1, player2 }
                .Select(user => (DiscordMember)user)
                .ToArray();

            foreach (var hand in hands)
            {
                Console.WriteLine(hand.Value.name);
            }

            foreach (var player in players)
            {
                DiscordDmChannel DMChannel = await player.CreateDmChannelAsync();
                DiscordMessage sentMessage = await DMChannel.SendMessageAsync($"Olá, {player.Username}");

                foreach (var emoji in emojiOptions)
                {
                    await sentMessage.CreateReactionAsync(emoji);
                }

                playerMessages[player] = sentMessage;
            }


            foreach (var kvp in playerMessages)
            {
                var player = kvp.Key;
                var message = kvp.Value;

                InteractivityResult<MessageReactionAddEventArgs> reaction = await interactivity.WaitForReactionAsync(
                    (reactionEventArgs) =>
                    {
                        return emojiOptions.Contains(reactionEventArgs.Emoji) && reactionEventArgs.User == player;
                    },
                    message,
                    player,
                    playTime
                );

                if (reaction.TimedOut)
                {
                    Console.WriteLine($"{player.Username} não respondeu a tempo.");
                }
                else
                {
                    playerReactions[player] = hands[reaction.Result.Emoji].name;
                    service.AnalyzeCards(playerReactions);
                }
            }

            var responseContent = string.Join("\n", playerReactions.Select(pr => $"{pr.Key.Username} jogou a carta: {pr.Value}"));
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(responseContent));
        }
    }
}
