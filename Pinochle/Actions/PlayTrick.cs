using System;
using Pinochle.Contracts;
using Pinochle.Cards;

namespace Pinochle.Actions
{
    public class PlayTrick : PlayerAction
    {
        public PinochleCard Play;

        public Tricks.Trick CurrentTrick { get; private set; }
        public PlayTrick(Seat player, PinochleCard play) : base(player, Phases.Playing) 
        {
            Play = play;
        }

        public override bool Apply(IGameRound round)
        {
            CurrentTrick = round.PlayTrick(Seat, Play);

            return true;
        }

        public override string ToString()
        {
            return String.Format("{0} played {1}", Seat, Play);
        }
    }
}
