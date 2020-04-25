using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pinochle;
using System.Text.Json.Serialization;

namespace PinochleServer.Models
{
    public class Player
    {
        [JsonConverter(typeof(Converters.SeatConverter))]
        public Seat Seat { get; set; }

        public string Id { get; set;  }
    }
}
