using System;
using System.Collections.Generic;
using Pinochle.Cards;
using Pinochle.Events.Phases;
using System.Linq;
using Pinochle.Actions;

namespace Pinochle
{
    class Game : Contracts.IPinochleGame
    {
        private event Action<Events.GameEvent> GameEvents;
        private readonly LinkedList<Round> rounds = new LinkedList<Round>(); // @todo remove this, let clients track if they need

        private Seat[] Players;

        private Round CurrentRound;

        public GameScore Score;

        public Seat ActivePlayer { 
            get { return Players[activePosition]; } 
            private set { activePosition = value.Position; } 
        }

        public void AddGameListener(Action<Events.GameEvent> listener)
        {
            GameEvents += listener;
        }

        public int StartingPosition
        {
            get { return startingPosition; }
            protected set { startingPosition = ValidatePosition(value); }
        }

        private int startingPosition;

        private int activePosition;

        public bool HasStarted;

        public bool IsCompleted;

        public const int NumberOfPlayers = 4;

        public Game()
        {
            Players = new Seat[NumberOfPlayers];
            Score = new GameScore();
            HasStarted = false;
            IsCompleted = false;
        }

        public void StartGame(int startingPosition = 0)
        {
            StartingPosition = startingPosition;
            activePosition = StartingPosition;


            for(int i = 0;  i < Players.Length; i++)
            {
                Players[i] = Seat.ForPosition(i);
            }

            HasStarted = true;
            StartRound();
        }

        protected void StartRound()
        {
            CurrentRound = new Round();

            TakeAction(new Deal(ActivePlayer));

            TakeAction(new OpenAuction(ActivePlayer));
        }

        public void TakeAction(PlayerAction action)
        {
            if (!IsCurrently(action.Phase))
            {
                throw new Exceptions.InvalidActionException("Game is not currently " + action.Phase);
            }

            if (activePosition != action.Seat.Position)
            {
                throw new Exceptions.OutOfTurnException();
            }
            
            if(action.Apply(CurrentRound)) {
                AdvancePlayer();

                GameEvents?.Invoke(new Events.ActionTaken(action, ActivePlayer));

                if(!IsCurrently(action.Phase))
                {
                    CompletePhase(action);
                }
            }
        }

        private void AdvancePlayer()
        {
            if (IsCurrently(Round.Phases.Bidding))
            {
                do
                {
                    SetNextPlayer();
                } while (CurrentRound.Auction.PlayerHasPassed(ActivePlayer));
            }
            else if (IsCurrently(Round.Phases.Calling) || IsCurrently(Round.Phases.Passing))
            {
                SetNextPlayer(1);
            } 
            else if (IsCurrently(Round.Phases.Playing)) 
            {
                if(CurrentRound.Arena != null)
                {
                    if(CurrentRound.Arena.ActiveTrick.IsCompleted)
                    {
                        ActivePlayer = CurrentRound.Arena.ActiveTrick.WinningPlay.Position;
                    } else
                    {
                        SetNextPlayer();

                    }

                } 
            } 
            else
            {
                SetNextPlayer();
            }
        }

        private void CompletePhase(PlayerAction action)
        {
            Round.Phases completedPhase = action.Phase;

            if (completedPhase == Round.Phases.Dealing)
            {
                GameEvents?.Invoke(new DealingCompleted(action.Seat));
            }
            else if (completedPhase == Round.Phases.Bidding)
            {
                GameEvents?.Invoke(new BiddingCompleted(Players[CurrentRound.Auction.WinningPosition], CurrentRound.Auction.WinningBid));

                ActivePlayer = Players[CurrentRound.Auction.WinningPosition];
            }
            else if(completedPhase == Round.Phases.Calling)
            {
                GameEvents?.Invoke(new CallingCompleted(Players[CurrentRound.Auction.WinningPosition], CurrentRound.Trump));

            }
            else if(completedPhase == Round.Phases.Passing)
            {
                CalculateMeld();

                GameEvents?.Invoke(new PassingCompleted()); // @todo pass meld in here

                CurrentRound.OpenArena();

                ActivePlayer = CurrentRound.Leader.Seat;
            }
            else if (completedPhase == Round.Phases.Playing)
            {
                GameEvents?.Invoke(new PlayingCompleted()); // @todo pass meld in here

                CompleteRound();
            }
        }

        public bool IsCurrently(Round.Phases phase) => (CurrentRound.Phase == phase);

        public Round.Phases CurrentPhase() => (CurrentRound.Phase);

        public Hand GetPlayerHand(Seat player)
        {
            return CurrentRound.PlayerHand(player);
        }

        public Seat[] AllPlayers()
        {
            return Players;
        }

        public List<Meld> GetPlayerMeld(Seat player)
        {
            return CurrentRound.MeldScore[player.Position];
        }

        public bool CanPlay(PinochleCard card)
        {
            return CurrentRound.Arena.CanPlay(card, GetPlayerHand());
        }


        public static bool IsValidPosition(int position)
        {
            return position > 0 && position < NumberOfPlayers;
        }

        public static int ValidatePosition(int position)
        {
            return IsValidPosition(position) ? position : 0;
        }

        private void CalculateMeld()
        {
            foreach (Seat player in Players)
            {
                CurrentRound.CalculateMeld(player);
            }
        }

        public void CompleteRound()
        {
            var roundScore = CurrentRound.CalculateTeamScore();

            Score.AddToTeamA(roundScore[0]);
            Score.AddToTeamB(roundScore[1]);
            //rounds.Append(CurrentRound);


            // Emit Round Complete

            if(Score.TeamA >= 150 || Score.TeamB >= 150) {
                IsCompleted = true;
                //Console.WriteLine("Game Over!"); @todo emit game completed event
            } else
            {
                activePosition = CurrentRound.Dealer.Position;
                SetNextPlayer();
                StartRound();
            }
        }

        public GameScore GetScore()
        {
         //   int[] scores = new int[4];

        //    scores[0] = rounds.Sum(round => round.TeamScore[0]);
        //    scores[1] = rounds.Sum(round => round.TeamScore[1]);

            return Score;
        }

        public RoundScore GetRoundScore()
        {
            return new RoundScore(CurrentRound.Arena, CurrentRound.MeldScore);
        }

        public Seat PlayerAtPosition(int position)
        {
            if(!IsValidPosition(position))
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

        protected int GetNexPosition(int sameTeam = 0) => ActivePlayer.NextPosition(sameTeam);
    }
}
