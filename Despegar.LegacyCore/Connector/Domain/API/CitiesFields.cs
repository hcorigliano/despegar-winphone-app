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
    public class CitiesFields : List<City>
    {
        //public List<City> cities { get; set; }
    }

    public class City
    {

        // API Properties
        public string code { get; set; }
        public int? id { get; set; }
        public string full_name { get; set; }

        public override string ToString()
        {
            return full_name;
        }
    }

}
