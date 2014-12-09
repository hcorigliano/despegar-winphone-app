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

    public class BookingStatus
    {
        //private static readonly Dictionary<BookingStatusEnum, string> bookingStatusDic = new Dictionary<BookingStatusEnum, string>
        //{
        //    {BookingStatusEnum.CCBookingFailed,"booking_failed"},
        //    {BookingStatusEnum.CCFixCreditCard,"fix_credit_card"},
        //    {BookingStatusEnum.CCPaymentFailed,"payment_failed"},
        //    {BookingStatusEnum.CCNewCreditCard,"new_credit_card"},
        //    {BookingStatusEnum.RiskQuestionRiskReview,"risk_review"}
        //};

        //public static string GetBookingStatusString(BookingStatusEnum key)
        //{
        //    try
        //    {
        //        return bookingStatusDic[key];
        //    }
        //    catch (Exception)
        //    {
        //        return string.Empty;
        //    }
            
        //}

        /*public static string GetBookingStatusEnum(string value)
        {
            try
            {
                //return bookingStatusDic.Values.Where(x=>x)
            }
            catch (Exception)
            {
                
                throw;
            }
        }*/

    }
}
