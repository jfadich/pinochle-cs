using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Events.Turns
{
    abstract class PlayerTurn
    {
        public Player Player { get; protected set; }

        public PlayerTurn(Player player)
        {
            Player = player;
        }
    }
}
