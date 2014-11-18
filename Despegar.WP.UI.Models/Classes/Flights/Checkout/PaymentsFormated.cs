using Despegar.Core.Business.Flight.BookingFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes.Flights.Checkout
{

    public class PaymentsWithoutInterest
    {
        public List<PaymentDetail> OnePay { get; set; }
        public List<PaymentDetail> SixPays { get; set; }
        public List<PaymentDetail> TwelvePays { get; set; }
        public List<PaymentDetail> TwentyFourPays { get; set; }


        public PaymentsWithoutInterest()
        {
            OnePay = new List<PaymentDetail>();
            SixPays = new List<PaymentDetail>();
            TwelvePays = new List<PaymentDetail>();
            TwentyFourPays = new List<PaymentDetail>();
        }
    }

    //---------------------------------------------------------------------------//



    public class PaymentsWithInterest
    {
        public List<ListPays> OnePay { get; set; }
        public List<ListPays> SixPays { get; set; }
        public List<ListPays> TwelvePays { get; set; }
        public List<ListPays> TwentyFourPays { get; set; }
        public string GrupLabelText { get; set; }

        public PaymentsWithInterest()
        {
            OnePay = new List<ListPays>();
            SixPays = new List<ListPays>();
            TwelvePays = new List<ListPays>();
            TwentyFourPays = new List<ListPays>();
            //TODO : RESOURCE          
               

            GrupLabelText = "";
        }


    }

    public class ListPays
    {
        public List<PaymentDetail> Cards { get; set; }

        public ListPays()
        {
            Cards = new List<PaymentDetail>();
        }
    }

    //---------------------------------------------------------------------------//

    public class PaymentsFormated
    {
        public PaymentsWithoutInterest pay_at_destination { get; set; }
        public PaymentsWithInterest with_interest { get; set; }
        public PaymentsWithoutInterest without_interest { get; set; }

        public PaymentsFormated()
        {
            pay_at_destination = new PaymentsWithoutInterest();
            with_interest = new PaymentsWithInterest();
            without_interest = new PaymentsWithoutInterest();
        }

        static public List<PaymentDetail> CardToArray(PaymentDetail card)
        {
            List<PaymentDetail> array = new List<PaymentDetail>();
            array.Add(card);
            return array;

        }
    }
}
