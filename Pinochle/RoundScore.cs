using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JFadich.Pinochle.Engine
{
    class RoundScore
    {
        public int[] TrickScores;

        public int[] MeldScores;

        public int TargetBid;

        public bool MetBid;

        public int TeamA
        {
            get
            {
                return TrickScores[0] + MeldScores[0];
            }
        }
        
        public int TeamB
        {
            get
            {
                return TrickScores[1] + MeldScores[1];
            }
        }

        public RoundScore(Tricks.Arena arena, List<Meld>[] meld)
        {
            if(arena == null)
            {
                TrickScores = new int[2] { 0, 0 };
            } else
            {
                TrickScores = arena.GetTeamScore();
            }

            MeldScores = new int[2];

            MeldScores[0] += meld[0] == null ? 0 : meld[0].Sum(meld => meld.GetValue());
            MeldScores[0] += meld[2] == null ? 0 : meld[2].Sum(meld => meld.GetValue());

            MeldScores[1] += meld[1] == null ? 0 : meld[1].Sum(meld => meld.GetValue());
            MeldScores[1] += meld[3] == null ? 0 : meld[3].Sum(meld => meld.GetValue());
        }
    }
}
