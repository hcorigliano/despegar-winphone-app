using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{
    public class MiscCurrencies : BaseResponse
    {
        public List<MiscCurrency> currencies;
    }

    public class MiscCurrency
    {
        public string countryId { get; set; }
        public double ratio { get; set; }
        public string id { get; set; }
        public string symbol { get; set; }
        public string description { get; set; }
    }
}
