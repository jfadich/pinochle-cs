
using System;

namespace JFadich.Pinochle.Engine.Events.CompletedPhases
{
    public class PassingCompleted : PhaseCompleted
    {
        public PassingCompleted() : base(Phases.Passing, Phases.Playing) { }
    }
}
