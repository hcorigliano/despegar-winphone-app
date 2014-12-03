using Despegar.Core.Business.Common.State;
using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Flight.BookingCompletePostResponse;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.IService;
using Despegar.WP.UI.Model.Classes.Flights.Checkout;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using Despegar.WP.UI.Models.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model
{
    public class FlightsCheckoutModel : ViewModelBase
    {
        #region 
        FlightsCrossParameter CrossParameters = new FlightsCrossParameter();

        private IFlightService flightService;
        private ICommonServices CommonServices;

        public Countries countries { get; set; }

        private BookingFields CoreBookingFields;

        public BookingFields bookingfields
        {
            get { return CoreBookingFields; }
            set
            {
                CoreBookingFields = value;
                OnPropertyChanged();
            }
        }

        private PaymentsFormated CorePaymentFormated;
        public PaymentsFormated PaymentFormated
        {
            get { return CorePaymentFormated; }
            set
            {
                CorePaymentFormated = value;
                OnPropertyChanged();
            }
        }

        private PriceFormated CorePriceFormated;
        public PriceFormated PriceFormated
        {
            get { return CorePriceFormated; }
            set
            {
                CorePriceFormated = value;
                OnPropertyChanged();
            }
        }

        public ICommand FormatPaymentsCommand
        {
            get
            {
                return new RelayCommand(() => { FormatPayments(); });
            }
        }


        public ICommand GetCountriesCommand
        {
            get
            {
                return new RelayCommand(() => GetCountries());
            }
        }
        #endregion

        public FlightsCheckoutModel(INavigator navigator, FlightsCrossParameter parameters)
        {
            flightService = GlobalConfiguration.CoreContext.GetFlightService();
            CommonServices = GlobalConfiguration.CoreContext.GetCommonService();
            CrossParameters = parameters;
            GetBookingFields();
        }
        public FlightsCheckoutModel()
        {
            flightService = GlobalConfiguration.CoreContext.GetFlightService();
            CommonServices = GlobalConfiguration.CoreContext.GetCommonService();

        }

        public async void GetBookingFields()
        {
            IsLoading = true;

            BookingFieldPost book = new BookingFieldPost();
            book.inbound_choice = CrossParameters.Inbound.choice;
            book.outbound_choice = CrossParameters.Outbound.choice;
            book.itinerary_id = CrossParameters.FlightId;//"prism_AR_0_FLIGHTS_A-1_C-0_I-0_RT-BUEMIA20141110-MIABUE20141111_xorigin-api!0!C_1212636001_843603426_-2008006059_1555498055_-278056197_804297563!1,6_1,4_1,5_1,2_1,3_1,1";
            
            //MOCKED
            //book.inbound_choice = 1;
            //book.outbound_choice = 1;
            //book.itinerary_id = "prism_AR_0_FLIGHTS_A-1_C-0_I-0_RT-BUEMIA20150610-MIABUE20150611_xorigin-api!0!C_2075990976!1,1";

            bookingfields = await flightService.GetBookingFields(book);

            FillsValueWithCoreValue(bookingfields);

            CorePaymentFormated = FormatPayments();

            CorePriceFormated = PriceFormatedConverter(bookingfields);

            GetCountries();

            IsLoading = false;

        }

        private void FillsValueWithCoreValue(BookingFields CoreBookingFields)
        {
            CoreBookingFields.form.contact.phones[0].country_code.coreValue = CoreBookingFields.form.contact.phones[0].country_code.value;
            CoreBookingFields.form.contact.phones[0].area_code.coreValue = CoreBookingFields.form.contact.phones[0].area_code.value;
        }

        public async Task<BookingFields> GetBookingFields(BookingFieldPost bookingFieldPost)
        {           
            return (await flightService.GetBookingFields(bookingFieldPost));
        }
        
        public async void CompleteCheckOut(dynamic form)
        {
            BookingCompletePostResponse response = await flightService.CompleteBooking(form, bookingfields.id);
            //BookingCompletePostResponse response = await flightService.CompleteBooking(form, "214ecbd4-7964-11e4-8980-fa163ec96567");
            //TODO : Go to Tks or Risk Questions
            int i = 1;
        }

        public async Task<List<State>> GetStates(string country)
        {
            return (await CommonServices.GetStates(country));
        }

        public PaymentsFormated FormatPayments()
        {
            //TODO: REFACTOR
            //Payments payments   +  static
            Payments payments = bookingfields.payments; //added line
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

        private async void GetCountries()
        {
            IConfigurationService configurationService = GlobalConfiguration.CoreContext.GetConfigurationService();
            countries = await configurationService.GetCountries();

        }

        public static async Task<List<CitiesFields>> GetCities(string CountryCode, string Search, string cityresult)
        {
            IConfigurationService configurationService = GlobalConfiguration.CoreContext.GetConfigurationService();
            return await configurationService.AutoCompleteCities(CountryCode, Search, cityresult);
        }

        private PriceFormated PriceFormatedConverter(BookingFields booking)
        {
            PriceFormated formated = new PriceFormated();

            formated.currency = booking.price.currency;
            formated.total = booking.price.total;
            formated.taxes = booking.price.taxes;
            formated.retention = booking.price.retention;
            formated.charges = booking.price.charges;
            formated.adult_base = booking.price.adult_base;
            formated.adults_subtotal = booking.price.adults_subtotal;
            formated.children_subtotal = booking.price.children_subtotal;
            formated.infants_subtotal = booking.price.infants_subtotal;
            formated.final_price = booking.price.final_price;

            formated.children_quantity = booking.form.passengers.Count(p => p.type == "CHILD").ToString();
            formated.infant_quantity = booking.form.passengers.Count(p => p.type == "INFANT").ToString();
            formated.adult_quantity = booking.form.passengers.Count(p => p.type == "ADULT").ToString();

            if (formated.children_subtotal != null)
                formated.children_base = (formated.children_subtotal / Convert.ToInt32(formated.children_quantity)).ToString();
            if (formated.infants_subtotal != null)
                formated.infant_base = (formated.infants_subtotal / Convert.ToInt32(formated.infant_quantity)).ToString();

            return formated;
        }
    }
}
