using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Globalization;
using Windows.Storage;

namespace Despegar.WP.UI.Common.Converter
{
    public class GetMonthName : IValueConverter
    {
        /// <summary>
        /// Convert the month into abreaviate using the country selected on the country selection page.
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int month = 1;
            string val = value.ToString();
            
            //The best way to get this working with binding is modifying the current culture when the app starts then from xaml use the ConvertLanguageParameter
            //string lan = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            string lan = roamingSettings.Values["countryLanguage"] as string;
            lan = (lan == null) ? String.Empty : lan;
            try
            {
                int.TryParse(val, out month);
            }
            catch (Exception)
            {
                return String.Empty;
            }
            var cultureInfo = new CultureInfo(lan);
            DateTimeFormatInfo dfi = cultureInfo.DateTimeFormat;
            return dfi.GetAbbreviatedMonthName(month).ToUpper();
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return String.Empty;
        }
    }
}
