﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Flight.BookingFields
{
    public class FlightsBookingFieldRequest
    {
        public string itinerary_id { get; set; }
        public int ? outbound_choice { get; set; }
        public int ? inbound_choice { get; set; }
        public string mobile_identifier { get; set; }

        // UPA parameter
        [JsonIgnore]
        public int SelectedItemIndex { get; set; }
    }
}