using System;
using System.Collections.Generic;
using System.Text;

namespace JFadich.Pinochle.Engine.Cards
{
    public class PinochleDeck : Deck
    {
        new public static PinochleDeck Make()
        {
            PinochleDeck deck = new PinochleDeck();

            foreach (PinochleCard.Suits suit in Enum.GetValues(typeof(PinochleCard.Suits)))
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
