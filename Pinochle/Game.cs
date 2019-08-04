using System;
using System.Collections.Generic;
using Pinochle.Cards;

namespace Pinochle
{
    class Game
    {
        public event Action<string> TurnTaken;
        public event Action<string> PhaseCompleted;
        public List<Round> Rounds { get; protected set; }

        public Player[] Players;

        protected Round CurrentRound;

        public Player ActivePlayer { get; protected set; }

        public void StartGame(int startingPosition = 0)
        {
            int numberOfPlayers = 4;

            Players = new Player[numberOfPlayers];

            for(int i = 0;  i < numberOfPlayers; i++)
            {
                Players[i] = new Player(i);
            }

            ActivePlayer = Players[startingPosition];

            StartRound();
        }

        public bool IsCurrently(Round.Phases phase) => (CurrentRound.Phase == phase);
        
        public void PlaceBid(int bid)
        {
            CurrentRound.Auction.PlaceBid(ActivePlayer, bid);

            TurnTaken?.Invoke(String.Format("{0} Bid {1}", ActivePlayer, bid));

            do
            {
                SetNextPlayer();
            } while (CurrentRound.Auction.PlayerPassed(ActivePlayer));
        }

        public void PassBid()
        {
            CurrentRound.Auction.Pass(ActivePlayer);

            TurnTaken?.Invoke(String.Format("{0} passed", ActivePlayer));

            if ( ! CurrentRound.Auction.IsOpen)
            {
                PhaseCompleted?.Invoke(String.Format("{0} wins the bid with a bid of {1}", Players[CurrentRound.Auction.WinningPosition], CurrentRound.Auction.WinningBid));

                CurrentRound.AdvancePhase();
            }

            do
            {
                SetNextPlayer();
            } while (CurrentRound.Auction.PlayerPassed(ActivePlayer));
        }

        public void CallTrump(Card.Suits Trump)
        {
            CurrentRound.CallTrump(ActivePlayer, Trump);

            PhaseCompleted?.Invoke(String.Format("{0} called {1} for trump", ActivePlayer, Trump));

            SetNextPlayer(1);
        }

        public void PassCards(Card[] cards)
        {

        }

        public Hand GetPlayerHand()
        {
            return CurrentRound.Hands[ActivePlayer.Position];
        }

        protected void StartRound()
        {
            if (CurrentRound != null) {
                Rounds.Add(CurrentRound);
            }

            CurrentRound = new Round();

            CurrentRound.Deal(ActivePlayer);

            PhaseCompleted?.Invoke(String.Format("{0} Dealt", ActivePlayer));

            SetNextPlayer();

            CurrentRound.Auction.Open(ActivePlayer);

            TurnTaken?.Invoke(String.Format("{0} opened with {1}", ActivePlayer, CurrentRound.Auction.CurrentBid));

            SetNextPlayer();
        }

        protected Game SetNextPlayer(int sameTeam = 0)
        {
            ActivePlayer = Players[GetNexPosition(sameTeam)];

            return this;
        }

        protected int GetNexPosition(int sameTeam = 0) => (ActivePlayer.Position + 1 + sameTeam) & 3;
    }
}
