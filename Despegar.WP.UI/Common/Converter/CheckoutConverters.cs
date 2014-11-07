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
            PaymentDetail paymentDetails = (PaymentDetail)value;
            switch (paymentDetails.installments.quantity)
            {
                case 1:
                    return "1 Pago con ";
                case 6:
                    if (paymentDetails.interest == 1.0)
                    {
                        return "6 Pagos sin interés con "; ;
                    }
                    return "6 Pagos con ";
                case 12:
                    if (paymentDetails.interest == 1.0)
                    {
                        return "6 Pagos sin interés con "; ;
                    }
                    return "6 Pagos con ";
                case 24:
                    if (paymentDetails.interest == 1.0)
                    {
                        return "6 Pagos sin interés con "; ;
                    }
                    return "6 Pagos con ";
            }

            return "";
            //return string.Format(parameter as string, value);
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
            PaymentDetail paymentDetails = (PaymentDetail)value;
            switch (paymentDetails.installments.quantity)
            {
                case 1:
                    return "1 Pago de $" + paymentDetails.installments.first.ToString();
                case 6:
                    return "1 Pago de $" + paymentDetails.installments.first.ToString() + " + 5 Pagos de $" + paymentDetails.installments.others.ToString();
                case 12:
                    return "1 Pago de $" + paymentDetails.installments.first.ToString() + " + 11 Pagos de $" + paymentDetails.installments.others.ToString();
                case 24:
                    return "1 Pago de $" + paymentDetails.installments.first.ToString() + " + 23 Pagos de $" + paymentDetails.installments.others.ToString();
            }

            return "";
            //return string.Format(parameter as string, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
