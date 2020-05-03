using System;
using System.Collections.Generic;
using System.Text;
using Pinochle;

namespace Pinochle.Events
{
    public class AdvancedPhase : GameEvent
    {
        public Phases Previous { get; }
        public Phases Current { get; }

        public AdvancedPhase(Phases previous, Phases current)
        {
            Previous = previous;
            Current = current;
        }
    }
}
