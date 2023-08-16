using JFadich.Pinochle.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace JFadich.Pinochle.Engine.Events
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
