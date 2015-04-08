using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class DateToShortTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {           
            if (value == null)
                return String.Empty;

            DateTime date = (DateTime)value;
            return date.ToString("HH:MM:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}