using BenchmarkDotNet.Running;
using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Engine.Contracts;
using Pinochle.Engine;
using Pinochle.EngineExample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace JFadich.Pinochle.PlayConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex.CacheSize += 100;
            //ConsoleGame pinochle = new ConsoleGame();

            //pinochle.Play();
            var summary = BenchmarkRunner.Run<RegexBenchmark>();
            Console.ReadLine();
        }

        static void Test()
        {
            var hands = GetHands();
            int match = 0;

            foreach (var h in hands)
            {
                var iteratorEval = new HandEvaluator((Hand)h, Card.Suits.Spades);
                var regexEval = new RegexHandEvaluator();

                var itMeld = iteratorEval.GetMeld();
                var reMeld = regexEval.GetMeld((Hand)h, Card.Suits.Spades);


                if (itMeld.Count == reMeld.Count && itMeld.SequenceEqual(reMeld))
                {
                    match++;
                }
                else
                {
                    Console.WriteLine("Hand: " + CardString(h.Cards));

                    Console.WriteLine("Iterative Meld:");
                    foreach (var meld in itMeld)
                    {
                        Console.WriteLine(string.Join(',', meld.Cards.ToList()));
                    }

                    Console.WriteLine("Regex Meld:");
                    foreach (var meld in reMeld)
                    {
                        Console.WriteLine(string.Join(',', meld.Cards.ToList()));
                    }
                }
            }

            Console.WriteLine($"Success {match}/{hands.Count}");
        }


        private static List<IHand> GetHands(int decks = 100000)
        {
            List<IHand> hands = new List<IHand>();
            for(int i =0; i < decks; i++)
            {
                PinochleDeck deck = PinochleDeck.Make();
                hands.AddRange(deck.Shuffle().Deal(4));
            }

            return hands;
        }

        static string CardString(PinochleCard[] cards) => string.Join(',', cards.ToList());
    }
    
}
