﻿using JFadich.Pinochle.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Engine.Events.Matchmaking
{
    public class RoomCreated : RoomEvent
    {
        public RoomCreated(GameRoom room) : base(room) { }
    }
}
