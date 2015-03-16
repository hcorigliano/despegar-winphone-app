using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Enums
{
    public enum HotelBookingStatusEnum
    {
        BOOKING_ERROR,
        FIX_CREDIT_CARD,
        NEW_CREDIT_CARD,
        RISK_QUESTIONS,
        SUCCESS,
        RISK_REJECTED,
        BookingCustomError
    }
}
