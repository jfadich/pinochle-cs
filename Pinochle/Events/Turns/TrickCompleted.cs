using System;
using System.Collections.Generic;
using Pinochle.Cards;
using Pinochle.Tricks;

namespace Pinochle.Events.Turns
{
    class TrickCompleted : PlayerTurn
    {
        public TrickCompleted(Player winner) : base(winner, String.Format("{0} wins the trick", winner)) {
        }
    }
}
