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
        private Regex[] patterns;

        public RegexHandEvaluator()
        {
            patterns = new Regex[]
            {
                new Regex(new RegexMeldTable(Card.Suits.Clubs).pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture),
                new Regex(new RegexMeldTable(Card.Suits.Hearts).pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture),
                new Regex(new RegexMeldTable(Card.Suits.Spades).pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture),
                new Regex(new RegexMeldTable(Card.Suits.Diamonds).pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture),
            };
        }

        public List<Meld> GetMeld(Hand hand, PinochleCard.Suits trump)
        {
            List<Meld> allMeld = new List<Meld>();
            string handString = cardsToString(hand.Cards);
            int index = (int)trump >> 4;
            MatchCollection matches = patterns[index].Matches(handString);
            string[] groupNames = patterns[index].GetGroupNames();

            foreach (Match match in matches)
            {
                string matchedCards = string.Empty;
                string name = string.Empty;
                List<string> fill = match.Groups["fill"].Captures.Select(c => c.Value).ToList();

                foreach (string group in groupNames)
                {
                    Group g = match.Groups[group];

                    if (g.Success && g.Name != "fill" && g.Name != "0")
                    {
                        matchedCards = g.Value;
                        name = g.Name;
                        break;
                    }
                }

                fill.ForEach(f => matchedCards = matchedCards.Replace(f, "."));
                
                Console.WriteLine("Found Group: '{0}' matched: {1}", name, matchedCards);
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
