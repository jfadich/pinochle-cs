using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Events.Turns
{
    class BidPlaced : PlayerTurn
    {
        public int Bid { get; protected set; }

        public bool IsOpen = false;
        public BidPlaced(Player player, int bid) : base(player, String.Format("{0} bid {1}", player, bid))
        {
            Bid = bid;
        }
    }
}
