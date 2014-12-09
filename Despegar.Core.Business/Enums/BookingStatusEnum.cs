using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Enums
{
    public enum  BookingStatusEnum
    {
        checkout_successful,
        booking_failed,
        fix_credit_card,
        payment_failed,
        new_credit_card,
        risk_review,
        BookingCustomError
    }
}
