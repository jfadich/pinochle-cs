using JFadich.Pinochle.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinochle.Engine.Contracts
{
    public interface IAuction
    {
        public int CurrentBid { get; }

        public int WinningBid { get; }

        public void Open(Seat player);

        public void PlaceBid(Seat player, int bid);

        public int GetPlayerBid(Seat player);

        public bool PlayerHasPassed(Seat player);
    }
}
