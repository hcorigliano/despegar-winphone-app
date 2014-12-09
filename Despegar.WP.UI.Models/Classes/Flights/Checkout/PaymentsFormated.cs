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

    public class PaymentsWithInterest : PaymentsWithoutInterest
    {
        public string GrupLabelText { get; set; }        
    }

    //---------------------------------------------------------------------------//

    public class InstallmentFormatted
    {
        public PaymentsWithoutInterest PayAtDestination { get; set; }
        public PaymentsWithInterest WithInterest { get; set; }
        public PaymentsWithoutInterest WithoutInterest { get; set; }

        public InstallmentFormatted()
        {
            PayAtDestination = new PaymentsWithoutInterest();
            WithInterest = new PaymentsWithInterest();
            WithoutInterest = new PaymentsWithoutInterest();
        }

    }
}
