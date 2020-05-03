using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Exceptions
{
    class InvalidActionException : PinochleRuleViolationException
    {
        public InvalidActionException(string message) : base(message)
        {

        }
        public InvalidActionException() : base("Invalid Player Action")
        {

        }
    }
}
