using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Exceptions
{
    class PinochleRuleViolationException : Exception
    {
        public PinochleRuleViolationException(string message) : base(message)
        {

        }
    }
}
