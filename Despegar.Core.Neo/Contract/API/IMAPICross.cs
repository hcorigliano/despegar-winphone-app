using Despegar.Core.Neo.Business.Common.State;
using Despegar.Core.Neo.Business.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Contract.API
{
    public interface IMAPICross
    {
        Task<Configuration> GetConfigurations();
        Task<UpdateFields> CheckUpdate(string AppVersion, string OsVersion, string Source, string Device);
        Task<List<State>> GetStates(string country);
        Task<List<CitiesFields>> AutoCompleteCities(string CountryCode, string Search, string CityResult);
        Task<Countries> GetCountries();
    }
}