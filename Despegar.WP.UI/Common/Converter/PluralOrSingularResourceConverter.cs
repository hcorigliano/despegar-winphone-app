using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    class PluralOrSingularResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            int intValue;
            try
            {
                intValue = (int)value;
            }
            catch
            {
                return null;
            }
            if (intValue == 0)
                return null;

            switch (parameter.ToString())
            {
                case "Room":
                    {
                        if (intValue == 1)
                            return loader.GetString("Common_Room");
                        else
                            return loader.GetString("Common_Rooms");
                    }
                case "Night":
                    {
                        if (intValue == 1)
                            return loader.GetString("Common_Night");
                        else
                            return loader.GetString("Common_Nights");
                    }
                case "Adult":
                    {
                        if (intValue == 1)
                            return loader.GetString("Common_Adult");
                        else
                            return loader.GetString("Common_Adults");
                    }
                case "Child":
                    {
                        if (intValue == 1)
                            return loader.GetString("Common_Child");
                        else
                            return loader.GetString("Common_Childs");
                    }
            
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
