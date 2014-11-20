﻿using Despegar.Core.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class TextGoReturnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            FlightSearchPages val = (FlightSearchPages)value;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (val == null) return String.Empty;

            switch ( val)
            {
                case FlightSearchPages.OneWay:
                case FlightSearchPages.RoundTrip:
                    return loader.GetString("Generic_Go");
                case FlightSearchPages.Multiple:
                    return loader.GetString("Generic_Multiple");
            }

            return loader.GetString("Generic_Go"); ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }


    public class TextMultipleConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {

            FlightSearchPages val = (FlightSearchPages)value;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (val == null) return String.Empty;

            switch (val)
            {
                case FlightSearchPages.OneWay:
                case FlightSearchPages.Multiple:
                    return String.Empty;
                case FlightSearchPages.RoundTrip:
                    return loader.GetString("Generic_Return");
            }

            return String.Empty;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}