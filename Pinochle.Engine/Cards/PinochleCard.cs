using System;
using System.Collections.Generic;
using System.Text;

namespace JFadich.Pinochle.Engine.Cards
{
    public class PinochleCard : Card, IComparable<PinochleCard>
    {
        new public enum Ranks : byte
        {
            Ace = Card.Ranks.Ace,
            Nine = Card.Ranks.Nine,
            Ten = Card.Ranks.Ten,
            Jack = Card.Ranks.Jack,
            Queen = Card.Ranks.Queen,
            King = Card.Ranks.King,
        };

        public PinochleCard(byte value) : base(value) { }

        public PinochleCard(Ranks rank, Suits suit) : base((Card.Ranks)rank, suit) { }

        public bool IsLegOfPinochle()
        {
            return this.Equals(new PinochleCard(Ranks.Queen, Suits.Spades)) || this.Equals(new PinochleCard(Ranks.Jack, Suits.Diamonds));
        }

        public bool IsCounter()
        {
            Ranks rank = (Ranks)getRank();

            return rank == Ranks.Ace || rank == Ranks.Ten || rank == Ranks.King;
        }

        public int GetPinochleValue()
        {
            if(getRank() == Card.Ranks.Ace)
            {
                return Value + 14;
            }

            if(getRank() == Card.Ranks.Ten)
            {
                return Value + 4;
            }

            return Value;
        }

        // @todo order this by pinochle rank
        public int CompareTo(PinochleCard other)
        {
            if (GetPinochleValue() == other.GetPinochleValue())
                return 0;

            return GetPinochleValue() > other.GetPinochleValue() ? -1 : 1;
        }
    }
}
