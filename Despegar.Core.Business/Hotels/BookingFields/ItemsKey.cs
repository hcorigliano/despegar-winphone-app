using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.BookingFields
{
    public class ItemsKey
    {
        public string item_id { get; set; }
        public string checkout_method { get; set; }
        public ItemPrice price { get; set; }
        public PriceDestination price_destination { get; set; }
        public PaymentOptions payment { get; set; }  

        public bool isPaymentAtDestination
        { 
            get 
            {
                if (payment.at_destination.Count() == 0)
                    return false;

                return true;
            } 
        
        }

    }
}