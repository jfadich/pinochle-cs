using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle
{
    class Player
    {
        public int Position { get; }

        public Player(int position)
        {
            Position = Game.ValidPosition(position) ? position : 0;
        }

        public Boolean IsSameTeam(Player player)
        {
            return (player.Position & 3) == (Position & 3);
        }

        public override string ToString()
        {
            return String.Format("Player {0}", Position + 1);
        }
    }
}
