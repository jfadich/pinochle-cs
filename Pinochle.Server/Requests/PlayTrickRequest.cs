using JFadich.Pinochle.Engine.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFadich.Pinochle.Server.Requests
{
    public class PlayTrickRequest
    {
        public PinochleCard Trick { get; set; }
    }
}
