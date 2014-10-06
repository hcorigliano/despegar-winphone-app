using System;

namespace Despegar.Core.Exceptions
{
    /// <summary>
    /// Represents an error when a HTTP Response has an Error Code (I.E: 404 Not found)
    /// </summary>
   public class HTTPStatusErrorException : Exception
    {
        public HTTPStatusErrorException()
        {
        }

        public HTTPStatusErrorException(string message)
            : base(message)
        {
        }

        public HTTPStatusErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}