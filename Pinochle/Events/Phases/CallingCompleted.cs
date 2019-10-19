
using System;

namespace Pinochle.Events.Phases
{
    class CallingCompleted : PhaseCompleted
    {
        public Player Leader;

        public CallingCompleted(Player leader, Cards.PinochleCard.Suits trump) : base(Round.Phases.Dealing, String.Format("{0} called {1} for trump", leader, trump)) {
            Leader = leader;
        }
    }
}
