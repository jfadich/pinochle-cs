using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Exceptions
{
    class OutOfTurnException : Exception
    {
        public OutOfTurnException(string message) : base(message)
        {

        }
    }
}
