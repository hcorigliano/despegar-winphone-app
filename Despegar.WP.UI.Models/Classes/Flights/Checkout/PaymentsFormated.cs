using Despegar.Core.Business.Flight.BookingFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes.Flights.Checkout
{    
    //---------------------------------------------------------------------------//

    /// <summary>
    /// Represents an installment with a fixed quantity (I.E: one pay,  two pays, three pays)  and the corresponding list of cards to choose from.
    /// </summary>
    public class InstallmentOption
    {
        public int InstallmentQuantity { get; set; }
        public List<PaymentDetail> Cards { get; set; }       

        // For "WithInterest" payments
        public string GrupLabelText { get; set; }

        public InstallmentOption(int quantity)
        {
            this.InstallmentQuantity = quantity;
            this.Cards = new List<PaymentDetail>();
        }
    }

    //---------------------------------------------------------------------------//

    public class InstallmentFormatted
    {
        //public List<InstallmentOption> PayAtDestination;  // TODO
        public List<InstallmentOption> WithInterest;
        public List<InstallmentOption> WithoutInterest;
        public string ResourceLabel { get; set; }
        public string GrupLabelText 
        { 
            get 
            {
                string input = String.Join(" , ", WithInterest.Select(x => x.InstallmentQuantity.ToString()));
                StringBuilder sb = new StringBuilder(input);
                sb[input.LastIndexOf(',')] = 'o';
                return sb.ToString() + " " + ResourceLabel;
            } 
        }

        public InstallmentFormatted()
        {
            //PayAtDestination = new PaymentsWithoutInterest();
            WithInterest = new List<InstallmentOption>();
            WithoutInterest = new List<InstallmentOption>();
        }

        public void AddWithoutInterest(PaymentDetail payment, bool withInterest)
        {
            var list = withInterest ? WithInterest : WithoutInterest;
            int quantity = payment.installments.quantity;

            InstallmentOption installment = list.FirstOrDefault(z => z.InstallmentQuantity == quantity);

            if (installment == null)
            {
                installment = new InstallmentOption(quantity);
                list.Add(installment);
            }

            installment.Cards.Add(payment);
        }       
    }
}