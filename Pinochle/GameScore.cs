using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle
{
    class GameScore
    {
        public int TeamA { get; private set; }
        public int TeamB { get; private set; }

        public void AddToTeamA(int score)
        {
            TeamA += score;
        }
        public void AddToTeamB(int score)
        {
            TeamB += score;
        }
    }
}
