using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Actions
{
    public abstract class PlayerAction
    {
        public Round.Phases Phase { get;}

        public Seat Seat { get; }

        public PlayerAction (Seat seat, Round.Phases phase)
        {
            Phase = phase;
            Seat = seat;
        }

        public abstract bool Apply(Round round);
    }
}
