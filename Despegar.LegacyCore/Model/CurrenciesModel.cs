using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Repository;
using Despegar.LegacyCore.Service;
using Despegar.LegacyCore.Connector.Domain.API;

namespace Despegar.LegacyCore.Model
{
    public class CurrenciesModel
    {

        public CurrenciesModel()
        {
            Logger.Info("[model:currencies] Currencies Model created");
        }

        
        public async Task Sync() {

            if (CurrenciesRep.All == null)
                CurrenciesRep.All = await APICurrenciesService.GetAll();

            else
                Logger.Info("[model:currencies] currencies all already syncronized");
        }

        public async Task<MiscCurrencies> GetAll()
        {
            if (CurrenciesRep.All == null)
                CurrenciesRep.All = await APICurrenciesService.GetAll();

            else
                Logger.Info("[model:currencies] getting all from repository");

            return CurrenciesRep.All;
        }


        public static async Task<MiscCurrency> Get(string id)
        {
            if (CurrenciesRep.All == null)
                CurrenciesRep.All = await APICurrenciesService.GetAll();

            if (CurrenciesRep.All != null)
                return CurrenciesRep.Get(id);
            else return null;
        }

        public static async Task<MiscCurrency> GetById(string id)
        {
            if (CurrenciesRep.All == null)
                CurrenciesRep.All = await APICurrenciesService.GetAll();

            if (CurrenciesRep.All != null)
                return CurrenciesRep.GetById(id);
            else return null;
        }
    }
}
