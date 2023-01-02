using System;
using System.Collections.Generic;
using JFadich.Pinochle.Engine.Cards;

namespace JFadich.Pinochle.Engine.Tricks
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
