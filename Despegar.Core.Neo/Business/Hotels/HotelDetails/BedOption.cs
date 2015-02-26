using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.HotelDetails
{
    public class BedOption
    {
        public string choice { get; set; }
        public string description { get; set; }
        public List<Layout> layout { get; set; }
        public bool Selected { get; set; }
    }
}
