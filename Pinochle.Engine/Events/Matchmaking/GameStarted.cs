﻿using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Engine.Events.Matchmaking
{
    public class GameStarted : RoomEvent
    {
        public GameStarted(GameRoom room) : base(room) { }
    }
}
