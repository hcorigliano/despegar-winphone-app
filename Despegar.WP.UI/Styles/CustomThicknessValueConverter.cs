using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Styles
{
    public class CustomThicknessValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            CustomThickness custom = value as CustomThickness;

            if (custom != null)
            {
                return new Thickness(custom.Left, custom.Top, custom.Right, custom.Bottom);
            }
            else
            {
                return new Thickness(0, 0, 0, 0);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
