using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingCompletePost
{
    public class OfflinePayment
    {
        public string owner_type { get; set; }
        public Document document { get; set; }
        public string gender { get; set; }
        public TypeAndNumber fiscal_document { get; set; }
    }
}
