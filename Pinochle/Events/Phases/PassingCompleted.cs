
using System;

namespace Pinochle.Events.Phases
{
    class PassingCompleted : PhaseCompleted
    {
        public PassingCompleted() : base(Round.Phases.Dealing, String.Format("Passing Completed")) { }
    }
}
