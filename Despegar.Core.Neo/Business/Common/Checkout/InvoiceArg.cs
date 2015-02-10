using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Common.Checkout
{
    public class InvoiceArg
    {
        public bool required { get; set; }
        public string data_type { get; set; }
        public Address address { get; set; }
        public RegularField fiscal_id { get; set; }
        public RegularField fiscal_name { get; set; }
        public RegularOptionsField fiscal_status { get; set; }

        public RegularField card_holder_name { get; set; }
    }
}