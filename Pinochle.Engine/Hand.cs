using System;
using System.Collections.Generic;
using JFadich.Pinochle.Engine.Cards;
using System.Linq;
using JFadich.Pinochle.Engine.Contracts;

namespace JFadich.Pinochle.Engine
{
    class Hand : IHand
    {
        public PinochleCard[] Cards { get => _cards.ToArray(); }

        private List<PinochleCard> _cards { get; }

        public PinochleCard[] DealtCards { get; }

        public Hand(PinochleCard[] cards)
        {
            Array.Sort(cards);
            DealtCards = cards;
            _cards = new List<PinochleCard>(cards);
        }

        public bool HasCards(PinochleCard[] needles)
        {
            List <PinochleCard> needlesList = new List<PinochleCard>(needles);
            return !needlesList.Where(needle => !_cards.Contains(needle)).Any();
        }

        public PinochleCard TakeCard(PinochleCard requestedCard)
        {
            if (!Cards.Contains(requestedCard))
            {
                throw new Exception(String.Format("Card '%s' is not in the hand", requestedCard));
            }

            _cards.Remove(requestedCard);

            return requestedCard;
        }

        public PinochleCard[] TakeCards(PinochleCard[] requestedCards)
        {
            if(requestedCards.Length > _cards.Count)
            {
                throw new Exception("Can't take more cards than are in the hand");
            }

            PinochleCard[] cards = new PinochleCard[requestedCards.Length];


            for(int i = 0; i < requestedCards.Length; i++)
            {
                cards[i] = TakeCard(requestedCards[i]);
            }

            return cards;
        }

        public void GiveCards(PinochleCard[] cards)
        {
            _cards.AddRange(cards);
            _cards.Sort();
        }

        public override string ToString()
        {
            return String.Join(" , ", _cards);
        }
    }
}
