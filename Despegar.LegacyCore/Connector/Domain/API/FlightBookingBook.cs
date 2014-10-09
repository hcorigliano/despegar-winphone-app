using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Despegar.LegacyCore.Model;

namespace Despegar.LegacyCore.Connector.Domain.API
{
    public class FlightBookingBook : BaseResponse
    {
        public FlightBookingBookData data { get; set; }
    }

    public class FlightBookingBookData
    {
        public string cuponError { get; set; } // nullable
        public string checkoutId { get; set; } // 27612924
        public string cuponResponse { get; set; }
        public string status { get; set; } // "OK"
        public string onlinePaymentResponse { get; set; } // "NOT_APPLICABLE"
        public string collectResponse { get; set; } // "CC_AUTHORIZED"
        public string ticket { get; set; } // "f11da008-a603-11e3-a9aa-fa163e7a50a2"
        public string riskResponse { get; set; } // "NOT_VERIFIED"
        public string pnr { get; set; } //"TEST-PNR"
        //public string riskResult: null

        public List<object> riskQuestions { get; set; }
        //public object messages: [0]


        public string checkOutStatus
        { 
            get
            {
                if (status == null || 
                    status.ToUpper() == "BOOKING_ERROR")
                    return BookingResponse.NO_RECOVERABLE_BOOKING_ERROR;

                if (collectResponse != null && 
                    collectResponse.ToUpper() == "CC_FIXABLE")
                    return BookingResponse.RECOVERABLE_FIX_CREDIT_CARD;

                if (collectResponse != null && 
                    collectResponse.ToUpper() == "CC_NEW")
                    return BookingResponse.RECOVERABLE_NEW_CREDIT_CARD;

                if (collectResponse != null && 
                    collectResponse.ToUpper() == "CC_MAX_RETRIES_ERROR")
                    return BookingResponse.NO_RECOVERABLE_CREDIT_CARD_ERROR;

                if (cuponResponse != null && 
                    cuponResponse.ToUpper() == "ERROR")
                    return BookingResponse.C_NO_RECOVERABLE_CONSUME_COUPON_ERROR;

                if (collectResponse != null &&
                    collectResponse.ToUpper() != "CC_FIXABLE" && 
                    collectResponse.ToUpper() != "CC_NEW" && 
                    collectResponse.ToUpper() != "CC_MAX_RETRIES_ERROR")
                {
                    if (riskResponse != null && 
                        riskResponse.ToUpper() == "REJECT")
                        return BookingResponse.NO_RECOVERABLE_RISK_REJECTED;

                    if (riskResponse != null && 
                        riskQuestions.Count > 0 &&
                        riskResponse.ToUpper() == "REVIEW")
                        return BookingResponse.RISK_QUESTIONS;
                }

                return BookingResponse.SUCCESS;
            }
        }
    }
}