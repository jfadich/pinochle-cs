using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Events.Turns
{
    class BidPassed : PlayerTurn
    {
        public BidPassed(Player player) : base(player, String.Format("{0} passed", player)) { }
    }
}
