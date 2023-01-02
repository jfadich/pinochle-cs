using System;
using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Engine.Contracts;

namespace JFadich.Pinochle.Engine.Actions
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
