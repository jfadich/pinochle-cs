using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Actions
{
    class Deal : PlayerAction
    {
        public Hand[] Hands { get; private set; }

        public Deal(Seat seat) : base(seat, Round.Phases.Dealing) { }
        public override bool Apply(Round round)
        {
            round.Deal(Seat);

            Hands = round.Hands;

            return true;
        }
    }
}
