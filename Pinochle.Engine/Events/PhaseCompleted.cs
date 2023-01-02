using System;
using System.Collections.Generic;
using System.Text;
using JFadich.Pinochle.Engine.Actions;

namespace JFadich.Pinochle.Engine.Events
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
