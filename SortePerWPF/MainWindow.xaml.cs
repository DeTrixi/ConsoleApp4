using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SortePerLibrary.Factories;
using SortePerLibrary.Models;
using SortePerLibrary.Services;

namespace SortePerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Players
        private List<IPlayerModel> _players;
        private ICardDeck _animalCardDeck;
        private IValidate _validate;
        private IRemoveCards _removeCards;
        private IGameManager _game;
        private bool _isGameStarted = false;

        public MainWindow()
        {
            InitializeComponent();
            InitGame();
            UpdateNameInputFields();
        }

        private void InitGame()
        {
            // gets a validate to later validate cards
            _validate = GameFactory.CreateValidator();
            // gets a instance of remove cards
            _removeCards = GameFactory.RemoveCards();


            // Create all the event listener
            // this listens to if cards has been removed
            _removeCards.RemoveCardsFromPlayers += RemoveCardsOnRemoveCardsFromPlayers; // TODO update ui

            // Event if invoked loser will be displayed to Console
            _validate.LoserHasBeenFound += ValidateOnLoserHasBeenFound;
        }

        // TODO decide if this shall remain
        // Event triggers if a pair of card is removed from a user
        private void RemoveCardsOnRemoveCardsFromPlayers(object sender, string e)
        {
            //MessageBox.Show($"{e}");
        }


        // Event i raised if there are found a looser
        private void ValidateOnLoserHasBeenFound(object sender, string e)
        {
            // TODO Open up a winner page
            // Update headline to You lose!
            HeadLineText.Content = $"{e}";
            DisplayCards.Children.Clear();
            Image cardImage = new Image();

            cardImage.MinWidth = 120;
            cardImage.Source =
                new BitmapImage(new Uri($@"/Resources/Images/AnimalPictures/cat.png",
                    UriKind.RelativeOrAbsolute));
            DisplayCards.Children.Add(cardImage);
            Mainwindow.Width = 500;
            // Hides the play button
            Play.Visibility = Visibility.Hidden;

        }


        /// <summary>
        /// This event updates hov many input Fields there are for the player to type there names
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoseAmountOfPlayersComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            UpdateNameInputFields();
        }


        /// <summary>
        /// This method updates hov many input Fields there are for the player to type there names
        /// </summary>
        private void UpdateNameInputFields()
        {
            int numberOfPlayers = int.Parse(ChoseAmountOfPlayersComboBox.Text);
            PlayersInput.Children.Clear();


            for (int i = 0; i < numberOfPlayers; i++)
            {
                TextBox name = new TextBox();
                name.MaxHeight = 30;
                name.Name = $"player{i}";
                name.Text = $"Player {i + 1}";
                PlayersInput.Children.Add(name);
            }
        }

        /// <summary>
        /// This method starts the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (_isGameStarted == true)
            {
                _game.Play();
            }
            else
            {
                // Generate the list with users
                _players = GameFactory.CreateUsers(GetUsernames());
                // Create a deck of cards
                _animalCardDeck = GameFactory.CreateAnimalCardDeck();
                // Shuffle tha card in no order
                _animalCardDeck = GameFactory.ShuffleCards(_animalCardDeck);
                // Deals the card uot to all the players
                _players = GameFactory.DealCards(_players, _animalCardDeck);
                // Creates the game
                _game = GameFactory.CreateGameLogic(_players, _validate, _removeCards);

                // Hides all the controls
                HideCurrentControls();


                // subscribes to event next player to play the game
                _game.CallNexPlayer += Game_CallNexPlayer;

                _game.PlayerHasNoMoreCardsAndHasLeftTheGame += GameOnPlayerHasNoMoreCardsAndHasLeftTheGame;

                _game.PlayerCallsTheGameFirstTime();
                // Update Headline text.
                // Create New view.
                _isGameStarted = true;
                // Sets a new text on the play button
                Play.Content = "Draw a card";
            }
        }


        /// <summary>
        /// this event is invoked if a player has no more cards an is out of the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameOnPlayerHasNoMoreCardsAndHasLeftTheGame(object sender, string e)
        {
            MessageBox.Show(e);
        }

        /// <summary>
        /// Displays the cards on the current players hand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game_CallNexPlayer(object sender, IPlayerModel e)
        {
            // subscribes to event next player to play the game.
            HeadLineText.Content = $"{e.Name}:  Your turn to draw a card from the player to the right";

            ShowCurrentPlayersCards(e);
        }

        /// <summary>
        /// Displays the currents players cards to the screen.
        /// </summary>
        /// <param name="player"></param>
        private void ShowCurrentPlayersCards(IPlayerModel player)
        {
            // Removes cards from the previous player if there is one
            DisplayCards.Children.Clear();

            foreach (var card in player.Cards)
            {
                Image cardImage = new Image();
                //name.MaxHeight = 30;
                cardImage.MaxWidth = 85;
                string path = Directory.GetCurrentDirectory();

                cardImage.Source =
                    new BitmapImage(new Uri($@"/Resources/Images/AnimalPictures/{card.Value.ToString()}.png",
                        UriKind.RelativeOrAbsolute));
                DisplayCards.Children.Add(cardImage);
            }
        }


        /// <summary>
        /// Gets the usernames from the User input StackPanel
        /// </summary>
        /// <returns></returns>
        private List<string> GetUsernames()
        {
            List<string> result = new List<string>();
            foreach (var player in PlayersInput.Children)
            {
                if (player is TextBox t)
                {
                    result.Add(t.Text);
                }
            }

            return result;
        }

        /// <summary>
        /// Hides the startup controls displayed
        /// </summary>
        private void HideCurrentControls()
        {
            SortePer.Visibility = Visibility.Hidden;
            PlayersInput.Visibility = Visibility.Hidden;
            ChoseAmountOfPlayersComboBox.Visibility = Visibility.Hidden;
        }
    }
}