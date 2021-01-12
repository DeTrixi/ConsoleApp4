using System;
using System.Collections.Generic;
using SortePerLibrary.Models;

namespace SortePerLibrary.Services
{
    public class Validate : IValidate
    {
        public event EventHandler<string> LoserHasBeenFound;

        /// <summary>
        /// This method validates on amount of players is between 3 and 7
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ValidateAmountOfPlayers(int amount)
        {
            if (amount < 3 || amount > 7)
            {
                return false;
            }

            return true;
        }


        public bool ValidateIsLoser(List<IPlayerModel> players)
        {
            if (players.Count == 1)
            {
                LoserHasBeenFound?.Invoke(this, $"{players[0].Name} You Lose!");
                return true;
            }

            return false;
        }
    }
}