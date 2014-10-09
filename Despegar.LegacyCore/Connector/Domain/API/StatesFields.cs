using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{
    

    public class StatesFields : BaseResponse
    {
        public List<State> states { get; set; }

    }

    public class State{

        // API Properties

        public int? oid{ get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int? capitalOID { get; set; }
        public int? countryOID { get; set; }
    }
}
