using System;
using System.Collections.Generic;
using Pinochle.Cards;
using Pinochle.Events.Phases;
using Pinochle.Events.Turns;
using System.Linq;

namespace Pinochle
{
    class Game
    {
        public event Action<PlayerTurn> TurnTaken;
        public event Action<PhaseCompleted> PhaseCompleted;
        public List<Round> Rounds { get; protected set; } = new List<Round>();

        public Player[] Players;

        public Round CurrentRound { get; protected set; }

        public Player ActivePlayer { get; protected set; }

        public bool IsCompleted = false;

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

        public Round.Phases IsCurrently() => (CurrentRound.Phase);


        protected void StartRound()
        {
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

        public void PassCardsToLeader(PinochleCard[] cards)
        {
            Player partner = Players[GetNexPosition(1)];

            TurnTaken?.Invoke(new PassedCards(ActivePlayer, partner, cards));

            GetPlayerHand().TakeCards(cards);

            SetNextPlayer(1);

            GetPlayerHand().GiveCards(cards);
        }

        public void PassCardsBack(PinochleCard[] cards)
        {
            Player partner = Players[GetNexPosition(1)];

            GetPlayerHand().TakeCards(cards);

            CurrentRound.PlayerHand(partner).GiveCards(cards);

            TurnTaken?.Invoke(new PassedCards(ActivePlayer, partner, cards));

            CurrentRound.AdvancePhase();

            CalculateMeld();

            PhaseCompleted?.Invoke(new PassingCompleted());
        }

        public void CalculateMeld()
        {
            foreach (Player player in Players)
            {
                CurrentRound.CalculateMeld(player);
            }
        }

        public void StartTricks()
        {
            CurrentRound.OpenArena();

            ActivePlayer = CurrentRound.Leader;
        }

        public void PlayTrick(PinochleCard play)
        {
            CurrentRound.PlayTrick(ActivePlayer, play);

            TurnTaken?.Invoke(new TrickPlayed(ActivePlayer, play));

            if (CurrentRound.Arena.ActiveTrick.IsCompleted)
            {
                ActivePlayer = CurrentRound.Arena.ActiveTrick.WinningPlay.Position;

                TurnTaken?.Invoke(new TrickCompleted(ActivePlayer));
            }
            else
            {
                SetNextPlayer();
            }

            if (!IsCurrently(Round.Phases.Playing))
            {
                PhaseCompleted?.Invoke(new PlayingCompleted());
                CompleteRound();
                return;
            }
        }

        public void CompleteRound()
        {
            CurrentRound.CalculateTeamScore();
            Rounds.Add(CurrentRound);

            int[] scores = GetScores();
            if(scores[0] >= 150 || scores[1] >= 150) {
                IsCompleted = true;
                Console.WriteLine("Game Over!");
            } else
            {
                ActivePlayer = CurrentRound.Dealer;
                SetNextPlayer();
                StartRound();
            }
        }

        public int[] GetScores()
        {
            int[] scores = new int[4];

            scores[0] = Rounds.Sum(round => round.TeamScore[0]);
            scores[1] = Rounds.Sum(round => round.TeamScore[1]);

            return scores;
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
