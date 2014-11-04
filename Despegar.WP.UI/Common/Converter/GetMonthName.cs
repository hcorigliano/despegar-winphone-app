using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Globalization;

namespace Despegar.WP.UI.Common.Converter
{
    public class GetMonthName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int month = 0;
            string val = value.ToString();

            try
            {
                int.TryParse(val, out month);
            }
            catch (Exception ex)
            {

                return String.Empty;
            }

            DateTimeFormatInfo dfi = new DateTimeFormatInfo();
            return dfi.GetMonthName(month).Substring(0,3);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return String.Empty;
        }
    }
}
