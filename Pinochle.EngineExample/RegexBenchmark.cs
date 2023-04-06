using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Engine.Contracts;
using JFadich.Pinochle.Engine;
using Pinochle.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Pinochle.EngineExample
{
    public class RegexBenchmark
    {
        private List<IHand> hands { get; set; }

        private RegexHandEvaluator regexEvalHandEvaluator { get; set; }

        public RegexBenchmark() 
        {
            hands = GetHands();
            regexEvalHandEvaluator = new RegexHandEvaluator();
        }

        [Benchmark(Baseline = true)]
        public void Iterative()
        {
            foreach (var h in hands)
            {
                var iteratorEval = new HandEvaluator((Hand)h, Card.Suits.Spades);

                var itMeld = iteratorEval.GetMeld();
            }
        }

        [Benchmark]
        public void Regex()
        {
            foreach (var h in hands)
            {
                var reMeld = regexEvalHandEvaluator.GetMeld((Hand)h, Card.Suits.Spades);
            }
        }


        private static List<IHand> GetHands(int decks = 100)
        {
            List<IHand> hands = new List<IHand>();
            for (int i = 0; i < decks; i++)
            {
                PinochleDeck deck = PinochleDeck.Make();
                hands.AddRange(deck.Shuffle().Deal(4));
            }

            return hands;
        }
    }
}
