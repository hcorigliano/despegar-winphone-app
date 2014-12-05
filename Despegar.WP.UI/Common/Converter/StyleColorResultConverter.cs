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

            string green = "IconGreen";
            string blue = "IconBlue";

            if (value != null)
            {

                if (value.GetType() == typeof(RouteInbound))
                {
                    return App.Current.Resources[blue];
                }

                if (value.GetType() == typeof(RouteOutbound))
                {
                    return App.Current.Resources[green];
                }
            }

            return App.Current.Resources[green];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return string.Empty;
        }
    }
}
