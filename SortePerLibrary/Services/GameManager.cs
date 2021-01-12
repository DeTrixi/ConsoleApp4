using System;
using System.Collections.Generic;
using SortePerLibrary.Factories;
using SortePerLibrary.Models;

namespace SortePerLibrary.Services
{
    /// <summary>
    /// This class is the primary Game Loop
    /// </summary>
    public class GameManager : IGameManager
    {
        // This is a private list of players
        private readonly List<IPlayerModel> _players;

        // This is a private Validator
        private readonly IValidate _validate;

        // RemoveAnimalCards Remove card from users and invokes a event
        private readonly IRemoveCards _removeCards;

        // the value of the current players
        private int _currentPlayer = 0;

        // the value of the next players
        private int _nextPlayer = 1;

        private Random ran = new Random();

        /// <summary>
        /// This event invokes when current player has finished his turn
        /// </summary>
        public event EventHandler<IPlayerModel> CallNexPlayer;

        public event EventHandler<string> PlayerHasNoMoreCardsAndHasLeftTheGame;


        /// <summary>
        /// Constructor that make insure that the List IPlayerModel , IValidate and IRemoveCards is present
        /// </summary>
        /// <param name="players"></param>
        /// <param name="validate"></param>
        /// <param name="removeCards"></param>
        public GameManager(List<IPlayerModel> players, IValidate validate, IRemoveCards removeCards)
        {
            _players = players;
            _validate = validate;
            _removeCards = removeCards;
            InitializeGame();
        }

        /// <summary>
        /// This method initializes the game and is started from the constructor
        /// </summary>
        private void InitializeGame()
        {
            RemovePairs();
            // NextPlayerToDrawCard();
        }


        /// <summary>
        /// This method start the game and invokes event tht will call out the first player
        /// </summary>
        public void PlayerCallsTheGameFirstTime()
        {
            InvokeNextPlayer();
            // NextPlayerToDrawCard();
        }
        // Start The Game


        /// <summary>
        /// This method will run after all card have changed hands and before game gets started
        /// </summary>
        private void RemovePairs()
        {
            _removeCards.RemoveCardFromPlayers(_players);
        }

        /// <summary>
        /// This method switches the users turn to draw card from the one right to the person
        /// </summary>
        private void NextPlayerToDrawCard()
        {
            if (_nextPlayer + 1 == _players.Count)
            {
                _nextPlayer = 0;
                _currentPlayer = _players.Count - 1;
            }
            else if (_currentPlayer + 1 == _players.Count)
            {
                _currentPlayer = 0;
                _nextPlayer = 1;
            }
            else
            {
                _currentPlayer++;
                _nextPlayer++;
            }
        }

        private void InvokeNextPlayer()
        {
            CallNexPlayer?.Invoke(this, _players[_currentPlayer]);
        }

        /// <summary>
        /// This method Simulates a players turn
        /// </summary>
        public void Play()
        {
            DrawRandomCard();

            RemovePairs();

            RemovePlayerIfFinished();

            if (_validate.ValidateIsLoser(_players))
            {
                return;
            }

            NextPlayerToDrawCard();
            InvokeNextPlayer();
        }

        /// <summary>
        /// Removes Player if player has no cards left finished and resets the user cue
        /// </summary>
        private void RemovePlayerIfFinished()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].Cards.Count == 0)
                {
                    PlayerHasNoMoreCardsAndHasLeftTheGame?.Invoke(this,
                        $"{_players[i].Name} Is Finish and has left the game");
                    _players.Remove(_players[i]);

                    if (_currentPlayer > 0)
                    {
                        _currentPlayer--;
                        _nextPlayer--;
                    }
                    else if (_currentPlayer == 0)
                    {
                        _currentPlayer = _players.Count - 1;
                        _nextPlayer = _currentPlayer - 1;
                    }
                }
            }
        }

        /// <summary>
        /// This method draws a card from the next player
        /// </summary>
        ///
        private void DrawRandomCard()
        {
            // This ran picks a random number (card) from the next player
            int randomNumber = ran.Next(0, _players[_nextPlayer].Cards.Count);
            ICardModel card = _players[_nextPlayer].Cards[randomNumber];

            // Places tha card on the hand of current player
            _players[_currentPlayer].Cards.Add(card);

            // Removes the card from next player
            //_players[_nextPlayer ].Cards.RemoveAll(x => x.Animals == card.Animals);
            _players[_nextPlayer].Cards.Remove(_players[_nextPlayer].Cards[randomNumber]);
        }
    }
}