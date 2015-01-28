using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.HotelDetails
{
    public class RoomAvailability
    {
        public string roompack_classification { get; set; }
        public MealPlan meal_plan { get; set; }
        public CancellationPolicy cancellation_policy { get; set; }
        public Price price { get; set; }
        public int max_payment_quantity { get; set; }
        public string payment_description { get; set; }
        public List<string> payment_types { get; set; }
        public List<string> choices { get; set; }
    }
}
