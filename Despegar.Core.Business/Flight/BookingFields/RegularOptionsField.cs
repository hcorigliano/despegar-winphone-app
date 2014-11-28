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

        public new void Validate() 
        {
            // TODO: Validate
            if(String.IsNullOrWhiteSpace(CoreValue))
            {
            }
        }
    }
}