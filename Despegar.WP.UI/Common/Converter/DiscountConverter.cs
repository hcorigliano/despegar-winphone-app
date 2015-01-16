using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class TypeDiscountTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            if (value != null && (string)value == "STANDARD") //MOBILE
                return loader.GetString("Common_Discount_Mobile_Fare");
            return "";
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class TypeDiscountImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && (string)value == "MOBILE") 
                return "assets\\PhoneDisccountLogo.png";
            return "";

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class DiscountConverter
    {
    }
}
