using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class StyleColorResultConverter : IValueConverter
    {
        public Style TrueValue { get; set; }
        public Style FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            object val = value;

            if (value != null)
            {

                if (value.GetType() == typeof(RouteInbound))
                {
                    //return string.Format(parameter as string, right);
                    return TrueValue;
                }

                if (value.GetType() == typeof(RouteOutbound))
                {
                    //return string.Format(parameter as string, left);
                    return FalseValue;
                }
            }

            return FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return string.Empty;
        }
    }
}
