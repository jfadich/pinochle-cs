
using System;

namespace Pinochle.Events.Phases
{
    class PassingCompleted : Events.PhaseCompleted
    {
        public PassingCompleted() : base(Round.Phases.Passing, Round.Phases.Playing) { }
    }
}
