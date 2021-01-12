using System;
using System.Collections.Generic;
using SortePerLibrary.Models;

namespace SortePerLibrary.Services
{
    public interface IRemoveCards
    {
        public event EventHandler<string> RemoveCardsFromPlayers;
        public List<IPlayerModel> RemoveCardFromPlayers(List<IPlayerModel> players);
        public IPlayerModel RemoveCardFromDeck(IPlayerModel player);
    }
}