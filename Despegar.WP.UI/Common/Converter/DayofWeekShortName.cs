using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class DayofWeekShortName : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {

            string val;

            try
            {
                val = value.ToString();
            }
            catch (Exception ex)
            {
                return String.Empty;
            }

            return val.Substring(0,3);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
