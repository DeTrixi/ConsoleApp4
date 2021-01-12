using System;

namespace SortePerLibrary.Models
{
    public class PlayingCardModel : ICardModel
    {
        public Enum Value { get; set; }
        public Enum Suit { get; set; }
    }
}