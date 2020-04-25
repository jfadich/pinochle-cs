using System;
using System.Collections.Generic;
using System.Text;
using Pinochle.Cards;

namespace Pinochle
{
    public class Meld
    {
        private int Value;

        private int DoubleValue;

        public List<PinochleCard> Cards;

        public string Name;

        public Boolean IsDoubled = false;

        public Meld(PinochleCard[] cards, int value, int doubleValue, string name)
        {
            Cards = new List<PinochleCard>(cards);
            Value = value;
            Name = name;
            DoubleValue = doubleValue;
        }

        public PinochleCard[] Double()
        {
            List<PinochleCard> doubled = new List<PinochleCard>(Cards);

            doubled.AddRange(Cards);

            return doubled.ToArray();
        }

        public int GetHashCode()
        {
            int code = 0;

            for (int i = 0; i < Cards.Count; i++)
            {
                code ^= Cards[i].Value;
                code <<= IsDoubled ? 2 : 1;
            }

            return code;
        }

        public int GetValue()
        {
            return IsDoubled ? DoubleValue : Value;
        }

        public override string ToString()
        {
            PinochleCard[] cards = IsDoubled ? Double() : Cards.ToArray();

            return String.Join(" , ", (object[])cards);
        }
    }
}
