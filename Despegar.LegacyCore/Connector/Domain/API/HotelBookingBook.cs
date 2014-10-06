using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{
    public class HotelBookingBook : BaseResponse
    {
        public HotelBookingBookData data { get; set; }
    }

    public class HotelBookingBookData
    {
        public string cuponError { get; set; } // nullable
        public string checkoutId { get; set; } // 27612924
        //public object cuponResponse: null
        public string status { get; set; } // "OK"
        public string onlinePaymentResponse { get; set; } // "NOT_APPLICABLE"
        public string checkOutStatus { get; set; } // "SUCCESS"
        public string collectResponse { get; set; } // "CC_AUTHORIZED"
        public string ticket { get; set; } // "f11da008-a603-11e3-a9aa-fa163e7a50a2"
        public string riskResponse { get; set; } // "NOT_VERIFIED"
        public string pnr { get; set; }
        
        //public string riskResult: null
        //public object riskQuestions: [0]
        //public object messages: [0]
    }
}