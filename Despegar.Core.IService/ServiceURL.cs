
using System.Collections.Generic;
namespace Despegar.Core.IService
{
    public enum ServiceKey
    {
        FlightsAirlines,
        FlightCitiesAutocomplete        
    }

    /// <summary>
    /// Service URLs constants class
    /// </summary>
    public static class ServiceURL
    {    
        private static readonly Dictionary<ServiceKey, string> serviceURLRepo = new Dictionary<ServiceKey, string>
        {
            {ServiceKey.FlightsAirlines, "mapi-flights/airlines?description={0}" },
            {ServiceKey.FlightCitiesAutocomplete, "mapi-cross/autocomplete/flights?search={0}" }
        };

        public static string GetServiceURL(ServiceKey key)
        {
            return serviceURLRepo[key];
        }
    }
}