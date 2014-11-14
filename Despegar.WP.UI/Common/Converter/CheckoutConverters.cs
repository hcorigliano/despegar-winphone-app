using Despegar.Core.Business.Flight.BookingFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Common.Converter
{
    public class PaymentDetailsToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            PaymentDetail paymentDetails = (PaymentDetail)value;
            switch (paymentDetails.installments.quantity)
            {
                case 1:
                    return "1 " + loader.GetString("Common_Pay_With");
                case 6:
                    if (paymentDetails.interest == 1.0)
                    {
                        return "6 " + loader.GetString("Common_Pays_Without_Interest");
                    }
                    return "6 " + loader.GetString("Common_Pays_With");
                case 12:
                    if (paymentDetails.interest == 1.0)
                    {
                        return "12 " + loader.GetString("Common_Pays_Without_Interest");
                    }
                    return "12 " + loader.GetString("Common_Pays_With");
                case 24:
                    if (paymentDetails.interest == 1.0)
                    {
                        return "24 " + loader.GetString("Common_Pays_Without_Interest");
                    }
                    return "24 " + loader.GetString("Common_Pays_With");
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    public class PaymentDetailsToPaysConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            PaymentDetail paymentDetails = (PaymentDetail)value;
            switch (paymentDetails.installments.quantity)
            {
                case 1:
                    return "1 " + loader.GetString("Common_Pay_Of") + "$" + paymentDetails.installments.first.ToString();
                case 6:
                    return "1 " + loader.GetString("Common_Pay_Of") + "$" + paymentDetails.installments.first.ToString() + " + 5 " + loader.GetString("Common_Pays_Of") + "$" + paymentDetails.installments.others.ToString();
                case 12:
                    return "1 " + loader.GetString("Common_Pay_Of") + "$" + paymentDetails.installments.first.ToString() + " + 11 " + loader.GetString("Common_Pays_Of") + "$" + paymentDetails.installments.others.ToString();
                case 24:
                    return "1 " + loader.GetString("Common_Pay_Of") + "$" + paymentDetails.installments.first.ToString() + " + 23 " + loader.GetString("Common_Pays_Of") + "$" + paymentDetails.installments.others.ToString();
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
