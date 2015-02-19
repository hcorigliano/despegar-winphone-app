using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.BookingCompletePost
{
    public class Comment
    {
        public string reference_code { get; set; }
        public string follow_reservation { get; set; }
        public string text { get; set; }
    }
}
