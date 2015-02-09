using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Common.Checkout
{
    public class Expiration : RegularField
    {
        public string from { get; set; }
        public string to { get; set; }

        // Custom
        public override void Validate()
        {
            CurrentError = null;
            Errors.Clear();

            if (required && String.IsNullOrWhiteSpace(CoreValue))
            {
                Errors.Add("REQUIRED");
                CurrentError = "REQUIRED";
                return;
            }

            if (!required && String.IsNullOrWhiteSpace(CoreValue))
            {
                return;
            }

            DateTime value = new DateTime(Int32.Parse(CoreValue.Substring(0,4)), Int32.Parse(CoreValue.Substring(5)), 1);
            DateTime min = new DateTime(Int32.Parse(from.Substring(0, 4)), Int32.Parse(from.Substring(5)), 1);
            DateTime max = new DateTime(Int32.Parse(to.Substring(0, 4)), Int32.Parse(to.Substring(5)), 1);

            if (value < min || value > max) 
            {
                Errors.Add("INVALID_DATE");
                CurrentError = "INVALID_DATE";
            }
        }
    }
}
