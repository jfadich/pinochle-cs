using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using JFadich.Pinochle.Engine.Cards;
using static JFadich.Pinochle.Engine.Cards.Card;

namespace JFadich.Pinochle.Engine
{
    public class RegexMeldTable
    {
        public readonly List<Regex> AllMeld = new();

        public string pattern;

        private PinochleCard.Suits Trump;

        public RegexMeldTable(PinochleCard.Suits trump)
        {
            this.Trump = trump;
            /*List<Meld> meld = new List<Meld>
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
            } */

            pattern = @$"(?<pinochle>{Pinochle()})|";

            pattern += @$"(?<dix>{Dix(trump)})|";
            pattern += @$"(?<run>{Run(trump)})|";


            foreach (PinochleCard.Suits suit in Enum.GetValues(typeof(PinochleCard.Suits)))
            {
                if (suit != trump)
                {
                    pattern += @$"(?<marriage{suit}>{Marriage(suit)})|";
                }
            }

            pattern += @$"(?<fourJacks>{FourOfAKind(PinochleCard.Ranks.Jack)})|";
            pattern += @$"(?<fourQueens>{FourOfAKind(PinochleCard.Ranks.Queen)})|";
            pattern += @$"(?<fourKings>{FourOfAKind(PinochleCard.Ranks.King)})|";
            pattern += @$"(?<fourAces>{FourOfAKind(PinochleCard.Ranks.Ace)})";
        }

        private static string getPattern(IEnumerable<byte> cards)
        {
            return string.Join(@"(?'fill'(?:\.\d{1,2})*\.)+", cards);
        }

        public static string Pinochle()
        {
            return getPattern(new[] {
                PinochleCard.MakeValue(Card.Ranks.Jack, PinochleCard.Suits.Diamonds),
                PinochleCard.MakeValue(Card.Ranks.Queen, PinochleCard.Suits.Spades)
            });
        }

        public static string Run(PinochleCard.Suits suit)
        {
            byte[] cards = new[]
            {
                PinochleCard.MakeValue(Card.Ranks.Ace, suit),
                PinochleCard.MakeValue(Card.Ranks.Ten, suit),
                PinochleCard.MakeValue(Card.Ranks.King, suit),
                PinochleCard.MakeValue(Card.Ranks.Queen, suit),
                PinochleCard.MakeValue(Card.Ranks.Jack, suit)
            };

            var patterns = new[]
            {
                $@"{cards[0]}",
                $@"{cards[1]}(?'fill'\.{cards[1]})?",
                $@"{cards[2]}(?'extra'\.{cards[2]})?",
                $@"{cards[3]}(?'extra'\.{cards[3]})?",
                $@"{cards[4]}",
            };

            return string.Join(@"\.", patterns);
        }

        public static string Marriage(PinochleCard.Suits suit)
        {
            return $@"{PinochleCard.MakeValue(Card.Ranks.King, suit)}\.{PinochleCard.MakeValue(Card.Ranks.Queen, suit)}";
        }

        public static string FourOfAKind(PinochleCard.Ranks rank)
        {
            return getPattern(new[]
            {
                PinochleCard.MakeValue((Ranks)rank, PinochleCard.Suits.Diamonds),
                PinochleCard.MakeValue((Ranks)rank, PinochleCard.Suits.Spades),
                PinochleCard.MakeValue((Ranks)rank, PinochleCard.Suits.Hearts),
                PinochleCard.MakeValue((Ranks)rank, PinochleCard.Suits.Clubs)
            });
        }

        public static string Dix(PinochleCard.Suits trump)
        {
            byte nineOfTrump = PinochleCard.MakeValue(Card.Ranks.Nine, trump);

            return @$"{nineOfTrump}(\.{nineOfTrump})?";
        }
    }
}