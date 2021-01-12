using System;
using System.Collections.Generic;

namespace SortePerLibrary.Models
{
    public class CardDeck : ICardDeck
    {
        public List<ICardModel> Cards { get; set; } = new List<ICardModel>();
    }
}