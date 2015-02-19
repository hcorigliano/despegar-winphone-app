using Despegar.Core.Neo.Business.Coupons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class VoucherSuccessTextConverter : IValueConverter
    { 
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) 
            { 
                return String.Empty; 
            }
            else 
            {
                CouponResponse val = (CouponResponse)value;
                var loader = new ResourceLoader();
                string result = loader.GetString("Checkout_Coupon_Valid");
                result += " " + val.percentage + " %";
                result += " ($" + val.currency + " " + val.amount.ToString("N0") + ")";

                return result;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
