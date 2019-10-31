using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Exceptions
{
    class IllegalTrickException : Exception
    {
        public IllegalTrickException(string message) : base(message)
        {

        }
    }
}
