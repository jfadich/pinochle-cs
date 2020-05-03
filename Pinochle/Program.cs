using System;
using System.Collections.Generic;
using JFadich.Pinochle.Engine.Cards;

namespace JFadich.Pinochle.Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsolePinochle pinochle = new ConsolePinochle();

            pinochle.Play();
            Console.ReadLine();
        }
    }
}
