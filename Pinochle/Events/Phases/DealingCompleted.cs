
using System;

namespace Pinochle.Events.Phases
{
    class DealingCompleted : PhaseCompleted
    {
        public Seat Dealer;

        public DealingCompleted(Seat dealer) : base(Round.Phases.Dealing, String.Format("{0} Dealt", dealer)) {
            Dealer = dealer;
        }
    }
}
