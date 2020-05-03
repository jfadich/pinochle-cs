using System;
using System.Collections.Generic;
using System.Text;
using Pinochle.Actions;

namespace Pinochle.Events
{
    class PhaseCompleted : GameEvent
    {
        public Round.Phases Phase { get; }

        public Round.Phases Next { get; }

        public PhaseCompleted(Round.Phases phase, Round.Phases nextPhase)
        {
            Phase = phase;
            Next = nextPhase;
        }

        public override string ToString()
        {
            return Phase + " Completed";
        }
    }
}
