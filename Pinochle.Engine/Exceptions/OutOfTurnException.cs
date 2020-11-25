using System;
using System.Collections.Generic;
using System.Text;

namespace JFadich.Pinochle.Engine.Exceptions
{
    class OutOfTurnException : PinochleRuleViolationException
    {
        public OutOfTurnException(string message) : base(message)
        {

        }
        public OutOfTurnException() : base("It is not currently your turn")
        {

        }
    }
}
