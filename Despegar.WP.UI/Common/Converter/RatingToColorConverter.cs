﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    class RatingToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Double points;

            try
            {
                Double.TryParse(value.ToString(), out points);
            }
            catch (Exception ex)
            {
                points = 0;
            }

            if (value == null)
                return "";

            if (points > 9.00)
                return "#01B02F";
            if (points > 7.00)
                return "#95C428";
            if (points > 5.00)
                return "#FFC006";
            return "#FF7900";
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


    
}
