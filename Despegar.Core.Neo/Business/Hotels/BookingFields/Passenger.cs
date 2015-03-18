using Despegar.Core.Neo.Business.Common.Checkout;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class Passenger
    {
        public bool required { get; set; }
        public RegularField first_name { get; set; }
        public RegularField last_name { get; set; }
        public RegularField room_reference { get; set; }

        // Custom
        public int Index { get; set; }

        public bool IsFrozen { get; set; }
    }
}