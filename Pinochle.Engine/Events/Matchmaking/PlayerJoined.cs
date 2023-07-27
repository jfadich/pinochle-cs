using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Events;
using JFadich.Pinochle.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Engine.Events.Matchmaking
{
    public class PlayerJoined : RoomEvent
    {
        public Player Player;

        public PlayerJoined(GameRoom room, Player player) : base(room)
        {
            Player = player;
        }
    }
}
