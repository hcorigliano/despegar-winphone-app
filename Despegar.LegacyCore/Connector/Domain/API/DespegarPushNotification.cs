using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{
    public class DespegarPushNotification
    {
        public List<DespegarPushNotificationDevice> devices { get; set; }

        public string UpaId
        { 
            get 
            {
                string upa = "";
                if (devices.Count > 0) upa = devices[0].upaId;
                return upa;
            } 
        }
    }

    public class DespegarPushNotificationDevice
    {
        public string upaId { get; set; }
    }
}
