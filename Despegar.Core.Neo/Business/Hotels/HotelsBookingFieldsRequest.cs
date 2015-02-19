using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Hotels
{
    public class HotelsBookingFieldsRequest
    {
        public string token { get; set; }
        public string hotel_id { get; set; }
        public List<string> room_choices { get; set; }
        public string thread_metrix_session_id { get; set; }
        public string mobile_identifier { get; set; }
    }
}
