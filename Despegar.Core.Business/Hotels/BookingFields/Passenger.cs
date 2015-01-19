using Despegar.Core.Business.Common.Checkout;

namespace Despegar.Core.Business.Hotels.BookingFields
{
    public class Passenger
    {
        public bool required { get; set; }
        public RegularField first_name { get; set; }
        public RegularField last_name { get; set; }
        public RegularField room_reference { get; set; }
    }
}