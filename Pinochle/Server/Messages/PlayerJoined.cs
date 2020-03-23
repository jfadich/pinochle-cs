using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Pinochle.Server.Messages
{
    class PlayerJoined : Message
    {
        public int Position { get; set; }
    }
}
