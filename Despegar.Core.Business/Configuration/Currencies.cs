using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Configuration
{
    public class Currencies
    {
        public CurrenciesDetails flights { get; set; }
        public CurrenciesDetails hotels { get; set; }
        public CurrenciesDetails packages { get; set; }
        public CurrenciesDetails cruises { get; set; }
        public CurrenciesDetails cars { get; set; }
    }
}
