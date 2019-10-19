using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Events.Turns
{
    class AuctionOpened : BidPlaced
    {
        new public bool IsOpen = true;
        public AuctionOpened(Player player, int bid) : base(player, bid) { }

        public override string ToString()
        {
            return String.Format("{0} opened with {1}", Player, Bid);
        }
    }
}
