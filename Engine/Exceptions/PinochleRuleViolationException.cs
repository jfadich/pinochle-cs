using System;
using System.Collections.Generic;
using System.Text;

namespace JFadich.Pinochle.Engine.Exceptions
{
    class PinochleRuleViolationException : Exception
    {
        public PinochleRuleViolationException(string message) : base(message)
        {

        }
    }
}
