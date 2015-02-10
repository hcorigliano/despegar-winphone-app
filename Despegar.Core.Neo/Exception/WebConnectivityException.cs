using System;

namespace Despegar.Core.Neo.Exceptions
{
    /// <summary>
    /// Represents an error in the HttpClient object when connecting to a service endpoint
    /// </summary>
    public class WebConnectivityException : Exception
    {
        public WebConnectivityException()
        {
        }

        public WebConnectivityException(string message)
            : base(message)
        {
        }

        public WebConnectivityException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}