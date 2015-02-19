using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Neo.Business.Configuration;


namespace Despegar.Core.Neo.IService
{
    public interface IConfigurationService
    {
        Task<Configuration> GetConfigurations();
        Task<UpdateFields> CheckUpdate(string OsVersion,string AppVersion, string Source , string Device);
        Task<List<CitiesFields>> AutoCompleteCities(string CountryCode, string Search, string CityResult);
        Task<Countries> GetCountries();
    }
}
