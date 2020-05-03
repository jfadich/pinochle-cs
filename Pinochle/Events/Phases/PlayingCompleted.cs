
using Pinochle.Tricks;
using System;

namespace Pinochle.Events.Phases
{
    class PlayingCompleted : Events.PhaseCompleted
    {
        public PlayingCompleted() : base(Round.Phases.Playing, Round.Phases.Complete) {
        }
    }
}
