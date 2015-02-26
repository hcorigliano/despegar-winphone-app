using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Notifications
{
    public class PushResponse
    {
        public string _id { get; set; }
        public List<Device> devices { get; set; }
        public List<Subscription> subscriptions { get; set; }
        public string social_id { get; set; }
    }
}
