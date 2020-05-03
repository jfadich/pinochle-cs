﻿using JFadich.Pinochle.Engine.Contracts;

namespace JFadich.Pinochle.Engine.Actions
{
    public class Deal : PlayerAction
    {
        public IHand[] Hands { get; private set; }

        public Deal(Seat seat) : base(seat, Phases.Dealing) { }
        public override bool Apply(IGameRound round)
        {
            round.Deal(Seat);

            Hands = round.Hands;

            return true;
        }
    }
}
