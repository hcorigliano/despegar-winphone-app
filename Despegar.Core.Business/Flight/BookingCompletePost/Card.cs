using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingCompletePost
{
    public class Card
    {
        public string number { get; set; }
        public string expiration { get; set; }
        public string security_code { get; set; }
        public string bank { get; set; }
        public string owner_type { get; set; }
        public string owner_name { get; set; }
        public TypeAndNumber owner_document { get; set; }
        public string owner_gender { get; set; }
        public TypeAndNumber owner_fiscal_document { get; set; }
    }
}