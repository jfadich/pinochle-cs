using System;
using System.Collections.Generic;
using Pinochle.Cards;

namespace Pinochle.Actions
{
    class PlayTrick : PlayerAction
    {
        public PinochleCard Play;

        public Tricks.Trick CurrentTrick { get; private set; }
        public PlayTrick(Seat player, PinochleCard play) : base(player, Round.Phases.Playing) 
        {
            Play = play;
        }

        public override bool Apply(Round round)
        {
            round.PlayTrick(Seat, Play);

            CurrentTrick = round.Arena.ActiveTrick;

            return true;
        }

        public override string ToString()
        {
            return String.Format("{0} played {1}", Seat, Play);
        }
    }
}
