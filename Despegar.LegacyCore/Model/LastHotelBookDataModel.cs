using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Connector.Domain.API;

namespace Despegar.LegacyCore.Model
{
    public static class LastHotelBookData
    {
        public static HotelBookingBook LastBookResponse { get; set; }
        public static HotelsAvailabilityModel AvailabilityModel { get; set; }
        public static HotelAvailabilityItem AvailabilityInfo { get; set; }
        public static List<HotelPassengerDefinition> PassengerDefinitions { get; set; }
        public static HotelCardDefinition CardDefinition { get; set; }
        public static List<HotelVoucherDefinition> VoucherDefinitions { get; set; }
        public static HotelContactDefinition ContactDefinition { get; set; }
    }
}
