using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Server.Messages
{
    class Chat : Message
    {
        public string Message { get; set; }
    }
}
