using System;
using System.Collections.Generic;
using Pinochle.Cards;

namespace Pinochle.Tricks
{
    public class Play
    {
        public Seat Position;

        public PinochleCard Card;

        public Play(Seat player, PinochleCard card)
        {
            Position = player;
            Card = card;
        }
    }
}
