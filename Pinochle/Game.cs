using System;
using System.Collections.Generic;
using Pinochle.Cards;
using Pinochle.Events.Phases;
using Pinochle.Events.Turns;

namespace Pinochle
{
    class Game
    {
        public event Action<PlayerTurn> TurnTaken;
        public event Action<PhaseCompleted> PhaseCompleted;
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
        protected void StartRound()
        {
            if (CurrentRound != null)
            {
                Rounds.Add(CurrentRound);
            }

            CurrentRound = new Round();

            CurrentRound.Deal(ActivePlayer);

            TurnTaken?.Invoke(new DealtHands(ActivePlayer));

            PhaseCompleted?.Invoke(new DealingCompleted(ActivePlayer));

            SetNextPlayer();

            CurrentRound.Auction.Open(ActivePlayer);

            TurnTaken?.Invoke(new AuctionOpened(ActivePlayer, CurrentRound.Auction.CurrentBid));

            SetNextPlayer();
        }

        public void PlaceBid(int bid)
        {
            CurrentRound.Auction.PlaceBid(ActivePlayer, bid);

            TurnTaken?.Invoke(new BidPlaced(ActivePlayer, CurrentRound.Auction.CurrentBid));

            do
            {
                SetNextPlayer();
            } while (CurrentRound.Auction.PlayerPassed(ActivePlayer));
        }

        public void PassBid()
        {
            CurrentRound.Auction.Pass(ActivePlayer);

            TurnTaken?.Invoke(new BidPassed(ActivePlayer));

            if ( ! CurrentRound.Auction.IsOpen)
            {
                PhaseCompleted?.Invoke(new BiddingCompleted(Players[CurrentRound.Auction.WinningPosition], CurrentRound.Auction.WinningBid));

                CurrentRound.AdvancePhase();
            }

            do
            {
                SetNextPlayer();
            } while (CurrentRound.Auction.PlayerPassed(ActivePlayer));
        }

        public void CallTrump(PinochleCard.Suits Trump)
        {
            CurrentRound.CallTrump(ActivePlayer, Trump);

            TurnTaken?.Invoke(new TrumpCalled(ActivePlayer, Trump));

            PhaseCompleted?.Invoke(new CallingCompleted(ActivePlayer, Trump));

            SetNextPlayer(1);
        }

        public void PassCardsToLeader(Card[] cards)
        {
            GetPlayerHand().TakeCards(cards);

            SetNextPlayer(1);

            GetPlayerHand().GiveCards(cards);
        }

        public void PassCardsBack(Card[] cards)
        {
            GetPlayerHand().TakeCards(cards);

            CurrentRound.PlayerHand(Players[GetNexPosition(1)]).GiveCards(cards);

            PhaseCompleted?.Invoke(new PassingCompleted());
        }

        public Hand GetPlayerHand()
        {
            return CurrentRound.PlayerHand(ActivePlayer);
        }

        protected Game SetNextPlayer(int sameTeam = 0)
        {
            ActivePlayer = Players[GetNexPosition(sameTeam)];

            return this;
        }

        protected int GetNexPosition(int sameTeam = 0) => (ActivePlayer.Position + 1 + sameTeam) & 3;
    }
}
