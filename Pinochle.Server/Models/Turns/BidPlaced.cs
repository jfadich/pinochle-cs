using JFadich.Pinochle.Engine.Actions;
using JFadich.Pinochle.Engine.Contracts;
using JFadich.Pinochle.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFadich.Pinochle.Engine;

namespace PinochleServer.Models.Turns
{
    public class BidPlaced : TurnTaken
    {
        public int Bid { get; }

        public BidPlaced(ActionTaken actionTaken, int bid) : base(actionTaken)
        {
            Bid = bid;
        }
    }
}
