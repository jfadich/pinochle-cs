using System;
using System.Collections.Generic;
using System.Text;

namespace JFadich.Pinochle.Engine.Exceptions
{
    class IllegalTrickException : PinochleRuleViolationException
    {
        public IllegalTrickException(string message) : base(message)
        {

        }
    }
}
