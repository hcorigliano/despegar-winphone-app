using Despegar.Core.Neo.Business.Flight.BookingFields;
using Despegar.Core.Neo.Business.Hotels.BookingFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Hotels
{    
    //---------------------------------------------------------------------------//

    /// <summary>
    /// Represents an installment with a fixed quantity (I.E: one pay,  two pays, three pays)  and the corresponding list of cards to choose from.
    /// </summary>
    public class InstallmentOption
    {
        public int InstallmentQuantity { get; set; }
        public List<HotelPayment> Cards { get; set; }

        public HotelPayment FirstCard { get { return Cards[0]; } }

        // For "WithInterest" payments
        public string GrupLabelText { get; set; }

        public InstallmentOption(int quantity)
        {
            this.InstallmentQuantity = quantity;
            this.Cards = new List<HotelPayment>();
        }
    }

    //---------------------------------------------------------------------------//

    public class InstallmentFormatted
    {
        public InstallmentOption PayAtDestination { get; set; }
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
                    int index = input.LastIndexOf(',');
                    if (index>0){
                        sb[index] = 'o';
                    }
                    return sb.ToString() + " " + ResourceLabel;
                } 
                else 
                    return String.Empty;
            } 
        }

        public InstallmentFormatted()
        {
            PayAtDestination = new InstallmentOption(1);
            WithInterest = new List<InstallmentOption>();
            WithoutInterest = new List<InstallmentOption>();
        }

        public void AddWithouInterestInstallment(HotelPayment payment)
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

        public void AddWithInterestInstallment(HotelPayment payment)
        {            
            // Each Card is a separated Installments, Cards are not grouped here.  It is one card per installment
            var installment = new InstallmentOption(payment.installments.quantity);
            installment.Cards.Add(payment);
            WithInterest.Add(installment);            
        }

        public void AddPayAtDestinationInstallment(HotelPayment payment)
        {
            PayAtDestination.Cards.Add(payment);
        }

    }
}