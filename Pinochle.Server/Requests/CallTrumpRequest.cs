using JFadich.Pinochle.Engine.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFadich.Pinochle.Server.Requests
{
    public class CallTrumpRequest
    {
        public PinochleCard.Suits Trump { get; set; }
    }
}
