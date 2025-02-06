using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;

namespace Baracata.Truco.Services
{
    internal class TrucoService
    {
        private InteractionContext context;
        public TrucoService(InteractionContext ctx)
        {
            context = ctx;
        }
        public void AnalyzeCards(Dictionary<DiscordMember, string> playerReactions)
        {
            
            foreach (var kvp in playerReactions)
            {
                Console.WriteLine($"{kvp.Key} jogou {kvp.Value}");
            }
        }
    }
}
