using System;
using System.Collections.Generic;
using System.Text;
using Pinochle.Actions;

namespace Pinochle.Events
{
    public class PhaseCompleted : GameEvent
    {
        public Phases Phase { get; }

        public Phases Next { get; }

        public PhaseCompleted(Phases phase, Phases nextPhase)
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
