using System;
using System.Collections.Generic;
using System.Text;
using JFadich.Pinochle.Engine.Cards;
using System.Linq;
using JFadich.Pinochle.Engine.Tricks;

namespace JFadich.Pinochle.Engine
{
    class HandEvaluator
    {
        private Hand Hand;

        private Card.Suits Trump;

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
                    List<PinochleCard> remainder = new List<PinochleCard>();
                    List<PinochleCard> found = new List<PinochleCard>();
                    foreach(PinochleCard card in Hand.Cards)
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
