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
    public class StatesModel
    {

        public StatesModel()
        {
            Logger.Info("[model:states] states Model created");
        }

        public async Task<List<State>> GetAll()        
        {
            StatesFields data = await APIStatesService.GetAll();          
            
            Logger.Info("[model:States] getting all States from repository");

            return data.states; 
        }

    }
}
