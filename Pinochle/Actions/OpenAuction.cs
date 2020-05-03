﻿using System;
using JFadich.Pinochle.Engine.Contracts;

namespace JFadich.Pinochle.Engine.Actions
{
    public class OpenAuction : PlayerAction
    {
        public int Bid { get; }

        public OpenAuction(Seat seat) : base(seat, Phases.Bidding) { }
        public override bool Apply(IGameRound round)
        {
            round.OpenAuction(Seat);

            return true;
        }

        public override string ToString()
        {
            return String.Format("{0} opened with {1}", Seat, Auction.StartingBid);
        }
    }
}
