using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes.Flights
{
    public class QuantityPassagersControViewModel
    {
        public PassagersQuantity Passengers { get; set; }
        public List<ChildrenAgeOption> ChildrenAgeOptions {get;set;}
    }
}