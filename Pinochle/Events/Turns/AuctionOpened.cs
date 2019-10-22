using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Events.Turns
{
    class AuctionOpened : BidPlaced
    {
        new public bool IsOpen = true;
        public AuctionOpened(Player player, int bid) : base(player, bid) {
            Message = String.Format("{0} opened with {1}", Player, Bid);
        }
    }
}
