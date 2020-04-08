using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle
{
    class Player
    {
        public int Position { get; }

        public String Name { get; }

        public Player(int position) : this(position, String.Format("Player {0}", position + 1)) { }

        public Player(int position, String name)
        {
            Position = Game.ValidPosition(position) ? position : 0;
            Name = name;
        }

        public Boolean IsSameTeam(Player player)
        {
            return (player.Position & 3) == (Position & 3);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
