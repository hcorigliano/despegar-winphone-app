using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{
    public class Discounts
    {
        public bool discountAvailable { get; set; }

        public DiscountCountry AR { get; set; }
        public DiscountCountry BR { get; set; }
        public DiscountCountry BO { get; set; }
        public DiscountCountry CL { get; set; }
        public DiscountCountry CR { get; set; }
        public DiscountCountry EC { get; set; }
        public DiscountCountry ES { get; set; }
        public DiscountCountry SV { get; set; }
        public DiscountCountry GT { get; set; }
        public DiscountCountry HN { get; set; }
        public DiscountCountry MX { get; set; }
        public DiscountCountry NI { get; set; }
        public DiscountCountry PA { get; set; }
        public DiscountCountry PY { get; set; }
        public DiscountCountry PE { get; set; }
        public DiscountCountry PR { get; set; }
        public DiscountCountry DO { get; set; }
        public DiscountCountry US { get; set; }
        public DiscountCountry UY { get; set; }
        public DiscountCountry VE { get; set; }


        public DiscountCountry Get(string country)
        {
            PropertyInfo pi = this.GetType().GetRuntimeProperty(country);
            return pi.GetValue(this, null) as DiscountCountry;
        }
    }

    public class DiscountCountry
    {
        public string minimumSupportedAppVersion { get; set; }
        public Discount discount { get; set; }
    }


    public class Discount
    {
        public HotelDiscount hotels { get; set; }
    }

    public class HotelDiscount
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int discountAmount { get; set; }
        public string code { get; set; }
        public string discountText { get; set; }
        public DiscountBanner banner { get; set; }
    }

    public class DiscountBanner
    {
        public DiscountDetailBanner homeBanner { get; set; }
        public DiscountDetailBanner resultsBanner { get; set; }
        public DiscountDetailBanner detailBanner { get; set; }
        
    }

    public class DiscountDetailBanner
    {
        public string leftText { get; set; }
        public string backColor { get; set; }
        public string textcolor { get; set; }
    }
}
