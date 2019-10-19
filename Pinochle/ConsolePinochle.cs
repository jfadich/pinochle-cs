using System;
using System.Collections.Generic;
using System.Text;
using Pinochle.Events.Turns;
using Pinochle.Events.Phases;

namespace Pinochle
{
    class ConsolePinochle
    {
        public List<PlayerTurn> Turns;
        public List<PhaseCompleted> PhaseMessages;

        protected Game Game;

        public void Play()
        {
            Game = new Game();
            Turns = new List<PlayerTurn>();
            PhaseMessages = new List<PhaseCompleted>();

            Game.PhaseCompleted += onPhaseCompleted;
            Game.TurnTaken += onTurnTaken;
            Game.StartGame();

            Auction();
            Draw();
            CallTrump();
            Draw();
            PassCards();
        }

        protected void Auction()
        {
            while (Game.IsCurrently(Round.Phases.Bidding))
            {
                Console.WriteLine(Game.GetPlayerHand());
                Console.Write(String.Format("What does {0} bid? ", Game.ActivePlayer));
                string bid = Console.ReadLine();

                try
                {
                    if (bid == "pass")
                    {
                        Game.PassBid();
                        continue;
                    }

                    Game.PlaceBid(int.Parse(bid));

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
                Console.WriteLine(Game.GetPlayerHand());
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
            Cards.Card[] pass = AskForCards();

            Game.PassCardsToLeader(pass);

            pass = AskForCards();

            Game.PassCardsBack(pass);
        }

        protected Cards.Card[] AskForCards()
        {
            Hand hand = Game.GetPlayerHand();

            int j = 0;
            foreach (Cards.PinochleCard card in hand.Cards)
            {
                Console.WriteLine(String.Format("{0}: {1}", j, card.GetName()));
                j++;
            }
            Console.WriteLine(Game.ActivePlayer + " select 4 cards pass to your partner");
            Console.WriteLine(Game.GetPlayerHand());

            bool inputValid = false;
            Cards.Card[] selectedCards;

            do
            {
                string cardsInput = Console.ReadLine();
                string[] cards = cardsInput.Split(" ");
                selectedCards = new Cards.Card[4];

                if (cards.Length < 4)
                {
                    continue;
                }

                for (int i = 0; i < 4; i++)
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
                            selectedCards[i] = hand.Cards[i];
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

        protected void Draw()
        {
            Console.Clear();

            foreach(PhaseCompleted phase in PhaseMessages)
            {
                Console.WriteLine(phase);
            }
        }

        public void onPhaseCompleted(PhaseCompleted phaseCompleted)
        {
            PhaseMessages.Add(phaseCompleted);
            Console.WriteLine(phaseCompleted);
        }
        public void onTurnTaken(PlayerTurn playerTurn)
        {
            Turns.Add(playerTurn);
            Console.WriteLine(playerTurn);
        }
    }
}
