using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using SortePerLibrary.Models;
using SortePerLibrary.Services;

namespace SortePerLibrary.Factories
{
    public class GameFactory
    {
        /// <summary>
        /// Private constructor THE CAN BE ONLY ONE
        /// </summary>
        private GameFactory()
        {
        }

        // /// <summary>
        // /// This Method creates a IGameManager Instance
        // /// </summary>
        // /// <returns>Returns a IGameManager instance </returns>
        public static IGameManager CreateGameLogic(List<IPlayerModel> players, IValidate validate, IRemoveCards removeCards)
        {
            return new GameManager(players, validate, removeCards);
        }


        /// <summary>
        /// This method Create a user
        /// </summary>
        /// <returns> Returns a list of user </returns>
        public static List<IPlayerModel> CreateUsers(List<string> names)
        {
            List<IPlayerModel> players = new List<IPlayerModel>();
            foreach (var name in names)
            {
                players.Add(new PlayerModel(name));
            }

            return players;
        }

        /// <summary>
        /// This method creates class for validating values
        /// </summary>
        /// <returns>A IValidate</returns>
        public static IValidate CreateValidator()
        {
            return new Validate();
        }


        /// <summary>
        /// This method create the card deck
        /// </summary>
        /// <returns>Returns a complete card deck</returns>
        public static ICardDeck CreateAnimalCardDeck()
        {
            ICardDeck cardDeck = new CardDeck();
            List<ICardModel> cards = new List<ICardModel>();
            foreach (Animals value in Enum.GetValues(typeof(Animals)))
            {
                // Ads two cards to the card deck
                if (value == Animals.Cat)
                {
                    cards.Add(new AnimalCardModel {Value = value});
                }
                else
                {
                    cards.Add(new AnimalCardModel {Value = value});
                    cards.Add(new AnimalCardModel {Value = value});
                }
            }

            cardDeck.Cards = cards;

            return cardDeck;
        }

        public static ICardDeck CreatePlayingCardDeck()
        {
            ICardDeck cardDeck = new CardDeck();
            List<ICardModel> cards = new List<ICardModel>();

            foreach (Suits suit in Enum.GetValues(typeof(Suits)))
            {
                foreach (Ranks rank in Enum.GetValues(typeof(Ranks)))
                {
                    if (rank == Ranks.Joker)
                    {
                    }
                    else
                    {
                        cards.Add(new PlayingCardModel {Value = rank, Suit = suit});
                    }
                }

            }
            cards.Add(new PlayingCardModel() {Value = Ranks.Joker});
            cardDeck.Cards = cards;
            return cardDeck;
        }

        /// <summary>
        /// This method shuffles the card deck
        /// </summary>
        /// <param name="cardDeck">Takes in a ICardDeck</param>
        /// <returns>Returns a shuffled card deck</returns>
        public static ICardDeck ShuffleCards(ICardDeck cardDeck)
        {
            var shuffled = cardDeck.Cards.OrderBy(x => Guid.NewGuid()).ToList();
            return new CardDeck {Cards = shuffled};
        }

        /// <summary>
        /// This Message Deals the card to all the players
        /// </summary>
        /// <param name="players">Takes a list of IPlayer</param>
        /// <param name="cardDeck">Takes the card deck</param>
        /// <returns>Returns List of IPlayerModel whit there card hands</returns>
        public static List<IPlayerModel> DealCards(List<IPlayerModel> players, ICardDeck cardDeck)
        {
            do
            {
                foreach (var player in players)
                {
                    // Breaks the loop if last card have been given out
                    if (cardDeck.Cards.Count == 0)
                    {
                        break;
                    }

                    player.Cards.Add(cardDeck.Cards[0]);
                    cardDeck.Cards.Remove(cardDeck.Cards[0]);
                }
            } while (cardDeck.Cards.Count >= 1);

            return players;
        }

        /// <summary>
        /// Creates an instance of IRemoveCards
        /// </summary>
        /// <returns></returns>
        public static IRemoveCards RemoveCards()
        {
            return new RemoveAnimalCards();
        }
    }
}