using System;
using System.Collections.Generic;
using Pinochle.Cards;
using System.Linq;

namespace Pinochle
{
    class Hand
    {
        public List<Card> Cards { get; }
        public Card[] DealtCards { get; }

        public Hand(Card[] cards)
        {
            Array.Sort(cards);
            DealtCards = cards;
            Cards = new List<Card>(cards);
        }

        public Boolean HasCards(Card[] needles)
        {
            List < Card > needlesList = new List<Card>(needles);
            return !needlesList.Where(needle => !Cards.Contains(needle)).Any();
        }

        public Card[] TakeCards(Card[] requestedCards)
        {
            if(requestedCards.Length > Cards.Count)
            {
                throw new Exception("Can't take more cards than are in the hand");
            }

            Card[] cards = new Card[requestedCards.Length];

            foreach(Card card in requestedCards)
            {

            }

            for(int i = 0; i < requestedCards.Length; i++)
            {
                Card card = requestedCards[i];

                if ( !Cards.Contains(card))
                {
                    throw new Exception(String.Format("Card '%s' is not in the hand", card));
                }

                cards[i] = card;
                Cards.Remove(card);
            }

            return cards;
        }

        public void GiveCards(Card[] cards)
        {
            Cards.AddRange(cards);
        }

        public override string ToString()
        {
            return String.Join(" , ", Cards);
        }
    }
}
