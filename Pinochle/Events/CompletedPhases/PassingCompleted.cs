
using System;

namespace Pinochle.Events.CompletedPhases
{
    public class PassingCompleted : PhaseCompleted
    {
        public PassingCompleted() : base(Phases.Passing, Phases.Playing) { }
    }
}
