using Spectre.Console;
using System;

namespace JFadich.Pinochle.PlayConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new AnsiConsoleFactory();

            IAnsiConsole console = factory.Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Detect,
                ColorSystem = ColorSystemSupport.Detect,
                Out = new AnsiConsoleOutput(Console.Out),
            });

            ConsoleGame pinochle = new ConsoleGame(console);

            pinochle.Play();
            Console.ReadLine();
        }
    }
}
