using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Configuration
{
    public class CitiesFields
    {
        public string full_name { get; set; }
        public string id { get; set; }
        public string code { get; set; }

        public override string ToString()
        {
            return this.full_name;
        }
    }

}
