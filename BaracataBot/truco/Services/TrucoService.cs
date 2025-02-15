using Baracata.Truco.Models;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
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
        public async Task<List<CardStructure>> GetCards()
        {
            var cardsObject = new List<CardStructure>();
            using (var sr = new StreamReader("../../../truco/assets/cardListing.json"))
            {
                string json = await sr.ReadToEndAsync();
                List<CardStructure> cardsJson = JsonConvert.DeserializeObject<List<CardStructure>>(json);

                foreach (var data in cardsJson)
                {
                    var newCard = new CardStructure()
                    {
                        id = Guid.NewGuid(),
                        name = data.name,
                        description = data.description,
                        value = data.value,
                        image = data.image
                    };
                    cardsObject.Add(newCard);
                }
            }

            return cardsObject;
        }

        public Dictionary<DiscordEmoji, CardStructure> AssignHands(DiscordEmoji[] emojiOptions, List<CardStructure> cards)
        {
            var hands = new Dictionary<DiscordMember, Dictionary<DiscordEmoji, CardStructure>>();
            var hand = new Dictionary<DiscordEmoji, CardStructure>();

            foreach (var (emoji, i) in emojiOptions.Select((value, i) => (value, i)))
            {
                var rnd = new Random();
                int r =  rnd.Next(cards.Count);
                hand[emoji] = cards[r];
            }

            return hand;
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
