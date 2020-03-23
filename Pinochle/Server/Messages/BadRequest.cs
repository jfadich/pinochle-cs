using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Server.Messages
{
    class BadRequest : Message
    {
        public string Message { get; set; }
    }
}
