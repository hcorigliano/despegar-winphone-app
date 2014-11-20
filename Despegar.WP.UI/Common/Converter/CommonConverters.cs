﻿using Despegar.Core.Business.Flight.BookingFields;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;


namespace Despegar.WP.UI.Common.Converter
{
    // TODO Replace for the VisibilityProConverter
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

    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) { return Visibility.Collapsed; }
            return  Visibility.Visible; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ListToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return  ((IList)value).Count != 0 ? Visibility.Visible : Visibility.Collapsed;
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

    public class DateToExpirationStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            if (value == null)
            {
                return "";
            }
            else
            {
                return ((DateTimeOffset)value).ToString("yyyy-MM");
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
                return ((DateTimeOffset)value).ToString("yyyy-MM");
            }
        }
    }

    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Format(parameter as string, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }


   
}
