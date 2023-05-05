﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JFadich.Pinochle.Engine.Actions;
using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Engine.Events;
using JFadich.Pinochle.Engine.Contracts;
using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Exceptions;
using JFadich.Pinochle.Engine.Events.CompletedPhases;
using Spectre.Console;

namespace JFadich.Pinochle.PlayConsole
{
    class ConsoleGame
    {
        public Dictionary<Phases, List<ActionTaken>> Turns;
        public List<PhaseCompleted> CompletedPhases;

        private Engine.Tricks.Trick CurrentTrick;

        protected IPinochleGame Game;

        private IAnsiConsole _console { get; }

        public ConsoleGame(IAnsiConsole console)
        {
            _console = console;

         //   Console.BackgroundColor = ConsoleColor.White;
         //   Console.ForegroundColor = ConsoleColor.Black;
         //   Console.Clear();
        }

        public void Play()
        {
            Game = GameFactory.Make();
            Turns = new Dictionary<Phases, List<ActionTaken>>();
            CompletedPhases = new List<PhaseCompleted>();
            
            foreach(Phases phase in Enum.GetValues(typeof(Phases)))
            {
                Turns.Add(phase, new List<ActionTaken>());
            }

            Game.AddGameListener(OnGameEvent);
            Game.StartGame();

            while(!Game.IsCompleted)
            {
                Draw();
                Auction();
                Draw();
                CallTrump();
                Draw();
                PassCards();
                Draw();
                ShowMeld();
                Draw();
                PlayTricks();
            }
        }

        protected void Auction()
        {
            while (Game.IsPhase(Phases.Bidding))
            {
                DrawHand();

                int bid = _console.Ask<int>(String.Format("What does {0} bid? 0 to pass", Game.ActivePlayer));

                try
                {
                    if (bid == 0)
                    {
                        Game.TakeAction(new PlaceBid(Game.ActivePlayer, -1)); // todo replace -1 with constant from Auction
                        Draw();
                        continue;
                    }

                    Game.TakeAction(new PlaceBid(Game.ActivePlayer, bid));

                    Draw();
                }
                catch (PinochleRuleViolationException exception)
                {
                    _console.MarkupLineInterpolated($"[red]{exception.Message}[/]");
                }
            }
        }

        protected void CallTrump()
        {
            PinochleCard.Suits? trump = null;

            do
            {
                DrawHand();
                Console.WriteLine(Game.ActivePlayer + " please select trump [c,h,s,d]");

                string providedTrump = Console.ReadLine();
                providedTrump.Trim();
                providedTrump.ToLower();

                switch (providedTrump)
                {
                    case "c":
                    case "clubs":
                        trump = PinochleCard.Suits.Clubs;
                        break;
                    case "h":
                    case "hearts":
                        trump = PinochleCard.Suits.Hearts;
                        break;
                    case "s":
                    case "spades":
                        trump = PinochleCard.Suits.Spades;
                        break;
                    case "d":
                    case "diamonds":
                        trump = PinochleCard.Suits.Diamonds;
                        break;
                }
            } while (trump == null);

            Game.TakeAction(new CallTrump(Game.ActivePlayer, (PinochleCard.Suits)trump));
        }

        protected void PassCards()
        {
            PinochleCard[] pass = AskForCards("select 4 cards pass to your partner", 4);

            Game.TakeAction(new PassCards(Game.ActivePlayer, pass));

            Draw();

            pass = AskForCards("select 4 cards pass back to your partner", 4);
            Game.TakeAction(new PassCards(Game.ActivePlayer, pass));
        }

        public void ShowMeld()
        {
            for (int i = 0; i < Game.PlayerCount; i++)
            {
                Seat player = Seat.ForPosition(i);
                ICollection<Meld>  meld = Game.GetPlayerMeld(player);

                Console.WriteLine("{0} Meld. {1} Points", player, meld.Sum(score => score.GetValue()));
                Console.WriteLine(Game.GetPlayerHand(player));
                foreach(Meld meldScore in meld)
                {
                    Console.WriteLine(meldScore);
                }
            }

            Console.ReadLine();
        }

        public void PlayTricks()
        {
            PinochleCard trick = AskForACard(" Play a trick.");

            Game.TakeAction(new PlayTrick(Game.ActivePlayer, trick));
            Draw();

            while(Game.IsPhase(Phases.Playing))
            {

                trick = AskForACard(" Play a trick.");

                try
                {
                    Game.TakeAction(new PlayTrick(Game.ActivePlayer, trick));
                    Draw();
                } catch(IllegalTrickException e)
                {
                    Draw();
                    Console.WriteLine(e.Message);
                }
            }
        }

        protected PinochleCard AskForACard(string message)
        {
            return AskForCards(message, 1)[0];
        }

        protected PinochleCard[] AskForCards(string message, int numberOfCards)
        {
            IHand hand = Game.GetPlayerHand(Game.ActivePlayer);

            int j = 0;
            foreach (PinochleCard card in hand.Cards)
            {
                bool canPlay = true;
                Console.ResetColor();

                if (card.getColor() == PinochleCard.Color.Red)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (Game.IsPhase(Phases.Playing))
                {
                    canPlay = Game.CanPlay(card);

                    if(!canPlay)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(String.Format("X: {0}", card.GetName()));
                        j++;
                        continue;
                    }
                }

                Console.WriteLine(String.Format("{0}: {1} {2}", j, card.GetSuitShortName(), card.GetName()));
                j++;
                
            }

            Console.ResetColor();
            Console.WriteLine(Game.ActivePlayer + message);
            

            bool inputValid = false;
            PinochleCard[] selectedCards;

            do
            {
                string cardsInput = Console.ReadLine();
                string[] cards = cardsInput.Split(" ");
                selectedCards = new PinochleCard[numberOfCards];

                if (cards.Length < numberOfCards)
                {
                    continue;
                }

                for (int i = 0; i < numberOfCards; i++)
                {
                    try
                    {
                        int cardIndex = int.Parse(cards[i]);
                        if (cardIndex > hand.Cards.Length - 1)
                        {
                            inputValid = false;
                            break;
                        }
                        else
                        {
                            selectedCards[i] = hand.Cards[cardIndex];
                            inputValid = true;
                        }
                    }
                    catch (Exception)
                    {
                        inputValid = false;
                    }
                }

            } while (!inputValid);

            return selectedCards;
        }

        protected void DrawHand()
        {
            AnsiConsole.WriteLine();
            var hand = Game.GetPlayerHand(Game.ActivePlayer);

            // Create a table
            var table = new Table();

            table.HideHeaders();
            for(int i = 0; i < hand.Cards.Length; i++)
            {
                table.AddColumn(i.ToString());
            }

            List<Text> handRow = new();

            foreach (PinochleCard card in hand.Cards)
            {
                Text text;

                if (card.getColor() == PinochleCard.Color.Red)
                {
                    text = new Text(card.GetShortName(), new Style(Color.Red, Color.White));
                }
                else
                {
                    text = new Text(card.GetShortName(), new Style(Color.Black, Color.White));
                }

                text.Centered();
                handRow.Add(text);
            }
            
            table.AddRow(handRow);
            table.Expand();

            _console.Write(table);
            _console.WriteLine("");
        }


        protected void Draw()
        {
            _console.Clear();


            //     GameScore score = Game.GetScore();
            //     var roundScore = Game.GetRoundScore();
            _console.Write("Phase " + Game.CurrentPhase.ToString());
            _console.Write(" | Current Player: " + Game.ActivePlayer);
        //    Console.Write(string.Format(" | Team A: {0}", score.TeamA));
            if(Game.CurrentPhase == Phases.Playing)
            {
        //        Console.Write(string.Format(" (+{0})", roundScore.TeamA));
            }
      //      Console.Write(string.Format(" | Team B: {0}", score.TeamB));

            if (Game.CurrentPhase == Phases.Playing)
            {
     //           Console.Write(string.Format(" (+{0})", roundScore.TeamB));
            }
            _console.WriteLine();
            _console.WriteLine("----------------------------------------------------------------------------------");

            foreach(PhaseCompleted phase in CompletedPhases)
            {
                _console.WriteLine(phase.ToString());
            }

            _console.WriteLine("----------------------------------------------------------------------------------");

            if(Game.IsPhase(Phases.Playing))
            {
                if(CurrentTrick != null)
                {
                    foreach (var play in CurrentTrick.Plays)
                    {
                        string s = play.Position + " : " + play.Card;

                        if(CurrentTrick.IsCompleted && CurrentTrick.WinningPlay == play)
                        {
                            s += " Winner";
                        }

                        _console.WriteLine(s);
                    }
                }
            } 
            else
            {
                foreach (ActionTaken turn in Turns[Game.CurrentPhase])
                {
                    _console.WriteLine(turn.ToString());
                }
            }


            _console.WriteLine("----------------------------------------------------------------------------------");
        }

        public void OnGameEvent(GameEvent gameEvent)
        {
            if (gameEvent is ActionTaken turn)
            {
                Turns[turn.Action.Phase].Add(turn);

                if (turn.Action is PlayTrick trickAction)
                {
                    CurrentTrick = trickAction.CurrentTrick;
                }
            }

            if(gameEvent is PhaseCompleted completed)
            {
                if(completed is PlayingCompleted)
                {
                    foreach (Phases phase in Enum.GetValues(typeof(Phases)))
                    {
                        Turns[phase].Clear();
                    }
                }
                CompletedPhases.Add(completed);
            }
        }
    }
}
