using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Coupons
{
    public class CouponResponse
    {
        public decimal amount { get; set;}
        public decimal percentage { get; set;}
        public string currency { get; set;}
        public string usage { get; set;}     
        public List<string> products { get; set; }        
        public string coupon_id { get; set;}
        public decimal total_amount_with_discount_applied { get; set;}
        public bool is_refundable { get; set;}
        public string coupon_type_name { get; set;}
        //additional_info: { }
    }
}