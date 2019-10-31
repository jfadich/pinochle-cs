using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle
{
    class Player
    {
        public int Position { get; protected set; }

        public Player(int position)
        {
            if(position > 3)
            {
                throw new Exception("Invalid Player Position");
            }

            Position = position;
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
