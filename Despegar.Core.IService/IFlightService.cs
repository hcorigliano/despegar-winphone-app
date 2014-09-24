using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.IService
{
    /// <summary>
    /// Contract for accessing to flight data service.
    /// </summary>
    public interface IFlightService
    {
        Task<string> GetItineraries(string airportCode);
    }
}
