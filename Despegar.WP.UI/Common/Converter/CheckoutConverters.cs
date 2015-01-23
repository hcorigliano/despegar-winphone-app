using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.Business.Hotels.BookingFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;

namespace Despegar.WP.UI.Common.Converter
{
    public class PaymentDetailsToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            PaymentDetail paymentDetails = (PaymentDetail)value;

            TextBlock text = new TextBlock();

            if (paymentDetails.installments.quantity == 1)
                text.Inlines.Add(new Run() { Text = "1 " + loader.GetString("Common_Pay_With") });
            else 
            {
                if (paymentDetails.interest == 1.0)
                {
                    text.Inlines.Add(new Run() { Text =  paymentDetails.installments.quantity + " " + loader.GetString("Common_Payments") + " " });

                    var b = new Bold();
                    b.Inlines.Add(new Run() { Text = loader.GetString("Common_Pays_Without_Interest") });
                    text.Inlines.Add(b);

                    text.Inlines.Add(new Run() { Text = " " + loader.GetString("Common_Payment_With") });
                }
                else 
                {
                    text.Inlines.Add(new Run() { Text =  paymentDetails.installments.quantity + " " + loader.GetString("Common_Pays_With") });
                }
            }           

            return new ContentControl() { Content = text };;
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

            string text = "1 " + loader.GetString("Common_Pay_Of") + "$" + paymentDetails.installments.first.ToString();

            if (paymentDetails.installments.quantity > 1)           
                text += " + " + (paymentDetails.installments.quantity - 1) + " " + loader.GetString("Common_Pays_Of") + "$" + paymentDetails.installments.others.ToString();
            
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    public class CardListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            List<HotelPayment> cards = value as List<HotelPayment>;
            bool isPayAtDestination = false;

            if(cards != null)
               isPayAtDestination = cards.Any(z => z.type=="at_destination");

            if (isPayAtDestination) 
            {
                // Add PayAtDestination "card"
                var newList = new List<HotelPayment>();
                newList.Add(new HotelPayment() { card = new Despegar.Core.Business.Hotels.BookingFields.Card() { code = "EFECTIVO" } });
                newList.AddRange(cards);
                
                return newList;
            }

            // Not Hotels, or not PayAtDestination
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }    
}