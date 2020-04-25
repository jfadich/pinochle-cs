using System;
using System.Collections.Generic;
using Pinochle.Cards;
using Pinochle.Tricks;

namespace Pinochle.Events.Turns
{
    class TrickPlayed : PlayerTurn
    {
        public Card Play;
        public TrickPlayed(Seat player, Card play) : base(player, String.Format("{0} played {1}", player, play)) {
            Play = play;
        }
    }
}
