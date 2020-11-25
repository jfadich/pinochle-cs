using System;
using System.Collections.Generic;
using System.Text;

namespace JFadich.Pinochle.Engine.Exceptions
{
    public class IllegalTrickException : PinochleRuleViolationException
    {
        public IllegalTrickException(string message) : base(message)
        {

        }
    }
}
