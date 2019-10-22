using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Events.Turns
{
    abstract class PlayerTurn
    {
        public Player Player { get; protected set; }

        public string Message;

        public PlayerTurn(Player player, string message)
        {
            Player = player;
            Message = message;
        }
        public override string ToString()
        {
            return Message;
        }
    }
}
