using System;

namespace Pinochle.Events.Phases
{
    class BiddingCompleted : PhaseCompleted
    {

        public BiddingCompleted(Player winner, int bid) : base(Round.Phases.Bidding, String.Format("{0} wins the bid with a bid of {1}", winner, bid)) { }
      
    }
}
