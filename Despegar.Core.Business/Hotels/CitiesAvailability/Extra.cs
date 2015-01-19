using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.CitiesAvailability
{
    public class Extra
    {
        public SearchedCity searched_city { get; set; }
        public NearbyCity nearby_city { get; set; }
        public bool final_result { get; set; }
    }
}
