using Despegar.Core.Neo.Business.Common.Checkout;
using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Business.Coupons;
using Despegar.Core.Neo.Business.CreditCard;
using Despegar.Core.Neo.Business.Enums;
using Despegar.Core.Neo.Business.Flight.BookingCompletePostResponse;
using Despegar.Core.Neo.Business.Flight.BookingFields;
using Despegar.Core.Neo.Business.Flights;
using Despegar.Core.Neo.Business.Forms;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.Core.Neo.Exceptions;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;

namespace Despegar.WP.UI.Model.ViewModel.Flights
{
    public class FlightsCheckoutViewModel : ViewModelBase
    {
        #region ** Private **
        private ICoreLogger logger;
        private IMAPIFlights flightService;
        private IMAPICross mapiCross;
        private IAPIv1 apiv1Service;
        private IMAPICoupons couponsService;
        private FlightsCrossParameter FlightCrossParameters;
        private ValidationCreditcards creditCardsValidations;
        #endregion

        #region ** Public Interface **
        public FlightBookingFields CoreBookingFields { get; set; }                
        public List<CountryFields> Countries { get; set; }
        public List<Despegar.Core.Neo.Business.Common.State.State> States { get; set; }
        public bool InvoiceRequired
        {
            get
            {
                if (GlobalConfiguration.Site == "AR")                
                    return CoreBookingFields != null ? CoreBookingFields.form.payment.invoice != null : false;                

                return false;
            }
        }
        public List<Despegar.Core.Neo.Business.Flight.BookingCompletePostResponse.RiskQuestion> FreeTextQuestions
        {
            get
            {
                if(FlightCrossParameters.BookingResponse != null)
                {
                    return FlightCrossParameters.BookingResponse.risk_questions.Where(x => x.free_text == "True").ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Despegar.Core.Neo.Business.Flight.BookingCompletePostResponse.RiskQuestion> ChoiceQuestions
        {
            get
            {
                if(FlightCrossParameters.BookingResponse != null)
                {
                    return FlightCrossParameters.BookingResponse.risk_questions.Where(x => x.free_text == "False").ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public bool IsFiscalNameRequired 
        {
            get 
            {
                if (InvoiceRequired)
                {
                    return CoreBookingFields.form.payment.invoice.fiscal_status.required && CoreBookingFields.form.payment.invoice.fiscal_status.CoreValue != "FINAL";
                }
                else { return false; }
            } 
        }

        public bool IsTermsAndConditionsAccepted { get; set; }
        public bool NationalityIsOpen { get; set; }
        private CouponResponse voucherResult;
        public CouponResponse VoucherResult { get { return voucherResult; } set { voucherResult = value; OnPropertyChanged(); } }
        public event EventHandler ShowRiskReview;
        public event EventHandler HideRiskReview;

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
        private InstallmentOption selectedInstallment;
        public InstallmentOption SelectedInstallment { get { return selectedInstallment; } 
            set 
            { 
                selectedInstallment = value;
                OnPropertyChanged();

                // Select first by default
                SelectedCard = value.FirstCard;                
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

                    if (creditCardsValidations != null)
                    {
                        ValidationCreditcard validation = creditCardsValidations.data.FirstOrDefault(x => x.bankCode == (selectedCard.card.bank == "" ? "*" : selectedCard.card.bank) && x.cardCode == selectedCard.card.company);

                        Validation valNumber = new Validation();
                        valNumber.error_code = "NUMBER";
                        valNumber.regex = validation.numberRegex;
                        CoreBookingFields.form.payment.card.number.validations = new List<Validation>();
                        CoreBookingFields.form.payment.card.number.validations.Add(valNumber);

                        Validation valLength = new Validation();
                        valLength.error_code = "LENGTH";
                        valLength.regex = validation.lengthRegex;
                        CoreBookingFields.form.payment.card.number.validations.Add(valLength);

                        Validation valCode = new Validation();
                        valCode.error_code = "CODE";
                        valCode.regex = validation.codeRegex;
                        CoreBookingFields.form.payment.card.security_code.validations = new List<Validation>();
                        CoreBookingFields.form.payment.card.security_code.validations.Add(valCode); //.number.validations.Add(val);
                    }
                }

                OnPropertyChanged(); 
            }
        }

        public Voucher Voucher { get; set; }

        public ICommand SendRiskAnswersCommand
        {
            get
            {
                return new RelayCommand(() => SendRiskAnswers());
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

        public FlightsCheckoutViewModel(INavigator navigator, ICoreLogger logger, IAPIv1 apiv1Service, IMAPIFlights flightServices, IMAPICross mapiCross, IMAPICoupons couponService, IBugTracker t)
            : base(navigator, t)
        {
            this.logger = logger;
            this.flightService = flightServices;
            this.couponsService = couponService;
            this.mapiCross = mapiCross;
            this.apiv1Service = apiv1Service;
        }
        
        /// <summary>
        /// Loads the initial required data for the Checkout Form
        /// </summary>
        public async Task Init()
        {
            BugTracker.LeaveBreadcrumb("Flight checkout view model init");
            IsLoading = true;

            string currentCountry = GlobalConfiguration.Site;
            string deviceId = GlobalConfiguration.UPAId;

            try
            {
                await GetBookingFields(deviceId);
                await LoadCountries();
                await LoadStates(currentCountry);

                // Format Price details / Installments
                FormatInstallments();
                PriceDetailsFormatted = FormatPrice();

                // Set Known Default Values && Adapt Checkout to the country
                ConfigureCountry(currentCountry);

                //Get validations for credit cards
                GetCreditCardsValidations();
                BugTracker.LeaveBreadcrumb("Flight checkout view model init complete");

#if DEBUG
                // Fill Test data
                FillBookingFields(CoreBookingFields);
#endif
            }
            catch (Exception e)
            {                
                logger.Log("[App:FlightsCheckout] Exception " + e.Message);
                IsLoading = false;
                OnViewModelError("CHECKOUT_INIT_FAILED");
            }

            IsLoading = false;

            BugTracker.LeaveBreadcrumb("Flight checkout ready");
        }

        private async void GetCreditCardsValidations()
        {
            try
            {
                BugTracker.LeaveBreadcrumb("Flight checkout view model get credit cards validations");
                creditCardsValidations = await apiv1Service.GetCreditCardValidations();
            }
            catch (Exception e)
            {
                logger.Log("[App:FlightsCheckout] Exception " + e.Message);
                IsLoading = false;
                OnViewModelError("CHECKOUT_INIT_FAILED");
            }
        }

        private void ConfigureCountry(string countryCode)
        {
            BugTracker.LeaveBreadcrumb("Flight checkout view model configure country");
            // Common

            // Passengers
            foreach (Passenger passanger in CoreBookingFields.form.passengers)
            {
                if (passanger.document != null && passanger.document.type != null)
                    passanger.document.type.SetDefaultValue();

                if (passanger.gender != null)
                    passanger.gender.SetDefaultValue();
            }

            // Contact
            if (CoreBookingFields.form.contact.Phone != null)
               CoreBookingFields.form.contact.Phone.type.SetDefaultValue();

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

                    // Invoice Arg
                    if (InvoiceRequired)
                    {
                        CoreBookingFields.form.payment.invoice.fiscal_status.PropertyChanged += Fiscal_status_PropertyChanged;

                        CoreBookingFields.form.payment.invoice.fiscal_status.SetDefaultValue();
                        CoreBookingFields.form.payment.invoice.address.country.SetDefaultValue();

                        // Turn State into a MultipleField
                        CoreBookingFields.form.payment.invoice.address.state.value = null;
                        CoreBookingFields.form.payment.invoice.address.state.options = States.Select(x => new Option() { value = x.id, description = x.name }).ToList();
                        CoreBookingFields.form.payment.invoice.address.state.SetDefaultValue();
                    }

                    CoreBookingFields.form.contact.phones[0].country_code.SetDefaultValue();
                    CoreBookingFields.form.contact.phones[0].area_code.SetDefaultValue();
                    break;
            }
            BugTracker.LeaveBreadcrumb("Flight checkout view model configure country complete");
        }

        #region ** Private Misc ** 

        private void Fiscal_status_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CoreValue")
            {
                OnPropertyChanged("IsFiscalNameRequired");
            }
        }

        private async Task GetBookingFields(string deviceID)
        {
            BugTracker.LeaveBreadcrumb("Flight checkout view model get booking fields init" );

            FlightsBookingFieldRequest book = new FlightsBookingFieldRequest();

            if (FlightCrossParameters.Inbound.choice != -1)
                book.inbound_choice = FlightCrossParameters.Inbound.choice; //-1
            else
                book.inbound_choice = null;
            
            if (FlightCrossParameters.Outbound.choice != 0) //TODO: Verificar por que es 0 en multiples
                book.outbound_choice = FlightCrossParameters.Outbound.choice;
            else
                book.outbound_choice = null;

            book.itinerary_id = FlightCrossParameters.FlightId;
            book.mobile_identifier = deviceID;

            CoreBookingFields = await flightService.GetBookingFields(book);

            BugTracker.LeaveBreadcrumb("Flight checkout view model get booking fields complete");
        }

        private async Task LoadCountries()
        {
            Countries = (await mapiCross.GetCountries()).countries;
        }

        private async Task LoadStates(string countryCode)
        {
            States = await mapiCross.GetStates(countryCode);
        }

        // Public because it is used from the InvoiceArg control
        public async Task<List<Despegar.Core.Neo.Business.Configuration.CitiesFields>> GetCities(string countryCode, string search, string cityresult)
        {
            return await mapiCross.AutoCompleteCities(countryCode, search, cityresult);
        }

        /// <summary>
        /// Format Credit Cards installments
        /// </summary>
        /// <returns></returns>
        private void FormatInstallments()
        {
            Payments payments = CoreBookingFields.payments;
            InstallmentFormatted = new InstallmentFormatted();

            // Without interest
            if (payments.without_interest != null)
            {
                foreach (PaymentDetail item in payments.without_interest)
                {
                    InstallmentFormatted.AddWithouInterestInstallment(item);                    
                }
            }

            // With Interest
            if (payments.with_interest != null)
            {
                foreach (PaymentDetail item in payments.with_interest)
                {
                    InstallmentFormatted.AddWithInterestInstallment(item);     
                }
            }
            
            if (InstallmentFormatted.WithInterest.Count != 0)
            {                
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                InstallmentFormatted.ResourceLabel = loader.GetString("Common_Pay_Of");
            }
        }

        /// <summary>
        /// Payment Details
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        private PriceFormated FormatPrice()
        {
            BugTracker.LeaveBreadcrumb("Flight checkout view model format price init");

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

            BugTracker.LeaveBreadcrumb("Flight checkout view model format price complete");

            return formated;
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
        private static FlightBookingFields FillBookingFields(FlightBookingFields bookingFields)
        {
            bookingFields.form.contact.email.CoreValue = "bookingvuelos@despegar.com";
            bookingFields.form.contact.emailConfirmation.CoreValue = "bookingvuelos@despegar.com";
            bookingFields.form.contact.phones[0].area_code.CoreValue = "11";
            bookingFields.form.contact.phones[0].country_code.CoreValue = "54";
            bookingFields.form.contact.phones[0].number.CoreValue = "44444444";
            bookingFields.form.contact.phones[0].type.CoreValue = "HOME";
            if (bookingFields.form.passengers[0].birthdate != null)
            {
                bookingFields.form.passengers[0].birthdate.CoreValue = "1988-11-27";
            }
            bookingFields.form.passengers[0].document.number.CoreValue = "12123123";
            bookingFields.form.passengers[0].document.type.CoreValue = "LOCAL";
            bookingFields.form.passengers[0].first_name.CoreValue = "Test";
            bookingFields.form.passengers[0].last_name.CoreValue = "Booking";
            bookingFields.form.passengers[0].gender.CoreValue = "MALE";
            bookingFields.form.passengers[0].nationality.CoreValue = "AR";
            bookingFields.form.payment.card.expiration.CoreValue = "2015-11";
            bookingFields.form.payment.card.number.CoreValue = "4242424242424245";
            bookingFields.form.payment.card.owner_document.number.CoreValue = "12123123";
            bookingFields.form.payment.card.owner_document.type.CoreValue = "LOCAL";
            if (bookingFields.form.payment.card.owner_gender != null)
                bookingFields.form.payment.card.owner_gender.CoreValue = "MALE";
            bookingFields.form.payment.card.owner_name.CoreValue = "Test Booking";
            bookingFields.form.payment.card.security_code.CoreValue = "123";
            bookingFields.form.payment.installment.card_code.CoreValue = "VI";
            bookingFields.form.payment.installment.card_type.CoreValue = "CREDIT";
            bookingFields.form.payment.installment.quantity.CoreValue = "1";
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
            bookingFields.form.payment.installment.complete_card_code.CoreValue = "VI";
            return bookingFields;
        }

        #endregion

        private async void ValidateAndBuy()
        {
            BugTracker.LeaveBreadcrumb("Flight checkout view model validate and buy init");

            if (!IsTermsAndConditionsAccepted)
            {
                OnViewModelError("TERMS_AND_CONDITIONS_NOT_CHECKED");
                BugTracker.LeaveBreadcrumb("Flight checkout buy terms not accepted");
                return;
            }

            string sectionID = "";

            // Validation
            if (!CoreBookingFields.IsValid(out sectionID))
            {
                BugTracker.LeaveBreadcrumb("Flight checkout ViewModel invalid fields");
                OnViewModelError("FORM_ERROR", sectionID);
            }
            else
            {
                try
                {
                    this.IsLoading = true;
                    object bookingData = null;

                    bookingData = await BookingFormBuilder.BuildFlightsForm(this.CoreBookingFields);

                    // Buy
                    FlightCrossParameters.PriceDetail = PriceDetailsFormatted;
                    FlightCrossParameters.BookingResponse = await flightService.CompleteBooking(bookingData, CoreBookingFields.id);

                    if (FlightCrossParameters.BookingResponse.Error != null) 
                    {
                        BugTracker.LeaveBreadcrumb("Flight checkout MAPI booking error response code: " + FlightCrossParameters.BookingResponse.Error.code.ToString());
                        // API Error ocurred, Check CODE and inform the user
                        OnViewModelError("API_ERROR", FlightCrossParameters.BookingResponse.Error.code);
                        this.IsLoading = false;
                        return;
                    }

                    // Booking processed, check the status of Booking request
                    AnalizeBookingStatus(FlightCrossParameters.BookingResponse.booking_status);
                }
                catch (HTTPStatusErrorException )
                {
                    OnViewModelError("COMPLETE_BOOKING_CONECTION_FAILED");
                }
                catch (Exception )
                {
                    OnViewModelError("COMPLETE_BOOKING_BOOKING_FAILED"); 
                }

                BugTracker.LeaveBreadcrumb("Flight checkout view model validate and buy complete");
                this.IsLoading = false;
            }
        }

        private async void SendRiskAnswers()
        {
            BugTracker.LeaveBreadcrumb("Flight checkout view model Risk init");
            ResourceLoader manager = new ResourceLoader();

            if (ValidateAnswers())
            {
                this.IsLoading = true;
                List<Despegar.Core.Neo.Business.Flight.BookingCompletePost.RiskAnswer> answers = new List<Despegar.Core.Neo.Business.Flight.BookingCompletePost.RiskAnswer>();
                BookingCompletePostResponse BookingResponse = new BookingCompletePostResponse();

                foreach (RiskQuestion question in FreeTextQuestions)
                {
                    question.risk_answer.question_id = question.id;
                    answers.Add(question.risk_answer);
                }

                foreach (RiskQuestion question in ChoiceQuestions)
                {
                    question.risk_answer.question_id = question.id;
                    question.risk_answer.answer_id = question.risk_answer.answer_id;
                    answers.Add(question.risk_answer);
                }

                dynamic result = new ExpandoObject();
                result.form = new ExpandoObject();
                result.form.risk_questions = answers;
                result.form.booking_status = "risk_review";

                BookingResponse = await flightService.CompleteBooking(result, CoreBookingFields.id);
                this.IsLoading = false;

                EventHandler AnswerHandler = HideRiskReview;
                if (AnswerHandler != null)
                {
                    AnswerHandler(this, null);
                }

                AnalizeBookingStatus(BookingResponse.booking_status);

            }
            else
            {
                var msg = new MessageDialog(manager.GetString("Flight_Checkout_Risk_Error"));
                await msg.ShowAsync();
            }
            BugTracker.LeaveBreadcrumb("Flight checkout view model Risk complete");

        }

        private bool ValidateAnswers()
        {
            foreach (RiskQuestion question in FlightCrossParameters.BookingResponse.risk_questions)
            {
                if (question.risk_answer.text == null || question.risk_answer.text == "")
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Validates the booking status
        /// </summary>
        private void AnalizeBookingStatus(string status)
        {
            BugTracker.LeaveBreadcrumb("Flight checkout view model booking status" + status);

            switch (GetStatus(status))
            {
                case BookingStatusEnum.checkout_successful:

                    Navigator.GoTo(ViewModelPages.FlightsThanks, FlightCrossParameters);
                    break;

                case BookingStatusEnum.booking_failed:

                    OnViewModelError("BOOKING_FAILED", FlightCrossParameters.BookingResponse.checkout_id);
                    break;

                case BookingStatusEnum.fix_credit_card:

                    this.CoreBookingFields.form.booking_status = "fix_credit_card";
                    OnViewModelError("ONLINE_PAYMENT_ERROR_FIX_CREDIT_CARD", "CARD");
                    break;

                case BookingStatusEnum.new_credit_card:

                    this.CoreBookingFields.form.payment.card.number.CoreValue = String.Empty;
                    this.CoreBookingFields.form.payment.card.expiration.CoreValue = String.Empty;
                    this.CoreBookingFields.form.payment.card.security_code.CoreValue = String.Empty;
                    this.CoreBookingFields.form.booking_status = "new_credit_card";

                    OnPropertyChanged("SelectedCard");
                    OnViewModelError("ONLINE_PAYMENT_ERROR_NEW_CREDIT_CARD", "INSTALLMENT");
                    break;

                case BookingStatusEnum.payment_failed:
                case BookingStatusEnum.risk_evaluation_failed:

                    OnViewModelError("ONLINE_PAYMENT_FAILED", "CARD");
                    break;

                case BookingStatusEnum.risk_review:

                    EventHandler RiskHandler = ShowRiskReview;
                    if (RiskHandler != null)
                    {
                        RiskHandler(this, null);
                    }
                    break;

                //case BookingStatusEnum.BookingCustomError:
                default:
                    break;
            }
        }

        /// <summary>
        /// Validates the reference code against the service and sets the Validation errors or succcess
        /// </summary>
        public async void ValidateVoucher()
        {
            BugTracker.LeaveBreadcrumb("Flight checkout view model validate voucher init");

            IsLoading = true;
            ResourceLoader loader = new ResourceLoader();
            var field = CoreBookingFields.form.Voucher;

            field.IsApplied = false;

            if (!String.IsNullOrWhiteSpace(field.CoreValue))
            {
                CouponParameter parameter = new CouponParameter()
                {
                    Beneficiary = CoreBookingFields.form.contact.email != null ? CoreBookingFields.form.contact.email.CoreValue : "",
                    TotalAmount = CoreBookingFields.price.total.ToString(),
                    CurrencyCode = CoreBookingFields.price.currency.code,
                    Product = "flight",
                    Quotation = String.Format(CultureInfo.InvariantCulture, "{0:0.#################}", CoreBookingFields.price.currency.ratio),
                    ReferenceCode = field.CoreValue,
                };

                VoucherResult = await couponsService.Validity(parameter);

                if (!VoucherResult.Error.HasValue)
                    field.IsApplied = true; // Voucher OK!
                else
                {
                    // Notify Coupon Error
                    field.IsApplied = false;
                    OnViewModelError("VOUCHER_VALIDITY_ERROR", VoucherResult.Error.ToString());
                    VoucherResult = null;
                }                
            }

            field.Validate();
            IsLoading = false;

            BugTracker.LeaveBreadcrumb("Flight checkout view model validate voucher complete");
        }

        public override void OnNavigated(object navigationParams)
        {
            BugTracker.LeaveBreadcrumb("Flight checkout start");
            FlightCrossParameters = navigationParams as FlightsCrossParameter;            
        }
    }
}