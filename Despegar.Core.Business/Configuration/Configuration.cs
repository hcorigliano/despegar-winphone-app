using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Configuration
{
    public class Configuration
    {
        public string cruiseOfficeCode { get; set; }
        public EmissionAnticipationDays emissionAnticipationDays { get; set; }
        public CurrencyMasks currencyMasks { get; set; }
        public bool visible { get; set; }
        public string brandId { get; set; }
        public string id { get; set; }
        public Contact contact { get; set; }
        public List<string> products { get; set; }
        public string mainCity { get; set; }
        public Domains domains { get; set; }
        public string description { get; set; }
        public List<object> otherHomeCities { get; set; }
        public LanguageAndCurrency defaults { get; set; }
        public Prices prices { get; set; }
        public Currencies currencies { get; set; }
    }
}
