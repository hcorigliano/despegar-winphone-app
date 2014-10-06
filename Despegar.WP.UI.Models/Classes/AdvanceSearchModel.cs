using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes
{
    public class AdvanceSearchModel: Interfaces.IInitializeModelInterface, Interfaces.IValidateInterface
    {
        public Dictionary<string,string> DepartureTime { get; set; }
        public Dictionary<string, string> ReturnTime { get; set; }
        public Dictionary<string,string> StopOver { get; set; }
        public Dictionary<string,string> TicketClass { get; set; }
        public string AirlineName { get; set; }


        public AdvanceSearchModel()
        {
            this.InitializeModel();
        }

        public void InitializeModel()
        {
            throw new NotImplementedException();
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
