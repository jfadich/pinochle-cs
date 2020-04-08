
using Pinochle.Tricks;
using System;

namespace Pinochle.Events.Phases
{
    class PlayingCompleted : PhaseCompleted
    {
        Arena Arena { get; }
        public PlayingCompleted(Arena arena) : base(Round.Phases.Playing, FormatMessage(arena)) {
            Arena = arena;
        }

        public static string FormatMessage(Arena arena)
        {
            int[] scores = arena.GetTeamScore();

            return String.Format("Tricks Completed : Team 1 {0} points | Team 2 {1} points", scores[0], scores[1]);
        }
    }
}
