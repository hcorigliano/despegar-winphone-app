using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class DayofWeekShortName : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            
            string day = string.Empty;

            // TODO: Use GLOBAL CONFIGURATION to obtain thte Current Language
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            string lan = roamingSettings.Values["countryLanguage"] as string;
            lan = (lan == null) ? String.Empty : lan;

            try
            {
                var cultureInfo = new CultureInfo(lan);
                var dateTimeInfo = cultureInfo.DateTimeFormat;
                System.DayOfWeek d = (System.DayOfWeek)value;
                day = dateTimeInfo.GetAbbreviatedDayName(d);
            }
            catch (Exception)
            {
                //TODO add log
                return String.Empty;
            }

            return day;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
