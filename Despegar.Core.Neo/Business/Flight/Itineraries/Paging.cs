using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.Itineraries
{
    public class Paging
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public int total { get; set; }             
    }
}
