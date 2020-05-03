using System;
using System.Collections.Generic;
using System.Text;
using Pinochle.Contracts;

namespace Pinochle
{
    public class GameFactory
    {
        public static IPinochleGame Make()
        {
            return new Game();
        }
    }
}
