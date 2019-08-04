using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Cards
{
    class PinochleCard : Card
    {
        new public enum Ranks : byte
        {
            Ace = 0b00_00_0001,
            Nine = 0b00_00_1001,
            Ten = 0b00_00_1010,
            Jack = 0b00_00_1011,
            Queen = 0b00_00_1100,
            King = 0b00_00_1101,
        };

        public PinochleCard(byte value) : base(value)
        {

        }

        public PinochleCard(Ranks rank, Suits suit) : base((Card.Ranks)rank, suit)
        {

        }

        public bool IsLegOfPinochle()
        {
            return this.Equals(new PinochleCard(Ranks.Queen, Suits.Spades)) || this.Equals(new PinochleCard(Ranks.Jack, Suits.Diamonds));
        }
        public bool IsCounter()
        {
            Ranks rank = (Ranks)getRank();

            return rank == Ranks.Ace || rank == Ranks.Ten || rank == Ranks.King;
        }

        public int CompareTo(PinochleCard other)
        {
            if (value == other.Value)
                return 0;

            return value > other.Value ? -1 : 1;
        }
    }
}
