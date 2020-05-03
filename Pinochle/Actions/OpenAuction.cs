using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Actions
{
    class OpenAuction : PlayerAction
    {
        public int Bid { get; }

        public OpenAuction(Seat seat) : base(seat, Round.Phases.Bidding) { }
        public override bool Apply(Round round)
        {
            round.Auction.Open(Seat);

            return true;
        }

        public override string ToString()
        {
            return String.Format("{0} opened with {1}", Seat, Auction.StartingBid);
        }
    }
}
