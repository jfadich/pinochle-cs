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
        public PlayerDealt(ActionTaken actionTaken) : base(actionTaken) { }
    }
}
