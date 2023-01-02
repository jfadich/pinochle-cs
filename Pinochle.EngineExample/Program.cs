using System;

namespace JFadich.Pinochle.PlayConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleGame pinochle = new ConsoleGame();

            pinochle.Play();
            Console.ReadLine();
        }
    }
}
