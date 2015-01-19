using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Common.Checkout
{
    public class Voucher : RegularField
    {
        public int min_quantity { get; set; } // Not necessary, MAPI validates this

        // Custom
        private bool isApplied;
        public bool IsApplied { get { return isApplied; } set { isApplied = value; OnPropertyChanged(); } }

        public override void Validate()
        {
            CurrentError = null;
            Errors.Clear();

            // Coupon is set
            if (!String.IsNullOrWhiteSpace(CoreValue))
            {
                // Is it Service Validated?
                if (!IsApplied)                             
                { 
                    // Some code is set, but was not validated or it is invalid
                    Errors.Add("INVALID_VOUCHER");
                    CurrentError = "INVALID_VOUCHER";
                }
            }

            OnPropertyChanged("IsValid");
        }
    }
}