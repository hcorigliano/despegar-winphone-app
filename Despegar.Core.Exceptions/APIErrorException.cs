using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Exceptions
{
    public class APIErrorException : Exception
    {
        public IAPIError ErrorData { get; set; }

         public APIErrorException()
        {            
        }

        public APIErrorException(string message)
            : base(message)
        {
        }

        public APIErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}