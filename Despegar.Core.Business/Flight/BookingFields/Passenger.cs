using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Passenger : Bindable
    {
        public bool required { get; set; }
        public string data_type { get; set; }
        public string type { get; set; }
        public Document document { get; set; }
        public RegularOptionsField gender { get; set; }
        public RegularField nationality { get; set; }                
        public RegularField first_name { get; set; }
        public RegularField last_name { get; set; }
        public Birthdate birthdate { get; set; }
        public int full_name_max_length { get; set; }

        private bool c;
        public bool IsEnabled { get { return c;} set{c =value; OnPropertyChanged();} }
    }
}