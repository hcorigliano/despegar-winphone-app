using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Connector.Domain.API;

namespace Despegar.LegacyCore.Repository
{
    public class CurrenciesRep
    {

        public static MiscCurrencies All { get; set; }

        public static MiscCurrency Get(string id)
        {
            return All.currencies.First(it => { return it.countryId == id; });
        }

        public static MiscCurrency GetById(string id)
        {
            return All.currencies.First(it => { return it.id == id; });
        }
    }
}
