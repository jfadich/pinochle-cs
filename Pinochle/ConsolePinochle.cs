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

            Draw();
            Auction();
            Draw();
            CallTrump();
            Draw();
            PassCards();
            Draw();
            ShowMeld();
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

            Draw();

            pass = AskForCards();

            Game.PassCardsBack(pass);
        }

        public void ShowMeld()
        {
            Game.CalculateMeld();
            foreach(Player player in Game.Players)
            {

                List<Meld>  meld = Game.CurrentRound.MeldScore[player.Position];

                Console.WriteLine("{0} Meld. {1} Points", player, meld.Sum(score => score.GetValue()));
                Console.WriteLine(Game.CurrentRound.PlayerHand(player));
                foreach(Meld meldScore in meld)
                {
                    Console.WriteLine(meldScore);
                }
            }

            Console.WriteLine("Here");
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

            Console.WriteLine(string.Format("Phase: {0} | Player: {1}", Game.CurrentRound.Phase, Game.ActivePlayer));
            Console.WriteLine("----------------------------------------------------------------------------------");

            foreach(PhaseCompleted phase in Phases)
            {
                Console.WriteLine(phase);
            }

            Console.WriteLine("----------------------------------------------------------------------------------");

            foreach(PlayerTurn turn in Turns[Game.CurrentRound.Phase])
            {
                Console.WriteLine(turn);
            }

            Console.WriteLine("----------------------------------------------------------------------------------");
        }

        public void onPhaseCompleted(PhaseCompleted phaseCompleted)
        {
            Phases.Add(phaseCompleted);
            Console.WriteLine(phaseCompleted);
        }
        public void onTurnTaken(PlayerTurn playerTurn)
        {
            Turns[Game.CurrentRound.Phase].Add(playerTurn);
            Console.WriteLine(playerTurn);
        }
    }
}
