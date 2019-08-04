using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Cards
{
    class PinochleDeck : Deck
    {
        new public static Deck Make()
        {
            Deck deck = new Deck();

            foreach (Card.Suits suit in Enum.GetValues(typeof(Card.Suits)))
            {
                foreach (PinochleCard.Ranks rank in Enum.GetValues(typeof(PinochleCard.Ranks)))
                {
                    deck.AddCard(new PinochleCard(rank, suit));
                    deck.AddCard(new PinochleCard(rank, suit));
                }
            }

            return deck;
        }
    }
}
