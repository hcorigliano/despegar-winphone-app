using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingCompletePost
{
    public class Form
    {
        public List<Passenger> passengers { get; set; }
        public Payment payment { get; set; }
        public Contact contact { get; set; }
        public Comment comment { get; set; }
        public Invoice invoice { get; set; }
        public List<string> vouchers { get; set; }
        public List<object> destination_services { get; set; }
        public OfflinePayment offline_payment { get; set; }
        public ExternalDebit external_debit { get; set; }
        public string sessionId { get; set; }
        public List<RiskAnswer> risk_questions { get; set; }
    }
}
