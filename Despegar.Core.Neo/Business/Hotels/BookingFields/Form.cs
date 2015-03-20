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
        public Voucher Voucher { get { return CheckoutMethodSelected.vouchers != null ? CheckoutMethodSelected.vouchers.FirstOrDefault() : null; } } // TODO: only one voucher? Why a list?
        public CardField CardInfo { get { return CheckoutMethodSelected.payment != null ? CheckoutMethodSelected.payment.card : null; } }
        public InvoiceArg Invoice { get { return CheckoutMethodSelected.payment != null ? CheckoutMethodSelected.payment.invoice : null; } }
        public Installment CurrentInstallment { get { return CheckoutMethodSelected.payment != null ? CheckoutMethodSelected.payment.installment : null; } }    

        public string CountrySite { get; set; }

        public string booking_status { get; set; }

        public CheckoutMethodKey CheckoutMethodSelected { get; set; }
    }
}