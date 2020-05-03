
using System;

namespace Pinochle.Events.CompletedPhases
{
    public class DealingCompleted : PhaseCompleted
    {
        Seat Dealer;
        public DealingCompleted(Seat dealer) : base(Phases.Dealing, Phases.Bidding) 
        {
            Dealer = dealer;
        }

        public override string ToString()
        {
            return Dealer + " dealt";
        }
    }
}
