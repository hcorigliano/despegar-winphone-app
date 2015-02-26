using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Notifications
{
    public class PushRegistrationRequest
    {
        public string brand { get; set; }
        public string country_id { get; set; }
        public string device_type { get; set; }
        public string token { get; set; }
        public string upa_id { get; set; }
        public string social_id { get; set; }
    }
}
