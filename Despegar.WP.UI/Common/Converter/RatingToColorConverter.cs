using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    class RatingToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return "";
            if ((double)value > 9)
                return "#01B02F";
            if ((double)value > 7)
                return "#95C428";
            if ((double)value > 5)
                return "#FFC006";
            return "#FF7900";
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


    
}
