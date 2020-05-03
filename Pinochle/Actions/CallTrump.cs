using System;
using System.Collections.Generic;
using System.Text;
using Pinochle.Cards;

namespace Pinochle.Actions
{
    class CallTrump : PlayerAction
    {
        public PinochleCard.Suits Trump { get; }

        public CallTrump(Seat seat, PinochleCard.Suits trump) : base(seat, Round.Phases.Calling)
        {
            Trump = trump;
        }
        public override bool Apply(Round round)
        {
            round.CallTrump(Seat, Trump);

            return true;
        }

        public override string ToString()
        {
            return String.Format("{0} called {1} for trump", Seat, Trump);
        }
    }
}
