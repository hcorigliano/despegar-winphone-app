using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Business.Configuration;

namespace Despegar.Core.IService
{
    public interface IConfigurationService
    {
        Task<Configuration> GetConfigurations();
        Task<UpdateFields> CheckUpdate();
        Task<CitiesFields> AutoCompleteCities(string CountryCode, string Search, string CityResult);       
    }
}
