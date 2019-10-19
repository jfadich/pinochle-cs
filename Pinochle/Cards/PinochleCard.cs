using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Cards
{
    class PinochleCard : Card
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

        // @todo order this by pinochle rank
        public int CompareTo(PinochleCard other)
        {
            if (value == other.Value)
                return 0;

            return value > other.Value ? -1 : 1;
        }
    }
}
