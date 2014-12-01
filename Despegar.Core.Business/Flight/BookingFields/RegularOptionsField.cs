using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class RegularOptionsField : RegularField
    {
        public List<Option> options { get; set; }
       
        public override void Validate()
        {            
            // Combobox bever shows error, there is always a selected value
        }

        /// <summary>
        /// Sets the API Default Value of the Field
        /// </summary>
        public override void SetDefaultValue()
        {
            if (value == null)
            {
                // Select first option available
                if (options != null && options.Count > 0)                
                    this.CoreValue = options.FirstOrDefault().value;
                
            } else {
                // API default                
                this.CoreValue = value;
            }
        }
    }
}