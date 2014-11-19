using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class DateToLongString : IValueConverter
    {
        /// <summary>
        /// Converts a DateTime object to string format like "16 NOV 2014"
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTimeOffset val = (DateTimeOffset)value;
            var currentLanguage = GlobalConfiguration.Language;
            var cultureInfo = new CultureInfo(currentLanguage);
            DateTimeFormatInfo format = cultureInfo.DateTimeFormat;
            string formattedDate = val.ToString("dd") + " " + format.GetAbbreviatedMonthName(val.Month) + " " + val.Year;

            return formattedDate.Replace(".", "").ToUpper();
            //return val.ToString("dd MMM yyyy").Replace(".", "").ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}