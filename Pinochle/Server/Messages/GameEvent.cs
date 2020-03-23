using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Server.Messages
{
    class GameEvent : Message
    {
        public string Event;

        public GameEvent(object data)
        {
            Event = data.GetType().Name;
        }
    }
}
