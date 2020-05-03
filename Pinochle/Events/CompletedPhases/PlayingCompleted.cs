
using JFadich.Pinochle.Engine.Tricks;
using System;

namespace JFadich.Pinochle.Engine.Events.CompletedPhases
{
    public class PlayingCompleted : PhaseCompleted
    {
        public PlayingCompleted() : base(Phases.Playing, Phases.Complete) {
        }
    }
}
