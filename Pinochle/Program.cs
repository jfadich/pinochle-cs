using System;
using System.Collections.Generic;
using Pinochle.Cards;

namespace Pinochle
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsolePinochle pinochle = new ConsolePinochle();

            pinochle.Play();
        }
    }
}
