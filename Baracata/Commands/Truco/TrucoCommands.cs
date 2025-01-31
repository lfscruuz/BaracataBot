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
            var playerReactions = new Dictionary<DiscordMember, DiscordEmoji>();

            await ctx.DeferAsync();

            InteractivityExtension interactivity = Program.Client.GetInteractivity();

            DiscordMember[] players = new[] { player1, player2 }
                .Select(user => (DiscordMember)user)
                .ToArray();

            DiscordEmoji[] emojiOptions = { DiscordEmoji.FromName(Program.Client, ":one:"),
                                     DiscordEmoji.FromName(Program.Client, ":two:"),
                                     DiscordEmoji.FromName(Program.Client, ":three:"),
                                     DiscordEmoji.FromName(Program.Client, ":four:")
            };


            //string optionsDescription = $"{emojiOptions[0]} | carta 1 \n" +
            //                            $"{emojiOptions[1]} | carta 2 \n" +
            //                            $"{emojiOptions[2]} | carta 3 \n" +
            //                            $"{emojiOptions[3]} | carta 4 \n";

            //DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            //{
            //    Color = DiscordColor.Red,
            //    Title = "escolha sua carta",
            //    Description = optionsDescription
            //};

            foreach (var player in players)
            {
                DiscordDmChannel DMChannel = await player.CreateDmChannelAsync();
                DiscordMessage sentMessage = await DMChannel.SendMessageAsync($"hello there {player.Username}");

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
                    Console.WriteLine($"{player.Username} did not respond in time.");
                }
                else
                {
                    playerReactions[player] = reaction.Result.Emoji;
                    Console.WriteLine($"{player.Username} reacted with {reaction.Result.Emoji}");
                }
            }

            var responseContent = string.Join("\n", playerReactions.Select(pr => $"{pr.Key.Username} played: {pr.Value}"));
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(responseContent));

        }
    }
}
