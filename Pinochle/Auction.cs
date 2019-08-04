using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle
{
    class Auction
    {
        public int[] Bids;

        public int StartingBid { get; protected set; } = 25;

        public int CurrentBid { get; protected set; }
        public int WinningBid { get; protected set; }

        public int WinningPosition { get; protected set; }

        public bool IsOpen { get; protected set; } = true;

        public bool[] PlayersPassed;

        public Auction()
        {
            Bids = new int[4];
            PlayersPassed = new bool[4];
        }

        public void Open(Player player)
        {
            PlaceBid(player, StartingBid);
        }

        public bool PlayerPassed(Player player) => PlayersPassed[player.Position];

        public void Pass(Player player)
        {
            if (PlayersPassed[player.Position])
            {
                throw new Exceptions.PinochleRuleViolationException("Player has already passed.");
            }

            PlayersPassed[player.Position] = true;
            int passedPlayers = 0;

            foreach(bool playerPassed in PlayersPassed)
            {
                if(playerPassed)
                {
                    passedPlayers++;
                }
            }

            if(passedPlayers == 3)
            {
                Close();
            }
        }

        public void PlaceBid(Player player, int bid)
        {
            int playersBid = Bids[player.Position];
            int minBid = CurrentBid + 1;

            if (PlayersPassed[player.Position])
            {
                throw new Exceptions.PinochleRuleViolationException("Player has already passed.");
            }

            if (bid < minBid)
            {
                throw new Exceptions.PinochleRuleViolationException("Please enter a bid of at least " + minBid);
            }

            Bids[player.Position] = bid;
            CurrentBid = bid;
        }
        protected void Close()
        {
            IsOpen = false;
            WinningBid = CurrentBid;

            for (int position = 0; position < 4; position++)
            {
                if(PlayersPassed[position])
                {
                    continue;
                }

                WinningPosition = position;
                break;
            }

        }
    }
}
