using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Model
{
    public static class BookingResponse
    {
        //--------- Checkout finished successfully ---------

        public const string SUCCESS = "SUCCESS";


        //------------- Recoverable errors ---------------

        /* There was a recoverable error validating the credit card.  Card data must be fixed */
        public const string RECOVERABLE_FIX_CREDIT_CARD = "FIX_CREDIT_CARD";

        /* There was an unrecoverable error validating the credit card.  New card should be provided*/
        public const string RECOVERABLE_NEW_CREDIT_CARD = "NEW_CREDIT_CARD";

        /* There was an unrecoverable error validating the credit card due to low founds.  New card should be provided */
        public const string RECOVERABLE_NEW_CREDIT_CARD_LOW_FOUNDS = "NEW_CREDIT_CARD_LOW_FOUNDS";


        //-------------- Aditional data needed ---------------

        /* Risk analysis of purchase indicated that more information is needed about the customer . Question responses must be provided to continue with workflow */
        public const string RISK_QUESTIONS = "RISK_QUESTIONS";


        //---------------- Blocker responses ---------------------

        /* Checkout finished with booking error. No further operations can be done with workflow  */
        public const string NO_RECOVERABLE_BOOKING_ERROR = "BOOKING_ERROR";

        /* There was a booking error, checkout can continue by picking a new hotel */
        public const string NO_RECOVERABLE_NEW_BOOKING = "NEW_BOOKING";

        /* There was a booking error due to a session expired in the provider, checkout can continue by picking a new hotel */
        public const string NO_RECOVERABLE_NEW_BOOKING_EXPIRED = "NEW_BOOKING_EXPIRED";

        /* There was a booking error, checkout can continue by picking a new provider for the same hotel */
        public const string NO_RECOVERABLE_NEW_BOOKING_NEW_PROVIDER = "NEW_BOOKING_NEW_PROVIDER";

        /* Checkout finished with an error validating the credit card. No further operations can be done with workflow  */
        public const string NO_RECOVERABLE_CREDIT_CARD_ERROR = "CREDIT_CARD_ERROR";

        /* Checkout finished with an error canceling an operation with the credit card. No further operations can be done with workflow  */
        public const string NO_RECOVERABLE_CREDIT_CARD_CANCEL_ERROR = "CREDIT_CARD_CANCEL_ERROR";

        /* Checkout finished with an error generating bank slip. No further operations can be done with workflow  */
        public const string NO_RECOVERABLE_BANK_SLIP_ERROR = "BANK_SLIP_ERROR";

        /* Checkout finished with an error due to a risky operation. No further operations can be done with workflow */
        public const string NO_RECOVERABLE_RISK_REJECTED = "RISK_REJECTED";


        //---------- coupon error custom message-not recoverable --------------

        /* Checkout finished with an error consuming a discount cupon. No further operations can be done with workflow */
        public const string C_NO_RECOVERABLE_CONSUME_COUPON_ERROR = "CONSUME_COUPON_ERROR";
    }
}
