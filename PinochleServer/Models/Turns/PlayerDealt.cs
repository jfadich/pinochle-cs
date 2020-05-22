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
    public class PlayerDealt : TurnTaken
    {
        public IHand MyHand { get; }
        public PlayerDealt(ActionTaken actionTaken, Seat seat) : base(actionTaken)
        {
            if(seat != null)
            {
                MyHand = (actionTaken.Action as Deal).Hands[seat.Position];
            }
        }
    }
}
