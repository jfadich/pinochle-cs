using System;
using System.Collections.Generic;
using System.Text;
using Pinochle.Cards;
using System.Linq;

namespace Pinochle
{
    class HandEvaluator
    {
        protected Hand Hand;

        protected Card.Suits Trump;

        public HandEvaluator(Hand hand, Card.Suits trump)
        {
            Hand = hand;
            Trump = trump;
        }

        public List<Meld> GetMeld()
        {
            List<Meld> allMeld = new List<Meld>();
            MeldTable meldTable = new MeldTable(Trump);

            foreach(Meld meld in meldTable.AllMeld)
            {
                if(Hand.HasCards(meld.Cards.ToArray()))
                {
                    List<Card> remainder = new List<Card>();
                    List<Card> found = new List<Card>();
                    foreach(Card card in Hand.Cards)
                    {
                        if( meld.Cards.Contains(card) && ! found.Contains(card)) {
                            found.Add(card);
                        } else
                        {
                            remainder.Add(card);
                        }
                    }
                    if( !remainder.Except(meld.Cards).Any())
                    {
                        meld.IsDoubled = true;
                    }

                    allMeld.Add(meld);
                }
            }

            return allMeld;
        }
    }
}
