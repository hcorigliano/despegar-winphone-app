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
        public Voucher Voucher { get { return checkout_method.FirstItem.vouchers != null ? checkout_method.FirstItem.vouchers.FirstOrDefault() : null; } } // TODO: only one voucher? Why a list?
        public CardField CardInfo { get { return checkout_method.FirstItem.payment.card; } }
        public InvoiceArg Invoice { get { return checkout_method.FirstItem.payment.invoice; } }
    }
}