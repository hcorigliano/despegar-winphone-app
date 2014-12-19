using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Exceptions
{
    /// <summary>
    /// Represents an API Error which has a specific Error Code and a message. An abstraction of an API Error.
    /// </summary>
    public interface IAPIError
    {
        int ErrorCode { get; }
        string Message { get; }
    }
}