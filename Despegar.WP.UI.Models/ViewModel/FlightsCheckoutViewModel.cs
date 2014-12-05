using Despegar.Core.Business.Common.State;
using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Dynamics;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.IService;
using Despegar.Core.Log;
using Despegar.WP.UI.Model.Classes.Flights.Checkout;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model.ViewModel
{
    public class FlightsCheckoutViewModel : ViewModelBase
    {        
        #region ** Private **
        private INavigator navigator;        
        private IFlightService flightService;
        private ICommonServices commonServices;
        private IConfigurationService configurationService;        
        private FlightsCrossParameter CrossParameters;

        // En DUDA
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
        // en duda
        #endregion

        #region ** Public Interface **
        public BookingFields CoreBookingFields { get; set; }

        // Only Invoice Arg fields for now
        public List<CountryFields> Countries { get; set; }
        public List<State> States { get; set; }

        public bool InvoiceRequired { get { return CoreBookingFields.form.payment.invoice != null; } }

        public bool IsFiscalNameRequired { get { return CoreBookingFields.form.payment.invoice.fiscal_status.required && CoreBookingFields.form.payment.invoice.fiscal_status.CoreValue != "FINAL"; } }

        public ICommand ValidateAndBuyCommand
        { 
           get 
           {
               return new RelayCommand(() => ValidateAndBuy());
           } 
        }    
        #endregion

        public FlightsCheckoutViewModel(INavigator navigator, IFlightService flightServices, ICommonServices commonServices, IConfigurationService configService, FlightsCrossParameter parameters)
        {
            this.navigator = navigator;
            this.flightService = flightServices;
            this.commonServices = commonServices;
            this.configurationService = configService;
            this.CrossParameters = parameters;
        }
        
        /// <summary>
        /// Loads the initial required data for the Checkout Form
        /// </summary>
        public async Task Init()
        {
            IsLoading = true;

            string currentCountry = GlobalConfiguration.Site;
            try
            {
                await GetBookingFields();
                await LoadCountries();           
                await LoadStates(currentCountry);

                // Format Payments
                CorePaymentFormated = FormatPayments();
                CorePriceFormated = PriceFormatedConverter(CoreBookingFields);

                // Set Known Default Values && Adapt Checkout to the country
                ConfigureCountry(currentCountry);                
            }
            catch (Exception e)
            {
                Logger.Log("[App:FlightsCheckout] Exception " + e.Message);
                IsLoading = false;
                OnViewModelError("CHECKOUT_INIT_FAILED");
            }

            IsLoading = false;
        }

        private void ConfigureCountry(string countryCode)
        {
            // Common
            foreach (var passanger in CoreBookingFields.form.passengers)
            {
                passanger.document.type.SetDefaultValue();
                passanger.gender.SetDefaultValue();
            }

            // Country Specific
            switch (countryCode)
            {
                case "AR":
                    foreach (var passanger in CoreBookingFields.form.passengers)
                    {
                        passanger.nationality.CoreValue = "AR";
                    }
                   
                    // Invoice Arg
                    if (InvoiceRequired)
                    {
                        CoreBookingFields.form.payment.invoice.fiscal_status.PropertyChanged += Fiscal_status_PropertyChanged;

                        CoreBookingFields.form.payment.invoice.fiscal_status.SetDefaultValue();
                        CoreBookingFields.form.payment.invoice.address.state.CoreValue = States.FirstOrDefault().id; // NOT WORKING, MUST BE DONE AFTER THIS CODE or use A RegularOptionsField (The Source works bad)
                    }                    

                    CoreBookingFields.form.contact.phones[0].country_code.SetDefaultValue();
                    CoreBookingFields.form.contact.phones[0].area_code.SetDefaultValue();
               break;
            }
        }

        private void Fiscal_status_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CoreValue") 
            {
                OnPropertyChanged("IsFiscalNameRequired");
            }
        }
       
        private async Task GetBookingFields()
        {            
            BookingFieldPost book = new BookingFieldPost();

            // HARDCODE TEST ARG
            //book.inbound_choice = 1;
            //book.outbound_choice = 1;
            //book.itinerary_id = "prism_AR_0_FLIGHTS_A-1_C-0_I-0_RT-BUEMIA20141110-MIABUE20141111_xorigin-api!0!C_1212636001_843603426_-2008006059_1555498055_-278056197_804297563!1,6_1,4_1,5_1,2_1,3_1,1";

            book.inbound_choice = CrossParameters.Inbound.choice;
            book.outbound_choice = CrossParameters.Outbound.choice;
            book.itinerary_id = CrossParameters.FlightId;

            CoreBookingFields = await flightService.GetBookingFields(book);
        }

        private async Task LoadCountries()
        {
            Countries = (await configurationService.GetCountries()).countries;
        }
        
        private async Task LoadStates(string countryCode)
        {
            States = await commonServices.GetStates(countryCode);
        }

        // Public because it is used from the InvoiceArg control
        public async Task<List<CitiesFields>> GetCities(string CountryCode, string Search, string cityresult)
        {            
            return await configurationService.AutoCompleteCities(CountryCode, Search, cityresult);
        }

        #region ** Utils ** 

        private PaymentsFormated FormatPayments()
        {
            //TODO: REFACTOR 
            Payments payments = CoreBookingFields.payments; //added line
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

        #endregion

        private async void ValidateAndBuy() 
        {
            dynamic objectToSerialize = DynamicFlightBookingFieldsToPost.ToDynamic(this.CoreBookingFields);
            CrossParameters.price = PriceFormated;
            CrossParameters.BookingResponse = await flightService.CompleteBooking(objectToSerialize, CoreBookingFields.id);
            //BookingCompletePostResponse response = await flightService.CompleteBooking(form, "214ecbd4-7964-11e4-8980-fa163ec96567");
            //TODO : Go to Tks or Risk Questions}

            if (CrossParameters.BookingResponse.booking_status == "checkout_successful")            
                navigator.GoTo(ViewModelPages.FlightsThanks, CrossParameters);            
        }
    }
}