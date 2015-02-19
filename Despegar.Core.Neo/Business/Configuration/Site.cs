using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Configuration
{
    public class Site
    {
        public string code { get; set; }
        public string name { get; set; }
        public string language { get; set; }
        public List<Product> products { get; set; }
        public Domain domain { get; set; }
        public Contact contact { get; set; }
        public string main_city { get; set; }
        public string default_currency { get; set; }
    }
}
