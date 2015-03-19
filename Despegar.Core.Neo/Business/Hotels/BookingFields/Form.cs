﻿using Despegar.Core.Neo.Business.Common.Checkout;
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
        public Voucher Voucher { get { return checkout_method.FirstItem.vouchers != null ? checkout_method.FirstItem.vouchers.FirstOrDefault() : null; } } // TODO: only one voucher? Why a list?
        public CardField CardInfo { get { return checkout_method.FirstItem.payment != null ? checkout_method.FirstItem.payment.card : null; } }
        public InvoiceArg Invoice { get { return checkout_method.FirstItem.payment != null ? checkout_method.FirstItem.payment.invoice : null; } }
        public Installment CurrentInstallment { get { return checkout_method.FirstItem.payment != null ? checkout_method.FirstItem.payment.installment : null; } }    
        public string CountrySite { get; set; }

        public string booking_status { get; set; }
    }
}