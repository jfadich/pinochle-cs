using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Events
{
    class AdvancedPhase : GameEvent
    {
        public Round.Phases Previous { get; }
        public Round.Phases Current { get; }

        public AdvancedPhase(Round.Phases previous, Round.Phases current)
        {
            Previous = previous;
            Current = current;
        }
    }
}
