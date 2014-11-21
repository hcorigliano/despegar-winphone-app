using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class ImageGoReturnConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            object val = value;

            string right = "right";
            string left = "left";

            if (value!=null)
            {

                if (value.GetType() == typeof(RouteInbound))
                {
                    return string.Format(parameter as string, right);
                }

                if (value.GetType() == typeof(RouteOutbound))
                {
                    return string.Format(parameter as string, left);
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return string.Empty;
        }
    }
}
