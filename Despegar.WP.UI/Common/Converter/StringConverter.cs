using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class StringConverter : IValueConverter
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The formatted value.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Format(parameter as string, value);
        }

        /// <summary>
        /// Converts the value back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The original value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // This does not need to be implemented for simple one-way conversion.
            return null;
        }
    } 
}