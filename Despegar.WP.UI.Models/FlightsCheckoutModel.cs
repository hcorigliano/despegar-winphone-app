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

            //TODO : FILL PAY AT DESTINATION
            return formated;
        }



        private static void FillFormatedWithoutInterest(List<PaymentDetail> list, PaymentsWithoutInterest paymentsFilter)
        {
            foreach(PaymentDetail item in list )
            {
                switch (item.installments.quantity)
                {
                    case 1:
                        paymentsFilter.OnePay.Add(item);
                        break;
                    case 6:
                        paymentsFilter.SixPays.Add(item);
                        break;
                    case 12:
                        paymentsFilter.TwelvePays.Add(item);
                        break;
                    case 24:
                        paymentsFilter.TwentyFourPays.Add(item);
                        break;
                }
            }
        }

        private static void FillFormatedWithInterest(List<PaymentDetail> list, PaymentsWithInterest paymentsFilter)
        {
            foreach (PaymentDetail item in list)
            {
                ListPays listPays = new ListPays();
                listPays.Cards.Add(item);
                switch (item.installments.quantity)
                {
                    case 1:
                        paymentsFilter.OnePay.Add(listPays);
                        break;
                    case 6:
                        paymentsFilter.SixPays.Add(listPays);
                        break;
                    case 12:
                        paymentsFilter.TwelvePays.Add(listPays);
                        break;
                    case 24:
                        paymentsFilter.TwentyFourPays.Add(listPays);
                        break;
                }
            }

            List<string> availablePayments = new List<string>();

            if (paymentsFilter.OnePay.Count != 0)
            {
                availablePayments.Add("1");
            }
            if (paymentsFilter.SixPays.Count != 0)
            {
                availablePayments.Add("6");
            }
            if (paymentsFilter.TwelvePays.Count != 0)
            {
                availablePayments.Add("12");
            }
            if (paymentsFilter.TwentyFourPays.Count != 0)
            {
                availablePayments.Add("24");
            }
            var input = String.Join(" , ", availablePayments); 
            StringBuilder sb = new StringBuilder(input);
            sb[input.LastIndexOf(',')] = 'o';
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            paymentsFilter.GrupLabelText = sb.ToString() + " " + loader.GetString("Common_Pay_Of");
        }


        //private async void GetCountries()
        //{
            //IConfigurationService configurationService = GlobalConfiguration.CoreContext.GetConfigurationService();
            //Countries con = await configurationService.GetCountries();

        //}
    }
}
