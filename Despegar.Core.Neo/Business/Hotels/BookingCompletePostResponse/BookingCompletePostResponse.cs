using Despegar.Core.Neo.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Hotels.BookingCompletePostResponse
{
    public class BookingCompletePostResponse
    {
        public string booking_id { get; set; }
        public string checkout_id { get; set; }
        public string booking_status { get; set; }
        public List<RiskQuestion> risk_questions { get; set; }

        public MAPIError Error { get; set; }
    }
}
