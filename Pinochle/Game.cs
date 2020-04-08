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
        private readonly List<Round> _rounds = new List<Round>();

        public Player[] Players;

        private Round CurrentRound;

        public Player ActivePlayer { get { return Players[activePosition]; } }

        public int StartingPosition
        {
            get { return startingPosition; }
            protected set { startingPosition = ValidPosition(value) ? value : 0; }
        }
        private int startingPosition;

        private int activePosition;

        public bool IsCompleted;

        public const int NumberOfPlayers = 4;

        public void StartGame(int startingPosition = 0)
        {
            StartingPosition = startingPosition;
            activePosition = StartingPosition;

            Players = new Player[NumberOfPlayers];

            for(int i = 0;  i < NumberOfPlayers; i++)
            {
                Players[i] = new Player(i);
            }

            StartRound();
        }

        public bool IsCurrently(Round.Phases phase) => (CurrentRound.Phase == phase);

        public Round.Phases CurrentPhase() => (CurrentRound.Phase);

        public Hand GetPlayerHand(Player player)
        {
            return CurrentRound.PlayerHand(player);
        }

        public List<Meld> GetPlayerMeld(Player player)
        {
            return CurrentRound.MeldScore[player.Position];
        }

        public bool CanPlay(PinochleCard card)
        {
            return CurrentRound.Arena.CanPlay(card, GetPlayerHand());
        }


        public static bool ValidPosition(int position)
        {
            return position > 0 && position < NumberOfPlayers;
        }

        protected void StartRound()
        {
            CurrentRound = new Round();

            CurrentRound.Deal(ActivePlayer);

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

        private void CalculateMeld()
        {
            foreach (Player player in Players)
            {
                CurrentRound.CalculateMeld(player);
            }
        }

        public void StartTricks()
        {
            CurrentRound.OpenArena();

            activePosition = CurrentRound.Leader.Position;
        }

        public void PlayTrick(PinochleCard play)
        {
            CurrentRound.PlayTrick(ActivePlayer, play);

            TurnTaken?.Invoke(new TrickPlayed(ActivePlayer, play));

            if (CurrentRound.Arena.ActiveTrick.IsCompleted)
            {
                activePosition = CurrentRound.Arena.ActiveTrick.WinningPlay.Position.Position;

                TurnTaken?.Invoke(new TrickCompleted(ActivePlayer));
            }
            else
            {
                SetNextPlayer();
            }

            if (!IsCurrently(Round.Phases.Playing))
            {
                PhaseCompleted?.Invoke(new PlayingCompleted(CurrentRound.Arena));
                CompleteRound();
                return;
            }
        }

        public void CompleteRound()
        {
            CurrentRound.CalculateTeamScore();
            _rounds.Add(CurrentRound);

            // Emit Round Complete

            int[] scores = GetScores();
            if(scores[0] >= 150 || scores[1] >= 150) {
                IsCompleted = true;
                Console.WriteLine("Game Over!");
            } else
            {
                activePosition = CurrentRound.Dealer.Position;
                SetNextPlayer();
                StartRound();
            }
        }

        public int[] GetScores()
        {
            int[] scores = new int[4];

            scores[0] = _rounds.Sum(round => round.TeamScore[0]);
            scores[1] = _rounds.Sum(round => round.TeamScore[1]);

            return scores;
        }

        public Player PlayerAtPosition(int position)
        {
            if(!ValidPosition(position))
            {
                return null;
            }

            return Players[position];
        }

        public Hand GetPlayerHand()
        {
            return CurrentRound.PlayerHand(ActivePlayer);
        }

        protected Game SetNextPlayer(int sameTeam = 0)
        {
            activePosition = GetNexPosition(sameTeam);

            return this;
        }

        protected int GetNexPosition(int sameTeam = 0) => (ActivePlayer.Position + 1 + sameTeam) & 3;
    }
}
