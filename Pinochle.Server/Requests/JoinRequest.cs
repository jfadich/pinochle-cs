using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFadich.Pinochle.Server.Requests
{
    public class JoinRequest
    {
        public string RoomId { get; set; }

        public string PlayerId { get; set; }

        public int SeatPosition { get; set; }
    }
}
