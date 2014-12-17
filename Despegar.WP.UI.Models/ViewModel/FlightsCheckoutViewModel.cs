﻿using Despegar.Core.Business.Common.State;
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
using System.Globalization;
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

        public bool InvoiceRequired { get { return GlobalConfiguration.Site == "AR"  && CoreBookingFields.form.payment.invoice != null; } }

        public bool IsFiscalNameRequired { //HERNAN: No integrar este codigo. Consecuencias: 1 docena de facturas.
            get 
            {
                if (InvoiceRequired)
                {
                    //return false;
                    return CoreBookingFields.form.payment.invoice.fiscal_status.required && CoreBookingFields.form.payment.invoice.fiscal_status.CoreValue != "FINAL";
                }
                else { return false; }
            } 
        }
        public bool IsTermsAndConditionsAccepted { get; set; }

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
                OnPropertyChanged();

                // Select first by default
                SelectedCard = value.FirstOrDefault();
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

                OnPropertyChanged();
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
                if (passanger.document != null && passanger.document.type != null)
                    passanger.document.type.SetDefaultValue();
                if (passanger.gender != null)
                    passanger.gender.SetDefaultValue();
            }

            // Card data
            if (CoreBookingFields.form.payment.card.owner_document != null && CoreBookingFields.form.payment.card.owner_document.type != null)
                CoreBookingFields.form.payment.card.owner_document.type.SetDefaultValue();
            if (CoreBookingFields.form.payment.card.owner_gender != null)
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
                        CoreBookingFields.form.payment.invoice.address.country.SetDefaultValue();

                        // Turn State into a MultipleField
                        CoreBookingFields.form.payment.invoice.address.state.value = null;
                        CoreBookingFields.form.payment.invoice.address.state.options = States.Select(x => new Option() { value= x.id, description = x.name }).ToList();
                        CoreBookingFields.form.payment.invoice.address.state.SetDefaultValue();
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
            if (payments.without_interest != null)
            {
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
            }

            // Without Interest
            if (payments.with_interest != null)
            {
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

            if (availablePayments.Count != 0)
            {
                string input = String.Join(" , ", availablePayments);
                StringBuilder sb = new StringBuilder(input);
                sb[input.LastIndexOf(',')] = 'o';
            

                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                InstallmentFormatted.WithInterest.GrupLabelText = sb.ToString() + " " + loader.GetString("Common_Pay_Of");
            }
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
#if DEBUG
            // Fill Test data
            FillBookingFields(CoreBookingFields);
#endif

            if (!IsTermsAndConditionsAccepted)
            {
                OnViewModelError("TERMS_AND_CONDITIONS_NOT_CHECKED");
                return;
            }

            string sectionID = "";
            // Validation
            if (!CoreBookingFields.IsValid(out sectionID))
            {
                OnViewModelError("FORM_ERROR", sectionID);
            }
            else
            {
                try
                {
                    this.IsLoading = true;
                    dynamic bookingData = null;

                    bookingData = await DynamicFlightBookingFieldsToPost.ToDynamic(this.CoreBookingFields);

                    CrossParameters.PriceDetail = PriceDetailsFormatted;
                    CrossParameters.BookingResponse = await flightService.CompleteBooking(bookingData, CoreBookingFields.id);

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

                        case BookingStatusEnum.booking_failed:
                            {
                                OnViewModelError("BOOKING_FAILED", CrossParameters.BookingResponse.checkout_id);
                                break;
                            }
                        //case BookingStatusEnum.fix_credit_card:
                        case BookingStatusEnum.new_credit_card:
                            {
                                this.selectedCard.card = new Card();
                                this.selectedCard.hasError = true;
                                this.selectedCard.CustomErrorType = BookingStatusEnum.new_credit_card.ToString().ToUpper();
                                this.CoreBookingFields.form.payment.card.number.CoreValue = String.Empty;
                                this.CoreBookingFields.form.payment.card.expiration.CoreValue = String.Empty;
                                this.CoreBookingFields.form.payment.card.security_code.CoreValue = String.Empty;

                                OnPropertyChanged("SelectedCard");
                                OnViewModelError("FORM_ERROR", "CARD");
                                break;
                            }
                        //case BookingStatusEnum.payment_failed:
                        case BookingStatusEnum.risk_review:
                            {
                                EventHandler RiskHandler = ShowRiskReview;
                                if (RiskHandler != null)
                                {
                                    RiskHandler(this, null);
                                }
                                break;
                            }
                        //case BookingStatusEnum.BookingCustomError:
                        default:
                            break;
                    }

                }
                catch (Exception ex)
                {
                    OnViewModelError("COMPLETE_BOOKING_CONECTION_FAILED");
                }
                this.IsLoading = false;
            }
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

        /// <summary>
        /// Test method, DEBUG ONLY
        /// </summary>
        /// <param name="bookingFields"></param>
        /// <returns></returns>
        private static BookingFields FillBookingFields(BookingFields bookingFields)
        {
            bookingFields.form.contact.email.CoreValue = "bookingvuelos@despegar.com";

            bookingFields.form.contact.phones[0].area_code.CoreValue = "11";
            bookingFields.form.contact.phones[0].country_code.CoreValue = "54";
            bookingFields.form.contact.phones[0].number.CoreValue = "44444444";
            bookingFields.form.contact.phones[0].type.CoreValue = "HOME";

            if (bookingFields.form.passengers[0].birthdate != null)
                bookingFields.form.passengers[0].birthdate.CoreValue = "1988-11-27";
            if (bookingFields.form.passengers[0].document != null)
                bookingFields.form.passengers[0].document.type.CoreValue = "LOCAL";
            bookingFields.form.passengers[0].first_name.CoreValue = "Test";
            bookingFields.form.passengers[0].last_name.CoreValue = "Booking";
            if (bookingFields.form.passengers[0].gender != null) { bookingFields.form.passengers[0].gender.CoreValue = "MALE"; }
            bookingFields.form.payment.card.expiration.CoreValue = "2015-11";
            bookingFields.form.payment.card.number.CoreValue = "4242424242424242";

            if (bookingFields.form.payment.card.owner_document != null)
                bookingFields.form.payment.card.owner_document.type.CoreValue = "LOCAL";
            if (bookingFields.form.payment.card.owner_gender != null)
                bookingFields.form.payment.card.owner_gender.CoreValue = "MALE";
            bookingFields.form.payment.card.owner_name.CoreValue = "Test Booking";
            bookingFields.form.payment.card.security_code.CoreValue = "123";
            bookingFields.form.payment.installment.card_code.CoreValue = "VI";
            bookingFields.form.payment.installment.card_type.CoreValue = "CREDIT";
            bookingFields.form.payment.installment.quantity.CoreValue = "1";

            bookingFields.form.payment.installment.complete_card_code.CoreValue = "VI";

            if (bookingFields.form.payment.card.owner_document != null)
                bookingFields.form.payment.card.owner_document.number.CoreValue = "12123123";
            if (bookingFields.form.passengers[0].document != null && bookingFields.form.passengers[0].document.number != null)
                bookingFields.form.passengers[0].document.number.CoreValue = "12123123";



            if (bookingFields.form.passengers[0].nationality != null)
            {
                bookingFields.form.passengers[0].nationality.CoreValue = bookingFields.form.passengers[0].nationality.value;
                switch (bookingFields.form.passengers[0].nationality.value)
                {
                    case "AR":

                        if (bookingFields.form.payment.invoice != null)
                        {
                            bookingFields.form.payment.invoice.address.city_id.CoreValue = "6585";
                            bookingFields.form.payment.invoice.address.country.CoreValue = "AR";
                            bookingFields.form.payment.invoice.address.department.CoreValue = "A";
                            bookingFields.form.payment.invoice.address.number.CoreValue = "1234";
                            bookingFields.form.payment.invoice.address.postal_code.CoreValue = "7777";
                            bookingFields.form.payment.invoice.address.state.CoreValue = "14061";
                            bookingFields.form.payment.invoice.address.street.CoreValue = "La Calle";
                            bookingFields.form.payment.invoice.fiscal_id.CoreValue = "20121231238";
                            bookingFields.form.payment.invoice.fiscal_name.CoreValue = "RazonSocial";
                            bookingFields.form.payment.invoice.fiscal_status.CoreValue = "INSCR";
                        }
                        break;

                    case "BR":
                        bookingFields.form.payment.card.owner_document.number.CoreValue = "85365865596";
                        if (bookingFields.form.passengers[0].document.number != null)
                            bookingFields.form.passengers[0].document.number.CoreValue = "85365865596";
                        break;

                    case "PE":
                        if (bookingFields.form.payment.invoice != null)
                        {
                            bookingFields.form.payment.invoice.address.city.CoreValue = "6585";
                            bookingFields.form.payment.invoice.address.country.CoreValue = "PE";
                            bookingFields.form.payment.invoice.address.state.CoreValue = "14061";
                            bookingFields.form.payment.invoice.address.street.CoreValue = "La Calle";
                            bookingFields.form.payment.invoice.fiscal_id.CoreValue = "20121231238";
                        }
                        break;

                    case "MX":
                        {
                            bookingFields.form.passengers[0].document.type.CoreValue = "PASSPORT";
                            if (bookingFields.form.passengers[0].document.number != null)
                                bookingFields.form.passengers[0].document.number.CoreValue = "12123123";
                            break;
                        }


                    default:

                        if (bookingFields.form.passengers[0].document.number != null)
                            bookingFields.form.passengers[0].document.number.CoreValue = "12123123";
                        break;
                }
            }
            else
            {

            }


            return bookingFields;
        }

    }
}
