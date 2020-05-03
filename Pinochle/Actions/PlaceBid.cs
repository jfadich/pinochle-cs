using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Actions
{
    class PlaceBid : PlayerAction
    {
        public int Bid { get; }

        public PlaceBid(Seat seat, int bid) : base(seat, Round.Phases.Bidding)
        {
            Bid = bid;
        }
        public override bool Apply(Round round)
        {
            round.PlaceBid(Seat, Bid);

            return true;
        }

        public override string ToString()
        {
            if(Bid == -1)
            {
                return String.Format("{0} passed", Seat);
            }

            return String.Format("{0} bid {1}", Seat, Bid);
        }
    }
}
