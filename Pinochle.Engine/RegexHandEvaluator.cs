using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Pinochle.Engine
{
    internal class RegexHandEvaluator
    {
        private RegexMeldTable meldTable = new();

        public RegexHandEvaluator()
        {
        }

        public List<Meld> GetMeld(Hand hand, PinochleCard.Suits trump)
        {
            List<Meld> allMeld = new List<Meld>();
            string handString = cardsToString(hand.Cards);

            foreach (var meldPattern in meldTable.AllMeld)
            {
                /*if(Regex.IsMatch(handString, meldPattern))
                {
                //    allMeld.Add(meld);
                } */
                if(meldPattern.IsMatch(handString))
                {
                    Console.Write("M");
                }
            }

            return allMeld;
        }

        private string cardsToString(IEnumerable<PinochleCard> cards)
        {
            return string.Join(".", cards.Select(c => c.Value)) + ".";
        }

        // Preprocess and store

    }
}
