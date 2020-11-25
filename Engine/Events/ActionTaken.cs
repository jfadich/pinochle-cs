using System;
using System.Collections.Generic;
using System.Text;
using JFadich.Pinochle.Engine.Actions;

namespace JFadich.Pinochle.Engine.Events
{
    public class ActionTaken : GameEvent
    {
        public PlayerAction Action { get; }

        public Seat NextPlayer { get; }

        public ActionTaken(PlayerAction action, Seat nextPlayer)
        {
            Action = action;
            NextPlayer = nextPlayer;
        }

        public override string ToString()
        {
            return Action.ToString();
        }
    }
}
