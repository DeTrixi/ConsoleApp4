using System;
using System.Collections.Generic;
using SortePerLibrary.Models;

namespace SortePerLibrary.Services
{
    public interface IValidate
    {
        event EventHandler<String> LoserHasBeenFound;
        bool ValidateAmountOfPlayers(int amount);

        bool ValidateIsLoser(List<IPlayerModel> players);

    }
}