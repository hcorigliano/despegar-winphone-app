using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.WP.UI.Model.Classes;

namespace Despegar.WP.UI.Model
{
    public class FlightSearchModel : AppModelBase , Interfaces.IValidateInterface ,Interfaces.IInitializeModelInterface
    {

        public DateTimeOffset DepartureDate { get; set; }
        public DateTimeOffset DestinationDate { get; set; }
        public string OriginFlight { get; set; }
        public string DestinationFlight { get; set; }
        public bool OneWay { get; set; }
        public int AdultsInFlights { get; set; }
        public int ChildrenInFlights { get; set; }
        public int InfantsInFlights { get; set; }
        public int LimitResult { get; set; }

        //public AdvanceSearchModel AdvanceSearch { get; set; }

        public FlightSearchModel()
        {
            this.InitializeModel();
        }

        public new void InitializeModel()
        {
            base.InitializeModel();
            //TODO uncomment following code for advance search
            //this.AdvanceSearch = new AdvanceSearchModel();

            OriginFlight = string.Empty;
            DestinationFlight = string.Empty;
        }

        public new void Validate()
        {
            //throw new NotImplementedException();
        }


    }
}
