
using System;
using Pinochle.Cards;

namespace Pinochle.Events.CompletedPhases
{
    public class CallingCompleted : PhaseCompleted
    {
        public Seat Leader;

        PinochleCard.Suits Trump;

        public CallingCompleted(Seat leader, PinochleCard.Suits trump) : base(Phases.Calling, Phases.Passing) {
            Leader = leader;
            Trump = trump;
        }

        public override string ToString()
        {
            return String.Format("{0} called {1} for trump", Leader, Trump);
        }
    }
}
