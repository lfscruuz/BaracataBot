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
using System.Numerics;
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
            var hands = new Dictionary<DiscordMember, Dictionary<DiscordEmoji, CardStructure>>();

            DiscordEmoji[] emojiOptions = { DiscordEmoji.FromName(Program.Client, ":one:"),
                                     DiscordEmoji.FromName(Program.Client, ":two:"),
                                     DiscordEmoji.FromName(Program.Client, ":three:"),
                                     DiscordEmoji.FromName(Program.Client, ":four:")
            };
            InteractivityExtension interactivity = Program.Client.GetInteractivity();

            await ctx.DeferAsync();


            DiscordMember[] players = new[] { player1, player2 }
                .Select(user => (DiscordMember)user)
                .ToArray();

            foreach (var player in players)
            {

                hands[player] = service.AssignHands(emojiOptions, cards);

                DiscordDmChannel DMChannel = await player.CreateDmChannelAsync();
                DiscordMessage sentMessage = await DMChannel.SendMessageAsync($"Olá, {player.Username}");

                foreach (var emoji in emojiOptions)
                {
                    await sentMessage.CreateReactionAsync(emoji);
                }

                playerMessages[player] = sentMessage;
            }

            foreach (var kvp in hands)
            {
                var player = kvp.Key;
                var dict = kvp.Value;

                foreach (var kvp2 in dict)
                {
                    var emoji = kvp2.Key;
                    var card = kvp2.Value;
                    Console.WriteLine($"{player.Username} was assigned the card {card.name}");
                }
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
                    try
                    {
                        var randomvariable = hands[player][reaction.Result.Emoji].name;
                        Console.WriteLine(randomvariable); // Use WriteLine instead of Write for better visibility
                        playerReactions[player] = randomvariable;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
                    }
                }
            }
            service.AnalyzeCards(playerReactions);

            var responseContent = string.Join("\n", playerReactions.Select(pr => $"{pr.Key.Username} jogou a carta: {pr.Value}"));
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(responseContent));
        }
    }
}
