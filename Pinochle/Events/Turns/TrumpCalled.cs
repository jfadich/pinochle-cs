using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Events.Turns
{
    class TrumpCalled : PlayerTurn
    {
        public Cards.PinochleCard.Suits Trump { get; protected set; }

        public TrumpCalled(Player player, Cards.PinochleCard.Suits trump) : base(player)
        {
            Trump = trump;
        }
        public override string ToString()
        {
            return String.Format("{0} called {1} for trump", Player, Trump);
        }
    }
}
