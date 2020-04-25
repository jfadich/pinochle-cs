using System;
using System.Collections.Generic;
using System.Text;
using Pinochle.Events.Turns;
using Pinochle.Events.Phases;
using System.Linq;

namespace Pinochle
{
    class ConsolePinochle
    {
        public Dictionary<Round.Phases, List<PlayerTurn>> Turns;
        public List<PhaseCompleted> Phases;

        protected Game Game;

        public void Play()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Game = new Game();
            Turns = new Dictionary<Round.Phases, List<PlayerTurn>>();
            Phases = new List<PhaseCompleted>();
            
            foreach(Round.Phases phase in Enum.GetValues(typeof(Round.Phases)))
            {
                Turns.Add(phase, new List<PlayerTurn>());
            }

            Game.PhaseCompleted += onPhaseCompleted;
            Game.TurnTaken += onTurnTaken;
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
            while (Game.IsCurrently(Round.Phases.Bidding))
            {
                DrawHand();
                Console.Write(String.Format("What does {0} bid? ", Game.ActivePlayer));
                string bid = Console.ReadLine();

                try
                {
                    if (bid == "pass")
                    {
                        Game.PassBid();
                        Draw();
                        continue;
                    }

                    Game.PlaceBid(int.Parse(bid));
                    Draw();

                }
                catch (Exceptions.PinochleRuleViolationException exception)
                {
                    Console.WriteLine(exception.Message);
                }
                catch (FormatException exception)
                {
                    Console.WriteLine("Please enter bid as a number");
                }

            }
        }

        protected void CallTrump()
        {
            Cards.Card.Suits? trump = null;
            do
            {
                DrawHand();
                Console.WriteLine(Game.ActivePlayer + " pease select trump [c,h,s,d]");

                string providedTrump = Console.ReadLine();
                providedTrump.Trim();
                providedTrump.ToLower();

                switch (providedTrump)
                {
                    case "c":
                    case "clubs":
                        trump = Cards.Card.Suits.Clubs;
                        break;
                    case "h":
                    case "hearts":
                        trump = Cards.Card.Suits.Hearts;
                        break;
                    case "s":
                    case "spades":
                        trump = Cards.Card.Suits.Spades;
                        break;
                    case "d":
                    case "diamonds":
                        trump = Cards.Card.Suits.Diamonds;
                        break;
                }
            } while (trump == null);

            Game.CallTrump((Cards.Card.Suits)trump);
        }

        protected void PassCards()
        {
            Cards.PinochleCard[] pass = AskForCards("select 4 cards pass to your partner", 4);

            Game.PassCardsToLeader(pass);

            Draw();

            pass = AskForCards("select 4 cards pass back to your partner", 4);

            Game.PassCardsBack(pass);
        }

        public void ShowMeld()
        {
            foreach(Seat player in Game.AllPlayers())
            {

                List<Meld>  meld = Game.GetPlayerMeld(player);

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
            Game.StartTricks();

            Cards.PinochleCard trick = AskForACard(" Play a trick.");

            Game.PlayTrick(trick);
            Draw();

            while(Game.IsCurrently(Round.Phases.Playing))
            {

                trick = AskForACard(" Play a trick.");

                try
                {
                    Game.PlayTrick(trick);
                    Draw();
                } catch(Exceptions.IllegalTrickException e)
                {
                    Draw();
                    Console.WriteLine(e.Message);
                }
            }
        }

        protected Cards.PinochleCard AskForACard(string message)
        {
            return AskForCards(message, 1)[0];
        }

        protected Cards.PinochleCard[] AskForCards(string message, int numberOfCards)
        {
            Hand hand = Game.GetPlayerHand();

            int j = 0;
            foreach (Cards.PinochleCard card in hand.Cards)
            {
                bool canPlay = true;
                Console.ResetColor();

                if (card.getColor() == Cards.PinochleCard.Color.Red)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (Game.IsCurrently(Round.Phases.Playing))
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
            Cards.PinochleCard[] selectedCards;

            do
            {
                string cardsInput = Console.ReadLine();
                string[] cards = cardsInput.Split(" ");
                selectedCards = new Cards.PinochleCard[numberOfCards];

                if (cards.Length < numberOfCards)
                {
                    continue;
                }

                for (int i = 0; i < numberOfCards; i++)
                {
                    try
                    {
                        int cardIndex = int.Parse(cards[i]);
                        if (cardIndex > hand.Cards.Count - 1)
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
                    catch (Exception e)
                    {
                        inputValid = false;
                    }
                }

            } while (!inputValid);

            return selectedCards;
        }

        protected void DrawHand()
        {
            Console.ResetColor();
            foreach (Cards.PinochleCard card in Game.GetPlayerHand().Cards)
            {
                if (card.getColor() == Cards.PinochleCard.Color.Red)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(card + " " );
            }
            Console.ResetColor();
            Console.WriteLine();

            
        }

        protected void Draw()
        {
            Console.Clear();
            Console.ResetColor();

            int[] scores = Game.GetScores();
            Console.WriteLine(string.Format("Phase: {0} | Current Player: {1} | Team 1 Score {2} | Team 0 Score {3}", Game.CurrentPhase(), Game.ActivePlayer, scores[0], scores[1]));
            Console.WriteLine("----------------------------------------------------------------------------------");

            foreach(PhaseCompleted phase in Phases)
            {
                Console.WriteLine(phase);
            }

            Console.WriteLine("----------------------------------------------------------------------------------");

            foreach(PlayerTurn turn in Turns[Game.CurrentPhase()])
            {
                Console.WriteLine(turn);
            }

            Console.WriteLine("----------------------------------------------------------------------------------");
        }

        public void onPhaseCompleted(PhaseCompleted phaseCompleted)
        {
            if(phaseCompleted is PlayingCompleted)
            {
                foreach (Round.Phases phase in Enum.GetValues(typeof(Round.Phases)))
                {
                    Turns[phase].Clear();
                }
            }

            Phases.Add(phaseCompleted);
            Console.WriteLine(phaseCompleted);
        }
        public void onTurnTaken(PlayerTurn playerTurn)
        {
            Turns[Game.CurrentPhase()].Add(playerTurn);
            Console.WriteLine(playerTurn);
        }
    }
}
