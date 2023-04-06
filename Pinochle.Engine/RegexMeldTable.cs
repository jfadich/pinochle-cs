using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JFadich.Pinochle.Engine.Cards;

namespace JFadich.Pinochle.Engine
{
    class RegexMeldTable
    {
        public readonly List<Regex> AllMeld = new();

        public RegexMeldTable()
        {
            List<Meld> meld = new List<Meld>
            {
                new Meld(Pinochle(), 4, 30, "Pinochle"),
                new Meld(FourOfAKind(PinochleCard.Ranks.Jack), 4, 40, "Jacks"),
                new Meld(FourOfAKind(PinochleCard.Ranks.Queen), 6, 60, "Queens"),
                new Meld(FourOfAKind(PinochleCard.Ranks.King), 8, 80, "Kings"),
                new Meld(FourOfAKind(PinochleCard.Ranks.Ace), 10, 100, "Aces"),
                new Meld(Dix(PinochleCard.Suits.Spades), 1, 2,"Dix"),
                new Meld(Marriage(PinochleCard.Suits.Spades), 4, 8, "Royal Marriage"),
                new Meld(Run(PinochleCard.Suits.Spades), 15, 150, "Run"),
                new Meld(Dix(PinochleCard.Suits.Diamonds), 1, 2,"Dix"),
                new Meld(Marriage(PinochleCard.Suits.Diamonds), 4, 8, "Royal Marriage"),
                new Meld(Run(PinochleCard.Suits.Diamonds), 15, 150, "Run"),
                new Meld(Dix(PinochleCard.Suits.Clubs), 1, 2,"Dix"),
                new Meld(Marriage(PinochleCard.Suits.Clubs), 4, 8, "Royal Marriage"),
                new Meld(Run(PinochleCard.Suits.Clubs), 15, 150, "Run"),
                new Meld(Dix(PinochleCard.Suits.Hearts), 1, 2,"Dix"),
                new Meld(Dix(PinochleCard.Suits.Hearts), 4, 8, "Royal Marriage"),
                new Meld(Dix(PinochleCard.Suits.Hearts), 15, 150, "Run"),
            };

            foreach(PinochleCard.Suits suit in Enum.GetValues(typeof(PinochleCard.Suits)))
            {
              //  if(suit != trump) todo fix royal marriage
              //  {
                    meld.Add(new Meld(Marriage(suit), 2, 4, "Marriage"));
              //  }
            }

            foreach(var m in meld)
            {
                AllMeld.Add(new Regex(getPattern(m.Cards), RegexOptions.Compiled));
            }
        }

        private string getPattern(IEnumerable<PinochleCard> cards)
        {
            return string.Join(@"(\.[0-9]+)*\.", cards.Select(c => c.Value)) + "\\.";
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
                new PinochleCard(rank, PinochleCard.Suits.Diamonds),
                new PinochleCard(rank, PinochleCard.Suits.Spades),
                new PinochleCard(rank, PinochleCard.Suits.Hearts),
                new PinochleCard(rank, PinochleCard.Suits.Clubs)
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