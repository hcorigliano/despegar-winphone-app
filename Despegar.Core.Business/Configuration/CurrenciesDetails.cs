using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Configuration
{
    public class CurrenciesDetails
    {
        public Defaults defaults { get; set; }
        public Permitted permitted { get; set; }
        public Checkout checkout { get; set; }
    }
}
