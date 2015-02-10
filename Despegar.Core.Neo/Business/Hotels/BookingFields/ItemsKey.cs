using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class ItemsKey
    {
        public string item_id { get; set; }
        public string checkout_method { get; set; }
        public ItemPrice price { get; set; }
        public PriceDestination price_destination { get; set; }
        public PaymentOptions payment { get; set; }  
    }
}