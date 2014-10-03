using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public class FlightResultsModel: AppModelBase , Interfaces.IInitializeModelInterface, Interfaces.IValidateInterface
    {

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
