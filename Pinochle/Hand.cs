using System;
using System.Collections.Generic;
using Pinochle.Cards;
using System.Text;

namespace Pinochle
{
    class Hand
    {
        public List<Card> Cards { get; }
        public Card[] DealtCards { get; }

        public Hand(Card[] cards)
        {
            Array.Sort(cards);
            this.DealtCards = cards;
            this.Cards = new List<Card>(cards);
        }

        public Card[] TakeCards(int[] cardIndexes)
        {
            if(cardIndexes.Length > Cards.Count)
            {
                throw new Exception("Can't take more cards than are in the hand");
            }

            Card[] cards = new Card[cardIndexes.Length];

            for(int i = 0; i < cardIndexes.Length; i++)
            {
                cards[i] = Cards[cardIndexes[i]];
            }

            for (int i = 0; i < cardIndexes.Length; i++)
            {
                Cards.RemoveAt(cardIndexes[i]);
            }

            return cards;
        }

        public override string ToString()
        {
            return String.Join(" , ", Cards);
        }
    }
}
