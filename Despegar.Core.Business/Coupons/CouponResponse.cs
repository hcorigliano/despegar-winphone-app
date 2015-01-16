using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Coupons
{

    public enum VoucherErrors
    {
        COUPON_INVALID = 400100,
        COUPON_EXPIRED = 400202,
        COUPON_NO_USES_REMAINING = 400203,
        COUPON_WRONG_COUNTRY = 400204,
        COUPON_INVALID_BENEFICIARY = 400206,
        COUPON_ALREADY_USED_BY_USER = 400207,
        COUPON_INVALID_DATE = 400423,
    }

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

        // Custom
        public VoucherErrors? Error { get; set; }
    }
}