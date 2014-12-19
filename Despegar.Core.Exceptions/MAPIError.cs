using Despegar.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Exceptions
{
    /// <summary>
    /// Specific MAPI API Error. Used for booking
    /// </summary>
    public class MAPIError
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<string> causes { get; set; }
        public string tracking { get; set; }
    }
}