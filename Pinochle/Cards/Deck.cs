using System;
using System.Collections.Generic;
using JFadich.Pinochle.Engine.Contracts;

namespace JFadich.Pinochle.Engine.Cards
{
    public class Deck
    {
        private Random random = new Random();

        public List<Card> Cards { get; }

        public Deck(List<Card> cards)
        {
            this.Cards = cards;
        }

        public Deck()
        {
            this.Cards = new List<Card>();
        }

        public static Deck Make()
        {
            Deck deck = new Deck();

            foreach (Card.Suits suit in Enum.GetValues(typeof(Card.Suits)))
            {
                foreach (Card.Ranks rank in Enum.GetValues(typeof(Card.Ranks)))
                {
                    if(rank == Card.Ranks.Joker)
                    {
                        continue;
                    }

                    deck.AddCard(new Card(rank, suit));
                }
            }

            return deck;
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public Card DrawCard()
        {
            int index = Cards.Count - 1;

            Card card = Cards[index];
            Cards.RemoveAt(index);

            return card;
        }

        public IHand[] Deal(int numberOfHands)
        {
            return Deal(numberOfHands, (Cards.Count - (Cards.Count % numberOfHands)) / numberOfHands);
        }

        public IHand[] Deal(int numberOfHands, int perHand)
        {
            int dealCount = numberOfHands * perHand;

            if (dealCount > Cards.Count)
            {
                throw new Exception("Can't deal that many cards");
            }

            Hand[] hands = new Hand[numberOfHands];

            for (int hand = 0; hand < numberOfHands; hand++)
            {
                PinochleCard[] cards = new PinochleCard[perHand];

                for(int card = 0; card < perHand; card++)
                {
                    cards[card] = (PinochleCard)DrawCard();
                }

                hands[hand] = new Hand(cards);
            }

            return hands;
        }

        public Deck Shuffle()
        {
            for (var i = 0; i < Cards.Count - 1; i++)
            {
                int swapIndex = random.Next(i, Cards.Count);

                Card swappedCard = Cards[swapIndex];
                Cards[swapIndex] = Cards[i];
                Cards[i] = swappedCard;

            }

            return this;
        }
    }
}
