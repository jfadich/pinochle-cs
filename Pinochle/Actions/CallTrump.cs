using System;
using Pinochle.Cards;
using Pinochle.Contracts;

namespace Pinochle.Actions
{
    public class CallTrump : PlayerAction
    {
        public PinochleCard.Suits Trump { get; }

        public CallTrump(Seat seat, PinochleCard.Suits trump) : base(seat, Phases.Calling)
        {
            Trump = trump;
        }
        public override bool Apply(IGameRound round)
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
