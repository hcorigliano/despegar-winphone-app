using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Coupons
{
    public class CouponParameter
    {
        public string ReferenceCode { get; set; }
        public string Beneficiary { get; set; }
        public string TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string Quotation { get; set; }
        public string Product { get; set; }
    }
}