using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Hotels.SearchBox
{
    public class HotelSearchModel : BusinessModelBase
    {

        public string DestinationHotel { get; set; }
        public string DestinationHotelText { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
        public DateTimeOffset DestinationDate { get; set; }
        
        
        
        public override bool Validate()
        {
            return IsValid;
        }

        public override bool IsValid 
        { 
            get{
                return false;
            
            }
        }
                    



        
    }
}
