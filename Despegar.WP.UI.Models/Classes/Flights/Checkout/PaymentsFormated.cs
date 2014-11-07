using Despegar.Core.Business.Flight.BookingFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes.Flights.Checkout
{
    public class PaymentWithoutInterest
    {
        public List<PaymentDetail> PaymentsDetails {get;set;}
        public string LabelText {get; set;}
        public string PaysText { get; set;}

        public PaymentWithoutInterest()
        {
            PaymentsDetails = new List<PaymentDetail>();
            //LabelText = "";
            //PaysText = "";
        }

    }


    public class PaymentsWithoutInterest
    {
        public PaymentWithoutInterest OnePay { get; set; }
        public PaymentWithoutInterest SixPays { get; set; }
        public PaymentWithoutInterest TwelvePays { get; set; }
        public PaymentWithoutInterest TwentyFourPays { get; set; }


        public PaymentsWithoutInterest()
        {
            OnePay = new PaymentWithoutInterest();
            SixPays = new PaymentWithoutInterest();
            TwelvePays = new PaymentWithoutInterest();
            TwentyFourPays = new PaymentWithoutInterest();
        }
    }

    //---------------------------------------------------------------------------//

    public class PaymentWithInterest
    {
        public PaymentDetail PaymentsDetails { get; set; }
        public string LabelText { get; set; }
        public string PaysText { get; set; }

        public PaymentWithInterest()
        {
            PaymentsDetails = new PaymentDetail();
            //LabelText = "";
            //PaysText = "";
        }

    }


    public class PaymentsWithInterest
    {
        public List<PaymentWithInterest> OnePay { get; set; }
        public List<PaymentWithInterest> SixPays { get; set; }
        public List<PaymentWithInterest> TwelvePays { get; set; }
        public List<PaymentWithInterest> TwentyFourPays { get; set; }
        public string GrupLabelText { get; set; }

        public PaymentsWithInterest()
        {
            OnePay = new List<PaymentWithInterest>();
            SixPays = new List<PaymentWithInterest>();
            TwelvePays = new List<PaymentWithInterest>();
            TwentyFourPays = new List<PaymentWithInterest>();
            //TODO : RESOURCE
            GrupLabelText = "6 , 12 o 24 pagos con";
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
    }
}
