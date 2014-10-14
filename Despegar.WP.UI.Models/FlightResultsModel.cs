using Despegar.Core.Business.Flight.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;


namespace Despegar.WP.UI.Model
{
    public class FlightResultsModel : AppModelBase, Interfaces.IInitializeModelInterface, Interfaces.IValidateInterface
    {
        public FlightsItineraries Itineraries{ get; set; }
        
        public FlightResultsModel()
        {
            this.InitializeModel();
        }

        public new void InitializeModel()
        {
            base.InitializeModel();

            //TODO initialize all variables needed for this page.
        }

        public new void Validate()
        {
            base.Validate();

            //Validate each variable for this model
        }

    }
}
