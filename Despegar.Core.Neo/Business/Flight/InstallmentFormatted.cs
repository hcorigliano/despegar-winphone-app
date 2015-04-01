using Despegar.Core.Neo.Business.Flight.BookingFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Flights
{    
    //---------------------------------------------------------------------------//

    /// <summary>
    /// Represents an installment with a fixed quantity (I.E: one pay,  two pays, three pays)  and the corresponding list of cards to choose from.
    /// </summary>
    public class InstallmentOption
    {
        public bool IsChecked { get; set; }

        public int InstallmentQuantity { get; set; }
        public List<PaymentDetail> Cards { get; set; }

        public PaymentDetail FirstCard { get { return Cards[0]; } }

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
        public List<InstallmentOption> WithInterest { get; set; }
        public List<InstallmentOption> WithoutInterest { get; set; }
        public string ResourceLabel { get; set; }
        public string GrupLabelText 
        { 
            get 
            {
                if (WithInterest.Count > 0) 
                {
                    string input = String.Join(" , ", WithInterest.Select(x => x.InstallmentQuantity.ToString()).Distinct());
                    StringBuilder sb = new StringBuilder(input);
                    if (input.LastIndexOf(',') != -1)
                        sb[input.LastIndexOf(',')] = 'o';
                    return sb.ToString() + " " + ResourceLabel;
                } 
                else 
                    return String.Empty;
            } 
        }

        public InstallmentFormatted()
        {
            //PayAtDestination = new PaymentsWithoutInterest();
            WithInterest = new List<InstallmentOption>();
            WithoutInterest = new List<InstallmentOption>();
        }

        public void AddWithouInterestInstallment(PaymentDetail payment)
        {
            int quantity = payment.installments.quantity;

            InstallmentOption installment = WithoutInterest.FirstOrDefault(z => z.InstallmentQuantity == quantity);

            if (installment == null)
            {
                installment = new InstallmentOption(quantity);
                WithoutInterest.Add(installment);
            }

            installment.Cards.Add(payment);
        }

        public void AddWithInterestInstallment(PaymentDetail payment)
        {            
            // Each Card is a separated Installments, Cards are not grouped here.  It is one card per installment
            var installment = new InstallmentOption(payment.installments.quantity);
            installment.Cards.Add(payment);
            WithInterest.Add(installment);            
        }

    }
}