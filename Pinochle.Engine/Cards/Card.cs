using System;
using System.Collections.Generic;
using System.Text;

namespace JFadich.Pinochle.Engine.Cards
{
    public class Card : IComparable<Card>
    {
        public enum Ranks : byte {
            Joker   = 0b00_00_0000,
            Ace     = 0b00_00_0001,
            Two     = 0b00_00_0010,
            Three   = 0b00_00_0011,
            Four    = 0b00_00_0100,
            Five    = 0b00_00_0101,
            Six     = 0b00_00_0110,
            Seven   = 0b00_00_0111,
            Eight   = 0b00_00_1000,
            Nine    = 0b00_00_1001,
            Ten     = 0b00_00_1010,
            Jack    = 0b00_00_1011,
            Queen   = 0b00_00_1100,
            King    = 0b00_00_1101,
        };

        public enum Suits : byte
        {
            Clubs       = 0b00_00_0000,
            Hearts      = 0b00_01_0000,
            Spades      = 0b00_10_0000,
            Diamonds    = 0b00_11_0000,
        };

        public enum Color : byte
        {
            Black   = 0b00_00_0000,
            Red     = 0b00_01_0000,
        };

        public enum Masks : byte
        {
            Rank    = 0b00_00_1111,
            Suit    = 0b00_11_0000,
            Color   = 0b00_01_0000
        };

        public byte Value { get; }

        public Card(byte value)
        {
            Value = value;
        }

        public Card(Ranks rank, Suits suit)
        {
            Value = MakeValue(rank, suit);
        }

        public static byte MakeValue(Ranks rank, Suits suit)
        {
            return (byte)((byte)rank + (byte)suit);
        }

        public Ranks getRank()
        {
            return (Ranks)(Value & (int)Masks.Rank);
        }

        public Suits getSuit()
        {
            return (Suits)(Value & (int)Masks.Suit);
        }
        public Color getColor()
        {
            return (Color)(Value & (int)Masks.Color);
        }

        public bool Equals(Card card)
        {
            return Value == card.Value;
        }

        public string GetName()
        {
            return String.Format("{0} of {1}", getRank(), getSuit());
        }
        public string GetShortName()
        {
            string rankSymbol;

            Ranks rank = getRank();

            switch (rank)
            {
                case Ranks.Ace:
                    rankSymbol = "A";
                    break;
                case Ranks.King:
                    rankSymbol = "K";
                    break;
                case Ranks.Queen:
                    rankSymbol = "Q";
                    break;
                case Ranks.Jack:
                    rankSymbol = "J";
                    break;
                default:
                    rankSymbol = ((int)rank).ToString();
                    break;
            }

            return String.Format("{0}{1}", rankSymbol, GetSuitShortName());
        }

        public string GetSuitShortName()
        {
            string suit = "";

            switch (getSuit())
            {
                case Suits.Spades:
                    suit = "♠";
                    break;
                case Suits.Clubs:
                    suit = "♣";
                    break;
                case Suits.Hearts:
                    suit = "♥";
                    break;
                case Suits.Diamonds:
                    suit = "♦";
                    break;
            }

            return suit;
        }

        public Boolean IsSuit(Card.Suits suit)
        {
            return getSuit() == suit;
        }

        public Boolean IsSuit(Card card)
        {
            return IsSuit(card.getSuit());
        }

        public override string ToString()
        {
            return GetShortName();
        }

        public override bool Equals(Object obj)
        {
            Card card = (Card)obj;

            return Value == card.Value;
        }

        public int CompareTo(Card other)
        {
            if (Value == other.Value)
                return 0;

            return Value > other.Value ? -1 : 1;
        }
    }
}
