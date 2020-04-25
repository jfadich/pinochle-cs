using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Events.Turns
{
    public abstract class PlayerTurn
    {
        public Seat Player { get; protected set; }

        public string Message;

        public PlayerTurn(Seat player, string message)
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
