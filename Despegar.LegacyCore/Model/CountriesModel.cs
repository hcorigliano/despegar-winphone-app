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
    public class CountriesModel
    {

        public CountriesModel()
        {
            Logger.Info("[model:countries] Countries Model created");
        }

        
        public async Task Sync() {

            if (CountriesRep.All == null)
                CountriesRep.All = await APICountriesService.GetAll();

            else
                Logger.Info("[model:countries] countries all already syncronized");
        }

        public async Task<GeoCountries> GetAll()
        {
            if (CountriesRep.All == null)
                CountriesRep.All = await APICountriesService.GetAll();
            
            else
                Logger.Info("[model:countries] getting all from repository");

            return CountriesRep.All;
        }

    }
}
