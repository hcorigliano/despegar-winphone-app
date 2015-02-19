using Despegar.Core.Neo.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class MiniboxVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            FlightSearchPages val = (FlightSearchPages)value;

            switch (val)
            {
                case FlightSearchPages.OneWay:
                    return true;
                case FlightSearchPages.RoundTrip:
                case FlightSearchPages.Multiple:
                    return false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
