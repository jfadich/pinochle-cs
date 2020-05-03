
using System;

namespace Pinochle.Events.Phases
{
    class DealingCompleted : Events.PhaseCompleted
    {
        Seat Dealer;
        public DealingCompleted(Seat dealer) : base(Round.Phases.Dealing, Round.Phases.Bidding) 
        {
            Dealer = dealer;
        }

        public override string ToString()
        {
            return Dealer + " dealt";
        }
    }
}
