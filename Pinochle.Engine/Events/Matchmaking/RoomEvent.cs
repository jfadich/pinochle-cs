using JFadich.Pinochle.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Engine.Events.Matchmaking
{
    public class RoomEvent
    {
        public GameRoom Room;

        public RoomEvent(GameRoom room)
        {
            Room = room;
        }
    }
}
