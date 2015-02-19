using Despegar.Core.Neo.Business.Enums;
using Despegar.Core.Neo.Business.Flight.SearchBox;
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
            if (value == null) return String.Empty;


            FlightSearchPages val = (FlightSearchPages)value;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

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
            if (value == null) return String.Empty;

            FlightSearchPages val = (FlightSearchPages)value;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

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


    public class TextHeaderConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {

            FlightSearchModel val = (FlightSearchModel)value;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (val == null) return String.Empty;

            switch (val.PageMode)
            {
                case FlightSearchPages.OneWay:
                    return loader.GetString("Generic_Go");
                case FlightSearchPages.Multiple:
                    return loader.GetString("Generic_Multiple");
                case FlightSearchPages.RoundTrip:
                    return loader.GetString("Generic_Go_Return");
            }

            return String.Empty;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
