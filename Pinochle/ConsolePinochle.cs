using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle
{
    class ConsolePinochle
    {
        public List<string> Turns;
        public List<string> PhaseMessages;

        protected Game Game;

        public void Play()
        {
            Game = new Game();
            Turns = new List<string>();
            PhaseMessages = new List<string>();

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
            Hand hand = Game.GetPlayerHand();

            int j = 0;
            foreach(Cards.PinochleCard card in hand.Cards)
            {
                Console.WriteLine(String.Format("{0}: {1}",j, card.GetName() ));
                j++;
            }
            Console.WriteLine(Game.ActivePlayer + " select 4 cards pass to your partner");
            Console.WriteLine(Game.GetPlayerHand());

            bool inputValid = false;
            int[] cardPositions;
            do
            {
                string cardsInput = Console.ReadLine();
                string[] cards = cardsInput.Split(" ");
                cardPositions = new int[4];

                if(cards.Length < 4)
                {
                    continue;
                }

                for(int i = 0; i < 4; i++)
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
                            cardPositions[i] = cardIndex;
                            inputValid = true; 
                        }
                    }
                    catch (Exception e)
                    {
                        inputValid = false;
                    }
                }

            } while (!inputValid);

            Console.WriteLine(cardPositions);
        }

        protected void Draw()
        {
            Console.Clear();

            foreach(string message in PhaseMessages)
            {
                Console.WriteLine(message);
            }
        }

        public void onPhaseCompleted(string message)
        {
            PhaseMessages.Add(message);
            Console.WriteLine(message);
        }
        public void onTurnTaken(string message)
        {
            Turns.Add(message);
            Console.WriteLine(message);
        }
    }
}
