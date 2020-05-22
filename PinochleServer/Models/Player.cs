using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFadich.Pinochle.Engine;
using System.Text.Json.Serialization;

namespace JFadich.Pinochle.Server.Models
{
    public class Player
    {
        public Seat Seat { get; set; }

        public string Id { get; set;  }
    }
}
