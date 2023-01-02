using System;
using System.Collections.Generic;
using System.Text;
using JFadich.Pinochle.Engine.Contracts;

namespace JFadich.Pinochle.Engine
{
    public class GameFactory
    {
        public static IPinochleGame Make()
        {
            return new Game();
        }
    }
}
