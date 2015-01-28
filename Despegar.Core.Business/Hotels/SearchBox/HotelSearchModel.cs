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

        //public void UpdateSearchDays()
        //{
        //    DateTime daysToAdd;
        //    DateTime daysToCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, LastAvailableHours, 0, 0);

        //    if (DateTime.Compare(DateTime.Now, daysToCompare) > 0)
        //    {
        //        daysToAdd = DateTime.Today.AddDays(EmissionAnticipationDay + 1);
        //    }
        //    else
        //    {
        //        daysToAdd = DateTime.Today.AddDays(EmissionAnticipationDay);
        //    }

        //    this.DepartureDate = daysToAdd;
        //    this.DestinationDate = daysToAdd;

        //    DateBoundary = daysToAdd;
        //}


        
    }
}
