using Despegar.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business
{
    /// <summary>
    /// Specific MAPI API Error. Used for booking
    /// </summary>
    public class MAPIError : IAPIError
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<string> causes { get; set; }
        public string tracking { get; set; }

        public int ErrorCode
        {
            get { return this.code; }
        }

        public string Message
        {
            get { return this.message; }
        }
    }
}