using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Pinochle.Server.Messages
{
    abstract class Message
    {
        public string UserToken { get; set; }

        public long Created { get; }

        public string Type;

        public Message()
        {
            Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public override string ToString()
        {
            Type = this.GetType().Name;
            return JsonConvert.SerializeObject(this);
        }
    }
}
