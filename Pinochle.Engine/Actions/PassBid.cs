using System;
using JFadich.Pinochle.Engine.Contracts;

namespace JFadich.Pinochle.Engine.Actions
{
    public class PassBid : PlayerAction
    {
        public PassBid(Seat seat) : base(seat, Phases.Bidding) { }

        public override bool Apply(IGameRound round)
        {
            round.PlaceBid(Seat, Auction.BidPass);

            return true;
        }

        public override string ToString()
        {
            return String.Format("{0} passed", Seat);
        }
    }
}
