using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Exceptions
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