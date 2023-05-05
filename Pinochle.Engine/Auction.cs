using System;
using System.Collections.Generic;
using System.Text;

namespace JFadich.Pinochle.Engine
{
    class Auction
    {
        public int[] Bids;

        public const int BidPass = -1;

        public static int StartingBid { get; } = 25;

        public int CurrentBid { get; private set; }
        public int WinningBid { get; private set; } = 0;

        public int WinningPosition { get; private set; }

        public bool IsOpen => WinningBid == 0;

        public Auction()
        {
            Bids = new int[Game.NumberOfPlayers];
        }

        public void Open(Seat player)
        {
            PlaceBid(player, StartingBid);
        }

        public bool PlayerHasPassed(Seat player) => Bids[player.Position] == BidPass;

        public void PlaceBid(Seat player, int bid)
        {
            int minBid = CurrentBid + 1;

            if (PlayerHasPassed(player))
            {
                throw new Exceptions.PinochleRuleViolationException("Player has already passed.");
            }

            if (bid < minBid && bid != BidPass)
            {
                throw new Exceptions.PinochleRuleViolationException("Must bid  at least " + minBid);
            }

            Bids[player.Position] = bid;

            // If this player is passing, check for the last person standing
            if(bid == BidPass)
            {
                CheckForClose();
            } else
            {
                CurrentBid = bid;
            }
        }

        private void CheckForClose()
        {
            int passedPlayers = 0;

            foreach (int playerBid in Bids)
            {
                if (playerBid == BidPass)
                {
                    passedPlayers++;
                }
            }

            if (passedPlayers == (Game.NumberOfPlayers - 1))
            {
                WinningBid = CurrentBid;
                WinningPosition = Array.IndexOf(Bids, CurrentBid);
            }
        }
    }
}
