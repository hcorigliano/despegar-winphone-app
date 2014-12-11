using Despegar.Core.Business.Common.State;
using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Dynamics;
using Despegar.Core.Business.Enums;
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
        #endregion

        #region ** Public Interface **
        public BookingFields CoreBookingFields { get; set; }

        // Only Invoice Arg fields for now
        public List<CountryFields> Countries { get; set; }
        public List<State> States { get; set; }

        public bool InvoiceRequired { get { return CoreBookingFields.form.payment.invoice != null; } }

        public bool IsFiscalNameRequired { //HERNAN: No integrar este codigo. Consecuencias: 1 docena de facturas.
            get 
            {
                if (InvoiceRequired)
                {
                    return CoreBookingFields.form.payment.invoice.fiscal_status.required && CoreBookingFields.form.payment.invoice.fiscal_status.CoreValue != "FINAL";
                }
                else { return false; }
            } 
        }

        public event EventHandler ShowRiskReview;

        /// <summary>
        /// For Details section
        /// </summary>
        private PriceFormated priceDetailsFormatted;
        public PriceFormated PriceDetailsFormatted
        {
            get { return priceDetailsFormatted; }
            set
            {
                priceDetailsFormatted = value;
                OnPropertyChanged();
            }
        }

        public bool NationalityIsOpen{get; set;}
     
        private InstallmentFormatted installmentFormatted;
        public InstallmentFormatted InstallmentFormatted
        {
            get { return installmentFormatted; }
            set
            {
                installmentFormatted = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected "RadioButton" payment strategy
        /// </summary>
        private List<PaymentDetail> seletedInstallment;
        public List<PaymentDetail> SelectedInstallment { get { return seletedInstallment; } 
            set 
            { 
                seletedInstallment = value;
              
                // Select first by default
                SelectedCard = value.FirstOrDefault();

                OnPropertyChanged();
                OnPropertyChanged("CurrentCards");
            } 
        }

        private PaymentDetail selectedCard;
        public PaymentDetail SelectedCard 
        {
            get { return selectedCard; }
            set 
            {
                selectedCard = value;
                
                // Set POST data
                if (selectedCard != null)
                {
                    var payments = CoreBookingFields.form.payment;
                    payments.installment.bank_code.CoreValue = selectedCard.card.bank;
                    payments.installment.quantity.CoreValue = selectedCard.installments.quantity.ToString();
                    payments.installment.card_code.CoreValue = selectedCard.card.code;
                    payments.installment.card_code.CoreValue = selectedCard.card.company;
                    payments.installment.card_type.CoreValue = selectedCard.card.type;
                    payments.installment.complete_card_code.CoreValue = selectedCard.card.code;
                }

                OnPropertyChanged(); // TODO: no esta actualizando el ComboBox de Tarjeta
            }
        }

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

                // Format Price details / Installments
                FormatInstallments();
                PriceDetailsFormatted = FormatPrice();

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

            // Passengers
            foreach (var passanger in CoreBookingFields.form.passengers)
            {
                passanger.document.type.SetDefaultValue();
                passanger.gender.SetDefaultValue();
            }

            // Card data
            CoreBookingFields.form.payment.card.owner_document.type.SetDefaultValue(); 
            CoreBookingFields.form.payment.card.owner_gender.SetDefaultValue(); 
          
            switch (countryCode)
            {
                case "AR":
                    // Passengers
                    foreach (var passanger in CoreBookingFields.form.passengers)
                    {
                        passanger.nationality.CoreValue = "AR";
                    }
                   
                    // Contact
                    CoreBookingFields.form.contact.Phone.type.SetDefaultValue();

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

        /// <summary>
        /// Format Credit Cards installments
        /// </summary>
        /// <returns></returns>
        private void FormatInstallments()
        {            
            Payments payments = CoreBookingFields.payments;
            InstallmentFormatted = new InstallmentFormatted();

            // With interest
            foreach (PaymentDetail item in payments.without_interest)
            {
                switch (item.installments.quantity)
                {
                    case 1:
                        InstallmentFormatted.WithoutInterest.OnePay.Add(item);
                        break;
                    case 6:
                        InstallmentFormatted.WithoutInterest.SixPays.Add(item);
                        break;
                    case 12:
                        InstallmentFormatted.WithoutInterest.TwelvePays.Add(item);
                        break;
                    case 24:
                        InstallmentFormatted.WithoutInterest.TwentyFourPays.Add(item);
                        break;
                }
            }

            // Without Interest
            foreach (PaymentDetail item in payments.with_interest)
            {              
                switch (item.installments.quantity)
                {
                    case 1:
                        InstallmentFormatted.WithInterest.OnePay.Add(item);
                        break;
                    case 6:
                        InstallmentFormatted.WithInterest.SixPays.Add(item);
                        break;
                    case 12:
                        InstallmentFormatted.WithInterest.TwelvePays.Add(item);
                        break;
                    case 24:
                        InstallmentFormatted.WithInterest.TwentyFourPays.Add(item);
                        break;
                }
            }

            List<string> availablePayments = new List<string>();

            if (InstallmentFormatted.WithInterest.OnePay.Count != 0)
                availablePayments.Add("1");

            if (InstallmentFormatted.WithInterest.SixPays.Count != 0)
                availablePayments.Add("6");

            if (InstallmentFormatted.WithInterest.TwelvePays.Count != 0)
                availablePayments.Add("12");

            if (InstallmentFormatted.WithInterest.TwentyFourPays.Count != 0)
                availablePayments.Add("24");
            
            string input = String.Join(" , ", availablePayments);
            StringBuilder sb = new StringBuilder(input);
            sb[input.LastIndexOf(',')] = 'o';

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            InstallmentFormatted.WithInterest.GrupLabelText = sb.ToString() + " " + loader.GetString("Common_Pay_Of");

            //TODO : FILL PAY AT DESTINATION
        }

        /// <summary>
        /// Payment Details
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        private PriceFormated FormatPrice()
        {
            PriceFormated formated = new PriceFormated();

            formated.currency = CoreBookingFields.price.currency;
            formated.total = CoreBookingFields.price.total;
            formated.taxes = CoreBookingFields.price.taxes;
            formated.retention = CoreBookingFields.price.retention;
            formated.charges = CoreBookingFields.price.charges;
            formated.adult_base = CoreBookingFields.price.adult_base;
            formated.adults_subtotal = CoreBookingFields.price.adults_subtotal;
            formated.children_subtotal = CoreBookingFields.price.children_subtotal;
            formated.infants_subtotal = CoreBookingFields.price.infants_subtotal;
            formated.final_price = CoreBookingFields.price.final_price;

            formated.children_quantity = CoreBookingFields.form.passengers.Count(p => p.type == "CHILD").ToString();
            formated.infant_quantity = CoreBookingFields.form.passengers.Count(p => p.type == "INFANT").ToString();
            formated.adult_quantity = CoreBookingFields.form.passengers.Count(p => p.type == "ADULT").ToString();

            if (formated.children_subtotal != null)
                formated.children_base = (formated.children_subtotal / Convert.ToInt32(formated.children_quantity)).ToString();
            if (formated.infants_subtotal != null)
                formated.infant_base = (formated.infants_subtotal / Convert.ToInt32(formated.infant_quantity)).ToString();

            return formated;
        }

        #endregion

        private async void ValidateAndBuy() 
        {
            this.IsLoading = true; 
            CrossParameters.PriceDetail = PriceDetailsFormatted;
            dynamic objectToSerialize = DynamicFlightBookingFieldsToPost.ToDynamic(this.CoreBookingFields);
            CrossParameters.BookingResponse = await flightService.CompleteBooking(objectToSerialize, CoreBookingFields.id);
            //BookingCompletePostResponse response = await flightService.CompleteBooking(form, "214ecbd4-7964-11e4-8980-fa163ec96567");
            //TODO : Go to Tks or Risk Questions}
            //if (CrossParameters.BookingResponse.booking_status == "checkout_successful")            
            //navigator.GoTo(ViewModelPages.FlightsThanks, CrossParameters);     

            switch (GetStatus(CrossParameters.BookingResponse.booking_status))
            {
                case BookingStatusEnum.checkout_successful:
                    {
                        navigator.GoTo(ViewModelPages.FlightsThanks, CrossParameters);
                        break;
                    }
                //Please uncomment the case that you are to use.

                //case BookingStatusEnum.booking_failed:
                //case BookingStatusEnum.fix_credit_card:
                case BookingStatusEnum.new_credit_card:
                    {
                        this.selectedCard.hasError = true;
                        this.selectedCard.CustomErrorType = BookingStatusEnum.new_credit_card.ToString();
                        ClearCreditCardFields();
                        //GotFocusOnCreditCardData();
                        break;
                    }
                //case BookingStatusEnum.payment_failed:
                case BookingStatusEnum.risk_review:
                    {
                        EventHandler RiskHandler = ShowRiskReview;
                        if(RiskHandler != null)
                        {
                            RiskHandler(this, null);
                        }
                        break;
                    }
                //case BookingStatusEnum.BookingCustomError:
                default:
                    break;
            }
            this.IsLoading = false;

        }

        private void ClearCreditCardFields()
        {
            this.selectedCard.card = new Card();

            OnPropertyChanged("SelectedCard");
        }

        private BookingStatusEnum GetStatus(string status)
        {
            try
            {
                BookingStatusEnum _status = (BookingStatusEnum)Enum.Parse(typeof(BookingStatusEnum), status);

                return _status;
            }
            catch (Exception)
            {

                return BookingStatusEnum.BookingCustomError;
            }

        }       
    }
}
