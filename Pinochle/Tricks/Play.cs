using System;
using System.Collections.Generic;
using Pinochle.Cards;

namespace Pinochle.Tricks
{
    class Play
    {
        public Player Position;

        public PinochleCard Card;

        public Play(Player player, PinochleCard card)
        {
            Position = player;
            Card = card;
        }
    }
}
