using Despegar.Core.Business.Flight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.IService
{
    /// <summary>
    /// Contract for accessing flight data service.
    /// </summary>
    public interface IFlightService
    {
        Task<Airline> GetAirline(string airlineDescription);
    }
}