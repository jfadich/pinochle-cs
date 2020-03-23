using System;
using System.Collections.Generic;
using Pinochle.Cards;
using Pinochle.Server;

namespace Pinochle
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConsolePinochle pinochle = new ConsolePinochle();

            //pinochle.Play();

            Server.Server server = new Server.Server();

            server.Start();
        }
    }
}
