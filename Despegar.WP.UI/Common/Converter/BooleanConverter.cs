using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    enum Parameters
    {
        Normal, Inverted
    }   

     public class BooleanConverter<T> :  IValueConverter
     {        
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) { return FalseValue; }

            else 
            {
                Parameters direction;
                bool parseSuccess = Enum.TryParse<Parameters>((string)parameter, out direction);

                bool theBool = System.Convert.ToBoolean(value);

                if (parseSuccess && direction == Parameters.Inverted)
                    theBool = !theBool;

                return  theBool ? TrueValue : FalseValue; 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value != null ? value.Equals(TrueValue) : false;
        }
     }
}