using Despegar.Core.Business.Common.State;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.IService;
using Despegar.WP.UI.Model.Classes.Flights.Checkout;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public class FlightsCheckoutModel
    {
        private IFlightService flightService;
        private ICommonServices CommonServices;

        public FlightsCheckoutModel()
        {
            flightService = GlobalConfiguration.CoreContext.GetFlightService();
            CommonServices = GlobalConfiguration.CoreContext.GetCommonService();
        }

        public async Task<BookingFields> GetBookingFields(BookingFieldPost bookingFieldPost)
        {           
            return (await flightService.GetBookingFields(bookingFieldPost));
        }
        
        public void CompleteCheckOut(dynamic form)
        {
            string json = JsonConvert.SerializeObject(form);
            int test = 1;
        }

        public async Task<List<State>> GetStates(string country)
        {
            return (await CommonServices.GetStates(country));
        }



        public static PaymentsFormated FormatPayments(Payments payments)
        {
            PaymentsFormated formated = new PaymentsFormated();
            FillFormatedWithInterest(payments.with_interest, formated.with_interest);
            FillFormatedWithoutInterest(payments.without_interest, formated.without_interest);
            formated.with_interest = FillTextInPaymentsFormatedWith(formated.with_interest);
            formated.without_interest = FillTextInPaymentsFormatedWithout(formated.without_interest);
            //TODO : FILL PAY AT DESTINATION
            return formated;
        }


        private static PaymentsWithInterest FillTextInPaymentsFormatedWith(PaymentsWithInterest paymentsWithInterest)
        {
            foreach(PaymentWithInterest payment in paymentsWithInterest.OnePay)
            {
                payment.LabelText = "1 Pago con ";
                payment.PaysText = "1 Pago de $" + payment.PaymentsDetails.installments.first.ToString();                 
            }
            foreach (PaymentWithInterest payment in paymentsWithInterest.SixPays)
            {
                payment.LabelText = "6 Pago con ";
                payment.PaysText = "1 Pago de $" + payment.PaymentsDetails.installments.first.ToString() + " + 5 Pagos de $" + payment.PaymentsDetails.installments.others.ToString();

            }
            foreach (PaymentWithInterest payment in paymentsWithInterest.TwelvePays)
            {
                payment.LabelText = "12 Pagos con ";
                payment.PaysText = "1 Pago de $" + payment.PaymentsDetails.installments.first.ToString() + " + 11 Pagos de $" + payment.PaymentsDetails.installments.others.ToString();

            }
            foreach (PaymentWithInterest payment in paymentsWithInterest.TwentyFourPays)
            {
                payment.LabelText = "24 Pago con ";
                payment.PaysText = "1 Pago de $" + payment.PaymentsDetails.installments.first.ToString() + " + 23 Pagos de $" + payment.PaymentsDetails.installments.others.ToString();

            }

            return paymentsWithInterest;
        }        

        private static PaymentsWithoutInterest FillTextInPaymentsFormatedWithout(PaymentsWithoutInterest paymentsFilter)
        {
            //TODO: USE RESOURCES
            if(paymentsFilter.OnePay.PaymentsDetails.Count() != 0)
            {
                paymentsFilter.OnePay.LabelText = "1 Pago con ";
                paymentsFilter.OnePay.PaysText = "1 Pago de $" + paymentsFilter.OnePay.PaymentsDetails[0].installments.first.ToString();                
            }
            if (paymentsFilter.SixPays.PaymentsDetails.Count() != 0)
            {
                paymentsFilter.SixPays.LabelText = "6 Pagos sin interés con ";
                paymentsFilter.SixPays.PaysText = "1 Pago de $" + paymentsFilter.SixPays.PaymentsDetails[0].installments.first.ToString() + " + 5 Pagos de $" + paymentsFilter.SixPays.PaymentsDetails[0].installments.others.ToString();         
            }
            if (paymentsFilter.TwelvePays.PaymentsDetails.Count() != 0)
            {
                paymentsFilter.TwelvePays.LabelText = "12 Pagos sin interés con ";
                paymentsFilter.TwelvePays.PaysText = "1 Pago de $" + paymentsFilter.TwelvePays.PaymentsDetails[0].installments.first.ToString() + " + 11 Pagos de $" + paymentsFilter.TwelvePays.PaymentsDetails[0].installments.others.ToString();
            }
            if (paymentsFilter.TwentyFourPays.PaymentsDetails.Count() != 0)
            {
                paymentsFilter.TwentyFourPays.LabelText = "24 Pagos sin interés con ";
                paymentsFilter.TwentyFourPays.PaysText = "1 Pago de $" + paymentsFilter.TwentyFourPays.PaymentsDetails[0].installments.first.ToString() + " + 23 Pagos de $" + paymentsFilter.TwentyFourPays.PaymentsDetails[0].installments.others.ToString();
            }            

            return paymentsFilter;
        }



        private static void FillFormatedWithInterest(List<PaymentDetail> list, PaymentsWithInterest paymentsWithInterest)
        {
            foreach (PaymentDetail item in list)
            {
                PaymentWithInterest payment = new PaymentWithInterest();
                payment.PaymentsDetails = item;
                switch (item.installments.quantity)
                {
                    case 1:

                        paymentsWithInterest.OnePay.Add(payment);
                        break;
                    case 6:
                        paymentsWithInterest.SixPays.Add(payment);
                        break;
                    case 12:
                        paymentsWithInterest.TwelvePays.Add(payment);
                        break;
                    case 24:
                        paymentsWithInterest.TwentyFourPays.Add(payment);
                        break;
                }
            }
        }

        private static void FillFormatedWithoutInterest(List<PaymentDetail> list, PaymentsWithoutInterest paymentsFilter)
        {
            foreach(PaymentDetail item in list )
            {
                switch (item.installments.quantity)
                {
                    case 1:
                        paymentsFilter.OnePay.PaymentsDetails.Add(item);
                        break;
                    case 6:
                        paymentsFilter.SixPays.PaymentsDetails.Add(item);
                        break;
                    case 12:
                        paymentsFilter.TwelvePays.PaymentsDetails.Add(item);
                        break;
                    case 24:
                        paymentsFilter.TwentyFourPays.PaymentsDetails.Add(item);
                        break;
                }
            }
        }
    }
}
