using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class CheckoutMethod : Dictionary<string, CheckoutMethodKey>
    {
        public CheckoutMethodKey FirstItem { get { return this.Count > 0 ? this.FirstOrDefault().Value : null; } }
    }
}
