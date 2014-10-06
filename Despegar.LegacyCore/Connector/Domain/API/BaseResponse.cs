using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{
    public class BaseResponse
    {
        public Meta meta { get; set; }
        public List<Errors> errors { get; set; }
    }

    public class Meta
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public string time { get; set; }
        public string reference { get; set; }
    }

    public class Errors
    {

    }
}
