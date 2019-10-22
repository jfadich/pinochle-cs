using System;
using System.Collections.Generic;
using System.Text;
using Pinochle.Cards;

namespace Pinochle
{
    class MeldTable
    {
        public readonly List<Meld> AllMeld;

        public MeldTable(PinochleCard.Suits trump)
        {
            AllMeld = new List<Meld>
            {
                new Meld(Dix(trump), 10, 20,"Dix"),
                new Meld(Marriage(trump), 40, 80, "Royal Marriage"),
                new Meld(Run(trump), 150, 1500, "Run"),
                new Meld(Pinochle(), 40, 300, "Pinochle"),
                new Meld(FourOfAKind(PinochleCard.Ranks.Jack), 40, 400, "Jacks"),
                new Meld(FourOfAKind(PinochleCard.Ranks.Queen), 60, 600, "Queens"),
                new Meld(FourOfAKind(PinochleCard.Ranks.King), 80, 800, "Kings"),
                new Meld(FourOfAKind(PinochleCard.Ranks.Ace), 100, 1000, "Aces"),

            };

            foreach(PinochleCard.Suits suit in Enum.GetValues(typeof(PinochleCard.Suits)))
            {
                if(suit != trump)
                {
                    AllMeld.Add(new Meld(Marriage(suit), 20, 40, "Marriage"));
                }
            }
        }

        public static PinochleCard[] Pinochle()
        {
            return new[] {
                new PinochleCard(PinochleCard.Ranks.Jack, PinochleCard.Suits.Diamonds),
                new PinochleCard(PinochleCard.Ranks.Queen, PinochleCard.Suits.Spades)
            };
        }

        public static PinochleCard[] Run(PinochleCard.Suits suit)
        {
            return new[]
            {
                new PinochleCard(PinochleCard.Ranks.Ace, suit),
                new PinochleCard(PinochleCard.Ranks.Ten, suit),
                new PinochleCard(PinochleCard.Ranks.King, suit),
                new PinochleCard(PinochleCard.Ranks.Queen, suit),
                new PinochleCard(PinochleCard.Ranks.Jack, suit)
            };
        }
        public static PinochleCard[] Marriage(PinochleCard.Suits suit)
        {
            return new[]
            {
                new PinochleCard(PinochleCard.Ranks.King, suit),
                new PinochleCard(PinochleCard.Ranks.Queen, suit)
            };
        }

        public static PinochleCard[] FourOfAKind(PinochleCard.Ranks rank)
        {
            return new[]
            {
                new PinochleCard(rank, PinochleCard.Suits.Clubs),
                new PinochleCard(rank, PinochleCard.Suits.Diamonds),
                new PinochleCard(rank, PinochleCard.Suits.Hearts),
                new PinochleCard(rank, PinochleCard.Suits.Spades)
            };
        }

        public static PinochleCard[] Dix(PinochleCard.Suits trump)
        {
            return new[]
            {
                new PinochleCard(PinochleCard.Ranks.Nine, trump),
            };
        }
    }
}
