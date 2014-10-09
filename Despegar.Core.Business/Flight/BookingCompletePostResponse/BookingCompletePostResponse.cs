using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Flight.BookingCompletePostResponse
{
    public class BookingCompletePostResponse
    {
        public string id { get; set; }
        public string pnr { get; set; }
        public string checkout_id { get; set; }
        public string booking_status { get; set; }
        public List<RiskQuestion> risk_questions { get; set; }
    }
}
