
using Pinochle.Tricks;
using System;

namespace Pinochle.Events.CompletedPhases
{
    public class PlayingCompleted : PhaseCompleted
    {
        public PlayingCompleted() : base(Phases.Playing, Phases.Complete) {
        }
    }
}
