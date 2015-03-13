using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.BookingFields
{
    public class LocationRequirements
    {
        public string title { get; set; }
        public string description { get; set; }
        public List<string> messages { get; set; }
    }
}