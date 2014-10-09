using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{
    public class GeoCountries : BaseResponse
    {
        public List<GeoCountry> countries {get; set;}
    }

    public class GeoCountry : BaseResponse
    {
        public string internalId { get; set; }
        public string phoneCode { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public GeoLocation geoLocation { get; set; }
    }

    public class GeoLocation
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
}
