using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Configuration
{
    public class Checkout
    {
        public Domestic domestic { get; set; }
        public International international { get; set; }
    }
}
