using System;
using System.Collections.Generic;

namespace SortePerLibrary.Models
{
    public interface IPlayerModel
    {
        string Name { get; }
        List<ICardModel> Cards { get; }
    }
}