using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SortePerLibrary.Models;

namespace SortePerLibrary.Services
{
    /// <summary>
    /// This method forwards single users to remove from hand
    /// </summary>
    public class RemoveAnimalCards : IRemoveCards
    {
        public event EventHandler<string> RemoveCardsFromPlayers;

        /// <summary>
        /// This method sends player to get removed pairs
        /// </summary>
        /// <param name="players"></param>
        /// <returns></returns>
        public List<IPlayerModel> RemoveCardFromPlayers(List<IPlayerModel> players)
        {
            List<IPlayerModel> newPlayerList = new List<IPlayerModel>();
            foreach (var player in players)
            {
                newPlayerList.Add(RemoveCardFromDeck(player));
            }

            return newPlayerList;
        }

        /// <summary>
        /// This method removes card from the stack
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public IPlayerModel RemoveCardFromDeck(IPlayerModel player)
        {
            Enum tempCardValue;


            for (int i = 0; i < player.Cards.Count; i++)

            {
                tempCardValue = player.Cards[i].Value;
                for (int j = i + 1; j < player.Cards.Count; j++)
                {
                    if (Equals(player.Cards[i].Value, player.Cards[j].Value))
                    {
                        player.Cards.Remove(player.Cards[j]);
                        player.Cards.Remove(player.Cards[i]);
                        RemoveCardsFromPlayers?.Invoke(this, $"{tempCardValue} is a pair and has been removed from {player.Name}'s card deck\n");
                    }
                }
            }

            return player;
        }
    }
}