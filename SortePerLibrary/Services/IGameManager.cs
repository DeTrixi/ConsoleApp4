using System;
using System.Collections.Generic;
using SortePerLibrary.Models;

namespace SortePerLibrary.Services
{
    public interface IGameManager
    {
        public event EventHandler<IPlayerModel> CallNexPlayer;

        public void Play();

        public void PlayerCallsTheGameFirstTime();

        public event EventHandler<String> PlayerHasNoMoreCardsAndHasLeftTheGame;

    }
}