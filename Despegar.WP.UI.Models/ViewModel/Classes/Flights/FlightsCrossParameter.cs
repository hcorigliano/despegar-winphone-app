﻿using Despegar.Core.Business.Flight.BookingCompletePostResponse;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.WP.UI.Model.Classes.Flights;
using System.Collections.Generic;

namespace Despegar.WP.UI.Model.ViewModel.Classes.Flights
{
    public class FlightsCrossParameter
    {
        public Route Inbound { get; set; }
        public Route Outbound { get; set; }
        public string FlightId { get; set; }
        public BookingCompletePostResponse BookingResponse { get; set; }
        public PriceFormated PriceDetail { get; set; }
        public int price { get; set; }
        public List<RoutesItems> MultipleRoutes { get; set; }
        
    }
}
