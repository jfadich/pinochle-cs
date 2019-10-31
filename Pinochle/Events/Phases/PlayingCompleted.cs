
using System;

namespace Pinochle.Events.Phases
{
    class PlayingCompleted : PhaseCompleted
    {
        public PlayingCompleted() : base(Round.Phases.Playing, String.Format("Round Complete")) { }
    }
}
