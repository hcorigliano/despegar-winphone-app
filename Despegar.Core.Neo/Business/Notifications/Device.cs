using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Notifications
{
    public class Device
    {
        public string _id { get; set; }
        public string token { get; set; }
        public string brand { get; set; }
        public string upa_id { get; set; }
        public string device_type { get; set; }
        public string country_id { get; set; }
        public long last_loggin { get; set; }
    }
}
