using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class TimeDurationConverterToHours: IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try { 
                if (value == null){
                    return 0;
                }


                string[] durationTime = ((string)value).Split(new Char[] { ':' });
                return durationTime[0];

            }catch(Exception ex)
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }


    public class TimeDurationConverterToMins : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try { 

                if (value == null)
                {
                    return 0;
                }

                string[] durationTime = ((string)value).Split(new Char[] { ':' });

                return durationTime[1];

            }catch(Exception ex)
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
