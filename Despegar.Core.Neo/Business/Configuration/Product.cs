using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Configuration
{
    public class Product
    {
        public string name { get; set; }
        public string status { get; set; }
        public int emission_anticipation_days { get; set; }
        public string last_available_hour { get; set; }
    }
}
