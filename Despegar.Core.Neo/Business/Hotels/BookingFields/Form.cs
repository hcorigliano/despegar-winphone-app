using Despegar.Core.Business.Hotels.BookingFields;
using Despegar.Core.Neo.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class Form
    {
        public List<Passenger> passengers { get; set; }
        public Contact contact { get; set; }
        public CheckoutMethod checkout_method { get; set; }
        public AdditionalData additional_data { get; set; }         

        // Custom
        // TODO: ADAPT TO NOT GET THE FIRST ITEM, BUT THE CORRECT ITEM dependening on PayAtDestination or OnlineCreditCard
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        public Voucher Voucher { get { return checkout_method.FirstItem.vouchers != null ? checkout_method.FirstItem.vouchers.FirstOrDefault() : null; } } // TODO: only one voucher? Why a list?
        public CardField CardInfo { get { return checkout_method.FirstItem.payment.card; } }
        public InvoiceArg Invoice { get { return checkout_method.FirstItem.payment.invoice; } }
        public BillingAddress BillingAddress { get { return checkout_method.FirstItem.payment.billing_address; } }
        public Installment CurrentInstallment { get { return checkout_method.FirstItem.payment.installment; } }    
        public string CountrySite { get; set; }

        public string booking_status { get; set; }
    }
}