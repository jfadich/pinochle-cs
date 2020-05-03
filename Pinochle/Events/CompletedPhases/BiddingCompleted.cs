using System;

namespace JFadich.Pinochle.Engine.Events.CompletedPhases
{
    public class BiddingCompleted : PhaseCompleted
    {
        public Seat Winner { get; }

        public int Bid { get; }

        public BiddingCompleted(Seat winner, int bid) : base(Phases.Bidding, Phases.Calling) 
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
