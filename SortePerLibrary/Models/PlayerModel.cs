using System;
using System.Collections.Generic;

namespace SortePerLibrary.Models
{
    public class PlayerModel : IPlayerModel
    {
        public List<ICardModel> Cards { get; set; } = new List<ICardModel>();
        private string _name;

        public PlayerModel(String name)
        {
            _name = name;
        }


        public string Name
        {
            get => _name;
        }
    }
}