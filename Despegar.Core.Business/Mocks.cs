using System.Collections.Generic;

namespace Despegar.Core.Business
{
    public enum MockKey
    {
        FlightCitiesAutocompleteBue,
        AirlineTest
    }

    /// <summary>
    /// Mocks repository
    /// </summary>
    public static class Mocks
    {
        private static readonly Dictionary<MockKey, string> mocksRepo = new Dictionary<MockKey, string>
        {                    
            {MockKey.FlightCitiesAutocompleteBue, "[{\"name\":\"Buenos Aires, Ciudad de Buenos Aires, Argentina\",\"id\":982,\"type\":\"city\",\"geo_location\":{\"latitude\":-34.60546023079388,\"longitude\":-58.381991406249995},\"code\":\"BUE\",\"country_code\":\"AR\",\"has_airport\":true},{\"name\":\"Mar del Plata, Buenos Aires, Argentina\",\"id\":4445,\"type\":\"city\",\"geo_location\":{\"latitude\":-37.9799,\"longitude\":-57.58979999999997},\"code\":\"MDQ\",\"country_code\":\"AR\",\"has_airport\":true},{\"name\":\"Bahia Blanca, Buenos Aires, Argentina\",\"id\":685,\"type\":\"city\",\"geo_location\":{\"latitude\":-38.7117,\"longitude\":-62.268100000000004},\"code\":\"BHI\",\"country_code\":\"AR\",\"has_airport\":true},{\"name\":\"Villa Gesell, Buenos Aires, Argentina\",\"id\":7922,\"type\":\"city\",\"geo_location\":{\"latitude\":-37.2634,\"longitude\":-56.973299999999995},\"code\":\"VLG\",\"country_code\":\"AR\",\"has_airport\":true},{\"name\":\"Tandil, Buenos Aires, Argentina\",\"id\":7265,\"type\":\"city\",\"geo_location\":{\"latitude\":-37.3178,\"longitude\":-59.15039999999999},\"code\":\"TDL\",\"country_code\":\"AR\",\"has_airport\":true},{\"name\":\"Tres Arroyos, Buenos Aires, Argentina\",\"id\":5510,\"type\":\"city\",\"geo_location\":{\"latitude\":-38.3869,\"longitude\":-60.3297},\"code\":\"OYO\",\"country_code\":\"AR\",\"has_airport\":true},{\"name\":\"La Plata, Buenos Aires, Argentina\",\"id\":4203,\"type\":\"city\",\"geo_location\":{\"latitude\":-34.9173,\"longitude\":-57.95010000000002},\"code\":\"LPG\",\"country_code\":\"AR\",\"has_airport\":true},{\"name\":\"Aeropuerto Buenos Aires Jorge Newbery, Buenos Aires, Argentina\",\"id\":192636,\"type\":\"airport\",\"geo_location\":{\"latitude\":-34.556267,\"longitude\":-58.41661099999999},\"city\":{\"id\":982,\"code\":\"BUE\",\"name\":\"Buenos Aires\"},\"code\":\"AEP\",\"country_code\":\"AR\"},{\"name\":\"Aeropuerto Buenos Aires Ministro Pistarini Ezeiza, Buenos Aires, Argentina\",\"id\":192635,\"type\":\"airport\",\"geo_location\":{\"latitude\":-34.81266,\"longitude\":-58.539761},\"city\":{\"id\":982,\"code\":\"BUE\",\"name\":\"Buenos Aires\"},\"code\":\"EZE\",\"country_code\":\"AR\"},{\"name\":\"Aeropuerto Buenaventura, Buenaventura, Colombia\",\"id\":192647,\"type\":\"airport\",\"geo_location\":{\"latitude\":3.825,\"longitude\":-76.995834},\"city\":{\"id\":991,\"code\":\"BUN\",\"name\":\"Buenaventura\"},\"code\":\"BUN\",\"country_code\":\"CO\"},{\"name\":\"Aeropuerto Heide-Buesum, Heide/Buesum, Alemania\",\"id\":194307,\"type\":\"airport\",\"geo_location\":{\"latitude\":54.15448,\"longitude\":8.89503},\"city\":{\"id\":2785,\"code\":\"HEI\",\"name\":\"Heide/Buesum\"},\"code\":\"HEI\",\"country_code\":\"DE\"},{\"name\":\"Aeropuerto Gen Rafael Buelna, Mazatlan, México\",\"id\":196396,\"type\":\"airport\",\"geo_location\":{\"latitude\":23.167305,\"longitude\":-106.270142},\"city\":{\"id\":4948,\"code\":\"MZT\",\"name\":\"Mazatlan\"},\"code\":\"MZT\",\"country_code\":\"MX\"},{\"name\":\"Aeropuerto Gen Buech, Riberalta, Bolivia\",\"id\":197539,\"type\":\"airport\",\"geo_location\":{\"latitude\":-11.006016,\"longitude\":-66.075752},\"city\":{\"id\":6370,\"code\":\"RIB\",\"name\":\"Riberalta\"},\"code\":\"RIB\",\"country_code\":\"BO\"}]" },
            {MockKey.AirlineTest, "{id: \"JJ\",name: \"Tam\"}"}
        };

        public static string GetMock(MockKey key)
        {
            return mocksRepo[key];            
        }
    }
}