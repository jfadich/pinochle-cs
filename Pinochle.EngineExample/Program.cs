using Spectre.Console;
using System;
using System.Text;

namespace JFadich.Pinochle.PlayConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            IAnsiConsole console = AnsiConsole.Create(new AnsiConsoleSettings
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
