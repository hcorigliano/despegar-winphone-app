using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.CitiesAvailability
{
    public class Item
    {
        public Hotel hotel { get; set; }
        public object price { get; set; }
        public City city { get; set; }
    }
}
