using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Connector.Domain.API;

namespace Despegar.LegacyCore.Model
{
    public static class LastFlightBookData
    {
        public static FlightBookingBook LastBookResponse { get; set; }
        public static FlightsAvailabilityModel AvailabilityModel { get; set; }
        public static FlightAvailabilityItem AvailabilityInfo { get; set; }
        public static List<FlightPassengerDefinition> PassengerDefinitions { get; set; }
        public static FlightCardDefinition CardDefinition { get; set; }
        //public static List<HotelVoucherDefinition> VoucherDefinitions { get; set; }
        public static FlightContactDefinition ContactDefinition { get; set; }
    }
}
