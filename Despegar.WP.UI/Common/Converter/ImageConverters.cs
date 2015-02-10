using Despegar.Core.Neo.Business.Hotels;
//using Despegar.Core.Neo.Business.Neo.Hotels;
//using Despegar.Core.Neo.Business.Neo.Hotels.CitiesAvailability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class StarsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && (int)value >= System.Convert.ToInt32(parameter)) //MOBILE
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class AmenitiesVisibiliryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && ((List<Amenity>)value).Exists(x => x.id == parameter.ToString()))
                return Visibility.Visible;
            return Visibility.Collapsed;    
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
