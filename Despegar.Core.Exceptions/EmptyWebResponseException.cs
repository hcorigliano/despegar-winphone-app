using System;

namespace Despegar.Core.Exceptions
{
    /// <summary>
    /// Represents an error when a call to a service has an Empty Response
    /// </summary>
    public class EmptyWebResponseException : Exception
    {
        public EmptyWebResponseException()
        {            
        }

        public EmptyWebResponseException(string message)
            : base(message)
        {
        }

        public EmptyWebResponseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}