using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Exceptions
{
    class IllegalTrickException : PinochleRuleViolationException
    {
        public IllegalTrickException(string message) : base(message)
        {

        }
    }
}
