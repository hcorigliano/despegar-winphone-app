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
    public class CitiesModel
    {
        public CitiesModel()
        {
            Logger.Info("[model:cities] Cities Model created");
        }

        public async Task<List<City>> GetAll(string stringBusqueda, int idState)
        {
            CitiesFields data = await APICitiesService.GetAll( stringBusqueda,  idState);

            Logger.Info("[model:States] getting all Cities from repository");

            return data;
        }
    }
}
