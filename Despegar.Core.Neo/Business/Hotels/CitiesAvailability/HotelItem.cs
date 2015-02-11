﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.CitiesAvailability
{
    public class HotelItem
    {
        public string id { get; set; }
        public Hotel hotel { get; set; }
        public Price price { get; set; }
        public string payment_description { get; set; }
        public decimal? distance { get; set; }
    }
}
