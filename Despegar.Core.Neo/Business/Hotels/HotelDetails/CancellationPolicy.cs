using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.HotelDetails
{
    public class CancellationPolicy
    {
        public string penalty_description { get; set; }
        public int hours_before_penalty { get; set; }
        public string penalty_short_description { get; set; }
        public string status { get; set; }
    }
}
