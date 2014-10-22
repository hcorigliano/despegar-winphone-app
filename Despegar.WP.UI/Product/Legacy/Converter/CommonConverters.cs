using Despegar.WP.UI.Strings;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Despegar.LegacyCore.Converter
{

    public class BoolToValueConverter<T> :  IValueConverter
    {        
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value == null) { return FalseValue; }
            else { return System.Convert.ToBoolean(value) ? TrueValue : FalseValue; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return value != null ? value.Equals(TrueValue) : false;
        }
    }

    public class VisibilityConverter : BoolToValueConverter<Visibility> { }

    public class StyleConverter : BoolToValueConverter<Style> { }

    public class NewBoolToValueConverter<T> : IValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return value != null ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return value != null ? TrueValue : FalseValue;
        }
    }

    public class DisplayBlockConverter : NewBoolToValueConverter<Visibility> { }

    
    public class ErrorCodeToPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return AppResources.GetLegacyString(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return AppResources.GetLegacyString(value.ToString());
        }
    }

    public class HotelCheckoutErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            //Logger.Info(parameter.ToString() + value.ToString() + "::" + Properties.ResourceManager.GetString("CheckoutLabel_Error_" + parameter.ToString() + "_" + value.ToString()));
            return AppResources.GetLegacyString("CheckoutLabel_Error_" + parameter.ToString() + "_" + value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return AppResources.GetLegacyString("CheckoutLabel_Error_" + parameter.ToString() + "_" + value.ToString());
        }
    }
}
