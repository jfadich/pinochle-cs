using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Events.Turns
{
    class TrumpCalled : PlayerTurn
    {
        public Cards.PinochleCard.Suits Trump { get; protected set; }

        public TrumpCalled(Seat player, Cards.PinochleCard.Suits trump) : base(player, String.Format("{0} called {1} for trump", player, trump))
        {
            Trump = trump;
        }
    }
}
