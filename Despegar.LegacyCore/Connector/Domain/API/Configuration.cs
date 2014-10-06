using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{
    public class Configurations : BaseResponse
    {
        public List<Configuration> configuration { get; set; }
    }

    public class Configuration
    {
        public string id { get; set; }

        public string mainCity { get; set; }
        public List<string> otherHomeCities { get; set; }
        public List<string> products { get; set; }
        public Domains domains { get; set; }
        public Prices prices { get; set; }
    }


    public class Domains
    {
        public string @base { get; set; }
        public string staticContent { get; set; }
        public string mediaContent {get; set;}
    }

    public class Prices
    {
        public object showsBreakDown { get; set; }
    }
}
