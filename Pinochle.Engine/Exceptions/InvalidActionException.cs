using System;
using System.Collections.Generic;
using System.Text;

namespace JFadich.Pinochle.Engine.Exceptions
{
    public class InvalidActionException : PinochleRuleViolationException
    {
        public InvalidActionException(string message) : base(message)
        {

        }
        public InvalidActionException() : base("Invalid Player Action")
        {

        }
    }
}
