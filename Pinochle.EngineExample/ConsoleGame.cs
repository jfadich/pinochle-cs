using System;
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
using Pinochle.Engine.Contracts;
using Spectre.Console.Rendering;

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
                int bid = _console.Ask<int>(String.Format("What does {0} bid? 0 to pass: ", Game.ActivePlayer));

                try
                {
                    if (bid == 0)
                    {
                        Game.TakeAction(new PassBid(Game.ActivePlayer));
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

            string providedTrump = _console.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select trump.")
                    .AddChoices(new[] {
                            "Clubs", "Hearts", "Spades",
                            "Diamonds"
                    }));

            providedTrump = providedTrump.ToLower();

            switch (providedTrump)
            {
                case "clubs":
                    trump = PinochleCard.Suits.Clubs;
                    break;
                case "hearts":
                    trump = PinochleCard.Suits.Hearts;
                    break;
                case "spades":
                    trump = PinochleCard.Suits.Spades;
                    break;
                case "diamonds":
                    trump = PinochleCard.Suits.Diamonds;
                    break;
            }

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
            IHand hand = Game.GetPlayerHand(Game.ActivePlayer);

            PinochleCard selectedCard = AnsiConsole.Prompt(
                new SelectionPrompt<PinochleCard>()
                    .Title(message)
                    .PageSize(5)
                    .AddChoices(hand.Cards));

            return selectedCard;
        }

        protected PinochleCard[] AskForCards(string message, int numberOfCards)
        {
            IHand hand = Game.GetPlayerHand(Game.ActivePlayer);

            List<PinochleCard> selectedCards = AnsiConsole.Prompt(
                new MultiSelectionPrompt<PinochleCard>()
                    .Title(message)
                    .PageSize(5)
                    .AddChoices(hand.Cards));//.ToList().Select(c => String.Format("{0} {1}", c.GetSuitShortName(), c.GetName())));/);

            /* int j = 0;
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

            } while (!inputValid); */

            return selectedCards.ToArray();
        }

        protected IRenderable DrawHand()
        {
            var hand = Game.GetPlayerHand(Game.ActivePlayer);

            List<Panel> handRow = new();

            foreach (PinochleCard card in hand.Cards)
            {
                Style style;

                if (card.getColor() == PinochleCard.Color.Red)
                {
                    style = new Style(Color.Red, Color.White);
                }
                else
                {
                    style = new Style(Color.Black, Color.White);
                }

                Panel panel = new Panel(new Text(card.GetShortName(), style).Centered());
                panel.BorderStyle = style;
                panel.Padding = new Padding(0,0);

                handRow.Add(panel);
            }
            

            return new Columns(handRow).Padding(0,0).Collapse();
        }

        private Layout DrawBidding()
        {
            int activePosition = Game.ActivePlayer.Position;
            IAuction auction = Game.GetAuction();

            var layout = new Layout("Bidding")
                .SplitColumns(
                    new Layout("Seat_1") { Ratio = 1 },
                    new Layout("Center") { Ratio = 2 },
                    new Layout("Seat_3") { Ratio = 1 });

            layout["Center"].SplitRows(
                            new Layout("Seat_2") { Ratio = 1 },
                            new Layout("Middle") { Ratio = 2 },
                            new Layout("Seat_0") { Ratio = 1 });

            layout["Seat_1"].Update(MakeSeat(new Seat(1),auction, activePosition));
            layout["Seat_3"].Update(MakeSeat(new Seat(3), auction, activePosition));
            layout["Center"]["Seat_2"].Update(MakeSeat(new Seat(2), auction, activePosition));
            layout["Center"]["Seat_0"].Update(MakeSeat(new Seat(0), auction, activePosition));

            layout["Center"]["Middle"].Update(
                new Panel(
                    Align.Center(
                        new FigletText(auction.CurrentBid.ToString()).Color(Color.Green),
                        VerticalAlignment.Middle))
                    .Expand()
                    .BorderColor(Color.Green)
                    );

            return layout;
        }

        private Panel MakeSeat(Seat seat, IAuction auction, int activePosition)
        {
            bool playerHasPassed = auction.PlayerHasPassed(seat);
            Color borderColor = playerHasPassed ? Color.Grey50 : (activePosition == seat.Position ? Color.Gold1 : Color.White);
            string bid = playerHasPassed ? "Passed" : auction.GetPlayerBid(seat).ToString();

            return new Panel(
                    Align.Center(
                        new Rows(
                            new Text(seat.ToString()),
                            new Text(bid)
                        ),
                        VerticalAlignment.Middle))
                    .Expand()
                    .BorderColor(borderColor);
        }

        private IRenderable DrawFeed()
        {
            List<IRenderable> feed = new()
            {
                new Rule("Round ").LeftJustified() // todo get round number
            };

            foreach (PhaseCompleted phase in CompletedPhases)
            {
                feed.Add(new Text(phase.ToString()));
            }

            feed.Add(new Rule(Game.CurrentPhase.ToString()).LeftJustified());

            if (Game.IsPhase(Phases.Playing))
            {
                if (CurrentTrick != null)
                {
                    foreach (var play in CurrentTrick.Plays)
                    {
                        string s = play.Position + " : " + play.Card;

                        if (CurrentTrick.IsCompleted && CurrentTrick.WinningPlay == play)
                        {
                            s += " Winner";
                        }

                        feed.Add(new Text(s));
                    }
                }
            }
            else
            {
                foreach (ActionTaken turn in Turns[Game.CurrentPhase])
                {
                    feed.Add(new Text(turn.ToString()));
                }
            }

            return new Rows(feed);
        }

        protected void Draw()
        {
            _console.Clear();

            var layout = new Layout("Root") { Size = 26 }
                .SplitRows(
                    new Layout("Score", new Text("SCORE")) { Size = 1 },
                    new Layout("Game") { Size = 25 }
                        .SplitColumns(
                            new Layout("Feed", DrawFeed()) { Ratio = 3 },
                            new Layout("Right") { Ratio = 7 }
                                .SplitRows(
                                    new Layout("Main", DrawBidding()) { Size = 22 },
                                    new Layout("Hand", DrawHand()) { Size = 3 })));



            _console.Write(layout);

            //     GameScore score = Game.GetScore();
            //     var roundScore = Game.GetRoundScore();

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
