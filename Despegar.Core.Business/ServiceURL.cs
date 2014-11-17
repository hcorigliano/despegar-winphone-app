using System.Collections.Generic;

namespace Despegar.Core.Business
{
    public enum ServiceKey
    {
        FlightsAirlines,
        FlightCitiesAutocomplete,
        FlightItineraries,
        FlightsBookingFields,
        Configuration,
        BookingCompletePost,
        Update,
        CitiesAutocomplete,
        Countries
    }

    /// <summary>
    /// Service URLs constants class
    /// </summary>
    public static class ServiceURL
    {    
        private static readonly Dictionary<ServiceKey, string> serviceURLRepo = new Dictionary<ServiceKey, string>
        {
            {ServiceKey.FlightsAirlines, "mapi-flights/airlines?description={0}" },
            {ServiceKey.FlightCitiesAutocomplete, "mapi-cross/autocomplete/flights?search={0}" },
            {ServiceKey.FlightItineraries,"mapi-flights/itineraries?from={0}&to={1}&departure_date={2}&adults={3}&return_date={4}&children={5}&infants={6}&offset={7}&limit={8}&order_by={9}&order_type={10}&currency_code={11}&filter={12}"},
            {ServiceKey.FlightsBookingFields,"mapi-flights/bookings?"},
            {ServiceKey.Configuration,"mapi-cross/configuration"},
            {ServiceKey.BookingCompletePost,"mapi-flights/bookings/{0}"},
            {ServiceKey.Update,"mapi-cross/apps/update/{0}/?os_version={1}&installation_source={2}&device_description={3}"},
            {ServiceKey.CitiesAutocomplete,"mapi-cross/apps/autocomplete/{0}/{1}/?city_result={2}"}, 
            {ServiceKey.Countries,"mapi-cross/apps/"}, 

        };

        public static string GetServiceURL(ServiceKey key)
        {
            return serviceURLRepo[key];
        }
    }
}