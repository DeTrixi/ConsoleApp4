using System;
using System.Collections.Generic;
using SortePerLibrary.Models;

namespace SortePerLibrary.Services
{
    public class RemovePlayingCards : IRemoveCards
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
        ///
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public IPlayerModel RemoveCardFromDeck(IPlayerModel player)
        {
            int numberInLoop = 0;
            foreach (var card in player.Cards)
            {
                if (card is PlayingCardModel firstCard)
                {
                    var firstValue = firstCard.Value;
                    var firstSuit = firstCard.Suit;


                    for (int i = numberInLoop + 1; i < player.Cards.Count; i++)
                    {
                        if (player.Cards[i] is PlayingCardModel pc)
                        {
                            var secondValue = pc.Value;
                            var secundSuit = pc.Suit;


                            if (Equals(firstValue, secondValue) && Equals(firstSuit, Suits.Hearts) &&
                                Equals(secundSuit, Suits.Diamonds) ||
                                Equals(firstSuit, Suits.Clubs) && Equals(secundSuit, Suits.Spades))
                            {
                                player.Cards.Remove(player.Cards[i]);
                                player.Cards.Remove(player.Cards[numberInLoop]);
                                RemoveCardsFromPlayers?.Invoke(this,
                                    $"{firstValue} is a pair and has been removed from {player.Name}'s card deck\n");
                            }
                        }
                    }
                }

                numberInLoop++;
            }

            return player;
        }
    }
}