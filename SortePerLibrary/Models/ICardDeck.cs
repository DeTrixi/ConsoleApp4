using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace SortePerLibrary.Models
{
    public interface ICardDeck
    {
        List<ICardModel> Cards { get; set; }
    }
}