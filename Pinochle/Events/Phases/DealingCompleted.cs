
using System;

namespace Pinochle.Events.Phases
{
    class DealingCompleted : PhaseCompleted
    {
        public Player Dealer;

        public DealingCompleted(Player dealer) : base(Round.Phases.Dealing, String.Format("{0} Dealt", dealer)) {
            Dealer = dealer;
        }
    }
}
