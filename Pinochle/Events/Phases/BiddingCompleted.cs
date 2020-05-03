using System;

namespace Pinochle.Events.Phases
{
    class BiddingCompleted : Events.PhaseCompleted
    {
        public Seat Winner { get; }

        public int Bid { get; }

        public BiddingCompleted(Seat winner, int bid) : base(Round.Phases.Bidding, Round.Phases.Calling) 
        {
            Winner = winner;
            Bid = bid;
        }

        public override string ToString()
        {
            return String.Format("{0} wins the bid with {1}", Winner, Bid);
        }

    }
}
