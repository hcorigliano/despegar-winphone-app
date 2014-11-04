using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;


namespace Despegar.WP.UI.Common.Converter
{

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) { return Visibility.Collapsed; }
            else { return System.Convert.ToBoolean(value) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class TypeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            switch(value.ToString())
            {
                case "ADULT":
                    return loader.GetString("Generic_Adult");
                case "CHILD":
                    return loader.GetString("Generic_Child");
                case "INFANT":
                    return loader.GetString("Generic_Infant");
                default:
                    return "";

            }
        }        

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            
            if(value == null)
            { 
                return "";
            }
            else
            {
                return ((DateTimeOffset)value).ToString("yyyy-MM-dd");
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                return ((DateTimeOffset)value).ToString("yyyy-MM-dd");
            }
        }
    }
   
}
