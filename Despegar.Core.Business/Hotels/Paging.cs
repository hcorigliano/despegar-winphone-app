using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels
{
    public class Paging
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public int total { get; set; }
    }
}
