using Despegar.Core.Business.Flight.Airline;
using System.Threading.Tasks;

namespace Despegar.Core.IService
{
    /// <summary>
    /// Contract for accessing flight data service.
    /// </summary>
    public interface IFlightService
    {
        Task<Airline> GetAirline(string searchString);
    }
}