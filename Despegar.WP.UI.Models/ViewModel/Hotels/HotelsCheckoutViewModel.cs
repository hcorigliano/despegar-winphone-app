using Despegar.Core.Neo.Business.Common.Checkout;
using Despegar.Core.Neo.Business.Common.State;
using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Business.Coupons;
using Despegar.Core.Neo.Business.CreditCard;
using Despegar.Core.Neo.Business.Enums;
using Despegar.Core.Neo.Business.Forms;
using Despegar.Core.Neo.Business.Hotels;
using Despegar.Core.Neo.Business.Hotels.BookingCompletePostResponse;
using Despegar.Core.Neo.Business.Hotels.BookingFields;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.Core.Neo.Exceptions;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsCheckoutViewModel : ViewModelBase
    {
        #region ** Private **
        private ICoreLogger logger;
        private IAPIv1 apiV1service; 
        private IMAPIHotels hotelService;
        private IMAPICross commonServices;
        private IMAPICoupons couponsService;
        private ValidationCreditcards creditCardsValidations;
        private HotelsCrossParameters crossParams;
        #endregion

        #region ** Public Interface **
        //public RoomsSelected
        public HotelsBookingFields CoreBookingFields { get; set; }
        public List<CountryFields> Countries { get; set; }
        public List<State> States { get; set; }
        public bool InvoiceRequired
        {
            get
            {
                if (GlobalConfiguration.Site == "AR")
                    return CoreBookingFields != null ? CoreBookingFields.form.Invoice != null : false;
                return false;
            }
        }

        //public 
        public bool IsFiscalNameRequired
        {
            get
            {
                if (InvoiceRequired)
                {
                    //return CoreBookingFields.form.Invoice.fiscal_status != null && CoreBookingFields.form.Invoice.fiscal_name != null  && CoreBookingFields.form.Invoice.fiscal_status.CoreValue != "FINAL";                                      
                    return CoreBookingFields.CheckoutMethodSelected.payment.invoice.fiscal_status != null && CoreBookingFields.CheckoutMethodSelected.payment.invoice.fiscal_name != null && CoreBookingFields.form.Invoice.fiscal_status.CoreValue != "FINAL_CONSUMER";                                      
                }
                else { return false; }
            }
        }

        public string PaymentAlertMessage { get; set; }
        public bool IsPaymentAlertRequired
        { 
            get
            {
                foreach (string choice in crossParams.BookRequest.room_choices)
                {
                    if (CoreBookingFields.items[choice].isPaymentAtDestination)
                    {
                        PaymentAlertMessage = CoreBookingFields.items[choice].price_destination.message;
                        return true;
                    }
                }
                return false;
            }

        }

        private CouponResponse voucherResult;
        public CouponResponse VoucherResult { get { return voucherResult; } set { voucherResult = value; OnPropertyChanged(); } }

        public Voucher Voucher { get; set; }

        public bool IsTermsAndConditionsAccepted { get; set; }        
        public event EventHandler ShowRiskReview;
        public event EventHandler HideRiskReview;

        public ItemsKey ItemSelected { get; set; }
        //public CheckoutMethodKey CheckoutMethodSelected { get; set; }

        /// <summary>
        /// Selected "RadioButton" payment strategy
        /// </summary>
        private InstallmentOption selectedInstallment;
        public InstallmentOption SelectedInstallment
        {
            get { return selectedInstallment; }
            set
            {
                if (value.FirstCard.type.ToLower().Contains("at_destination"))
                {
                    ItemSelected = CoreBookingFields.items.FirstOrDefault(x => x.Value.isPaymentAtDestination).Value;
                }
                else
                {
                    ItemSelected = CoreBookingFields.items.FirstOrDefault(x => !x.Value.isPaymentAtDestination).Value;
                }
                CoreBookingFields.CheckoutMethodSelected = CoreBookingFields.form.checkout_method.FirstOrDefault(x => x.Key == ItemSelected.checkout_method).Value;


                value.SelectedInstallment = true;
                selectedInstallment = value;
                OnPropertyChanged();

                // Select first by default
                SelectedCard = value.FirstCard;
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

        private HotelPayment selectedCard;
        public HotelPayment SelectedCard
        {
            get { return selectedCard; }
            set
            {
                selectedCard = value;

                // Set POST data
                if (selectedCard != null)
                {
                    PaymentForm payments = CoreBookingFields.form.checkout_method.FirstItem.payment;
                    if (payments != null && payments.installment.quantity == null)
                        payments.installment.quantity = new RegularField();
                    if (selectedCard.card != null)
                    {
                        payments.installment.bank_code.CoreValue = selectedCard.card.bank;
                        payments.installment.card_code.CoreValue = selectedCard.card.code;
                        payments.installment.card_code.CoreValue = selectedCard.card.company;
                        payments.installment.card_type.CoreValue = selectedCard.card.type;
                        if (payments.installment.complete_card_code == null)
                            payments.installment.complete_card_code = new RegularField();
                        payments.installment.complete_card_code.CoreValue = selectedCard.card.code;
                    }

                    if (creditCardsValidations != null && selectedCard.card != null)
                    {
                            ValidationCreditcard validation = creditCardsValidations
                                .data.FirstOrDefault(x => x.bankCode == (String.IsNullOrWhiteSpace(selectedCard.card.bank) ? "*" : selectedCard.card.bank) && x.cardCode == selectedCard.card.company);

                        Validation valNumber = new Validation();
                        valNumber.error_code = "NUMBER";
                        valNumber.regex = validation.numberRegex;
                        payments.card.number.validations = new List<Validation>();
                        payments.card.number.validations.Add(valNumber);

                        Validation valLength = new Validation();
                        valLength.error_code = "LENGTH";
                        valLength.regex = validation.lengthRegex;
                        payments.card.number.validations.Add(valLength);

                        Validation valCode = new Validation();
                        valCode.error_code = "CODE";
                        valCode.regex = validation.codeRegex;
                        payments.card.security_code.validations = new List<Validation>();
                        payments.card.security_code.validations.Add(valCode); //.number.validations.Add(val);
                    }
                }

                OnPropertyChanged();
            }
        }

        public ICommand ValidateAndBuyCommand
        {
            get
            {
                return new RelayCommand(async () => await ValidateAndBuy());
            }
        }
   
        public ICommand ValidateAndBuyNoCheckDuplicates
        {
            get
            {
                return new RelayCommand(async () => await ValidateAndBuy(false));
            }
        }


        public List<Despegar.Core.Neo.Business.Hotels.BookingCompletePostResponse.RiskQuestion> FreeTextQuestions
        {
            get
            {
                if (crossParams.BookingResponse != null)
                {
                    return crossParams.BookingResponse.risk_questions.Where(x => x.free_text.ToLower() == "true").ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Despegar.Core.Neo.Business.Hotels.BookingCompletePostResponse.RiskQuestion> ChoiceQuestions
        {
            get
            {
                if (crossParams.BookingResponse != null)
                {
                    return crossParams.BookingResponse.risk_questions.Where(x => x.free_text.ToLower() == "false").ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public ICommand SendRiskAnswersCommand
        {
            get
            {
                return new RelayCommand(() => SendRiskAnswers());
            }
        }


        #endregion        

        public HotelsCheckoutViewModel(INavigator navigator, IMAPIHotels hotelService, IMAPICross commonService, IMAPICoupons couponsService, IAPIv1 apiV1service, ICoreLogger logger, IBugTracker t)
            : base(navigator,t)
        {
            this.logger = logger;
            this.hotelService = hotelService;
            this.apiV1service = apiV1service;
            this.commonServices = commonService;
            this.couponsService = couponsService;
            this.PaymentAlertMessage = string.Empty;
        }

        public async Task Init()
        {
            BugTracker.LeaveBreadcrumb("Hotel checkout view model init");
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
                //PriceDetailsFormatted = FormatPrice();

                SelectTheFirstInstallment();


                // Set Known Default Values && Adapt Checkout to the country
                ConfigureCountry(currentCountry);

                //Get validations for credit cards
                GetCreditCardsValidations();
                BugTracker.LeaveBreadcrumb("Hotels checkout view model init complete");
            }
            catch (Exception e)
            {
                logger.Log("[App:HotelsCheckout] Exception " + e.Message);
                IsLoading = false;

                OnViewModelError("CHECKOUT_INIT_FAILED");
            }

            IsLoading = false;
        }

        private void SelectTheFirstInstallment()
        {
            if (this.InstallmentFormatted.PayAtDestination.Cards.Count() != 0)
            {
                InstallmentFormatted.PayAtDestination.IsChecked = true;
                SelectedInstallment = InstallmentFormatted.PayAtDestination;
            }
            else if (this.InstallmentFormatted.WithoutInterest != null)
            {
                InstallmentFormatted.WithoutInterest[0].IsChecked = true;
                SelectedInstallment = InstallmentFormatted.WithoutInterest[0];
            }
            else
            {
                InstallmentFormatted.WithInterest[0].IsChecked = true;
                SelectedInstallment = InstallmentFormatted.WithoutInterest[0];
            }
        }

        private void ConfigureCountry(string currentCountry)
        {
            BugTracker.LeaveBreadcrumb("Flight checkout view model configure country");

            // Common

            // Contact
            if (CoreBookingFields.form.contact.Phone != null)
                CoreBookingFields.form.contact.Phone.type.SetDefaultValue();

            // Card data
            //if (CoreBookingFields.form.payment.card.owner_document != null && CoreBookingFields.form.payment.card.owner_document.type != null)
            //    CoreBookingFields.form.payment.card.owner_document.type.SetDefaultValue();
            //if (CoreBookingFields.form.payment.card.owner_gender != null)
            //    CoreBookingFields.form.payment.card.owner_gender.SetDefaultValue();       


            switch (currentCountry)
            {
                case "AR":

                    // Invoice Arg
                    //var checkout = null;
                    var checkout = CoreBookingFields.form.checkout_method.FirstOrDefault(x => x.Value.payment.invoice != null);
                    if (checkout.Value != null)
                    {
                        //CoreBookingFields.items.FirstOrDefault(x => x.Value.payment.

                        checkout.Value.payment.invoice.fiscal_status.PropertyChanged += Fiscal_status_PropertyChanged;

                        checkout.Value.payment.invoice.fiscal_status.SetDefaultValue();
                        if (checkout.Value.payment.invoice.address.country != null)
                            checkout.Value.payment.invoice.address.country.SetDefaultValue();

                        // Turn State into a MultipleField
                        if (checkout.Value.payment.invoice.address.state != null)
                        {
                            checkout.Value.payment.invoice.address.state.value = null;
                            checkout.Value.payment.invoice.address.state.options = States.Select(x => new Option() { value = x.id, description = x.name }).ToList();
                            checkout.Value.payment.invoice.address.state.SetDefaultValue();
                        }
                    }

                    CoreBookingFields.form.contact.phones[0].country_code.SetDefaultValue();
                    CoreBookingFields.form.contact.phones[0].area_code.SetDefaultValue();
                    break;
            }
            BugTracker.LeaveBreadcrumb("Flight checkout view model configure country complete");
        }

        // Public because it is used from the InvoiceArg control
        public async Task<List<CitiesFields>> GetCities(string countryCode, string search, string cityresult)
        {
            //todo
            return await commonServices.AutoCompleteCities(countryCode, search, cityresult);
        }

        /// <summary>
        /// Validates the reference code against the service and sets the Validation errors or succcess
        /// </summary>
        public async void ValidateVoucher()
        {
            BugTracker.LeaveBreadcrumb("Hotels view model validate voucher init");

            IsLoading = true;
            ResourceLoader loader = new ResourceLoader();
            Voucher field = CoreBookingFields.form.Voucher;

            field.IsApplied = false;

            var pricing = CoreBookingFields.items.FirstOrDefault().Value.price;  // TODO: Find out better about the items in th

            CouponParameter parameter = new CouponParameter()
            {
                Beneficiary = CoreBookingFields.form.contact.email != null ? CoreBookingFields.form.contact.email.CoreValue : "",
                TotalAmount = pricing.total.ToString(),
                CurrencyCode = pricing.currency.code,
                Product = "hotel",
                Quotation = String.Format(CultureInfo.InvariantCulture, "{0:0.#################}", pricing.currency.ratio),
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

            field.Validate();
            IsLoading = false;

            BugTracker.LeaveBreadcrumb("Hotels checkout view model validate voucher complete");
        }

        /// <summary>
        /// Format Credit Cards installments
        /// </summary>
        /// <returns></returns>
        private void FormatInstallments()
        {
            //var payments = CoreBookingFields.items.First().Value.payment.with_interest.
            InstallmentFormatted = new InstallmentFormatted();

            // TODO: More Items???
            //var item = CoreBookingFields.items.First().Value.payment;
            foreach (var element in CoreBookingFields.items)
            {
                var item = element.Value.payment;
                // Pay at destination
                if (item.at_destination != null)
                {
                    foreach (HotelPayment payment in item.at_destination)
                        InstallmentFormatted.AddPayAtDestinationInstallment(payment);
                    //Pay_at_Destination_checkout_method = element.Value.checkout_method;
                }
                //else
                //{
                //    Pay_With_Card_checkout_method = element.Value.checkout_method;
                //}

                // Without interest
                if (item.without_interest != null)
                {
                    foreach (HotelPayment payment in item.without_interest)
                        InstallmentFormatted.AddWithouInterestInstallment(payment);
                }

                // With Interest
                if (item.with_interest != null)
                {
                    foreach (HotelPayment payment in item.with_interest)
                        InstallmentFormatted.AddWithInterestInstallment(payment);
                }

                if (InstallmentFormatted.WithInterest.Count != 0)
                {
                    var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                    InstallmentFormatted.ResourceLabel = loader.GetString("Common_Pay_Of");
                }
            }
        }

        private async Task ValidateAndBuy(bool checkDuplicated = true)
        { 
            BugTracker.LeaveBreadcrumb("Hotels checkout view model validate and buy init");

            if (!IsTermsAndConditionsAccepted)
            {
                OnViewModelError("TERMS_AND_CONDITIONS_NOT_CHECKED");
                BugTracker.LeaveBreadcrumb("Hotel checkout buy terms not accepted");
                return;
            }

            string sectionID = "";

            // Validation
            if (!CoreBookingFields.IsValid(out sectionID))
            {
                BugTracker.LeaveBreadcrumb("Hotel checkout ViewModel invalid fields");
                OnViewModelError("FORM_ERROR", sectionID); // TODO: Catch
            }
            else
            {
                try
                {
                    this.IsLoading = true;
                    object bookingData = null;

                    bookingData = await BookingFormBuilder.BuildHotelsForm(this.CoreBookingFields, this.CoreBookingFields.CheckoutMethodSelected.payment != null ? this.CoreBookingFields.CheckoutMethodSelected.payment.invoice : null, SelectedCard, checkDuplicated);

                    //// Buy
                    //crossParams.PriceDetail = PriceDetailsFormatted;
                    
                    crossParams.BookingResponse = await hotelService.CompleteBooking(bookingData, CoreBookingFields.id , ItemSelected.item_id );

                    if (crossParams.BookingResponse.Error != null)
                    {
                        BugTracker.LeaveBreadcrumb("Hotels checkout MAPI booking error response code: " + crossParams.BookingResponse.Error.code.ToString());

                        switch (crossParams.BookingResponse.Error.code)
                        {
                            case 2366:
                                OnViewModelError("DUPLICATED_BOOKING", crossParams.BookingResponse.Error);
                                break;
                            default:
                                // API Error ocurred, Check CODE and inform the user
                                OnViewModelError("API_ERROR", crossParams.BookingResponse.Error.code);
                                break;
                        }

                        this.IsLoading = false;
                        return;
                    }

                    //// Booking processed, check the status of Booking request
                    AnalizeBookingStatus(crossParams.BookingResponse.booking_status);
                }
                catch (HTTPStatusErrorException e)
                {
                    OnViewModelError("COMPLETE_BOOKING_CONECTION_FAILED");
                }
                catch (Exception e)
                {
                    OnViewModelError("COMPLETE_BOOKING_BOOKING_FAILED");
                }

                BugTracker.LeaveBreadcrumb("Hotels checkout view model validate and buy complete");
                this.IsLoading = false;
            }
        }

        public void FillBookingFields()
        {
            CoreBookingFields.form.passengers[0].first_name.CoreValue = "test";
            CoreBookingFields.form.passengers[0].last_name.CoreValue = "booking";
            CoreBookingFields.form.contact.email.CoreValue = "testhoteles@despegar.com";
            CoreBookingFields.form.contact.emailConfirmation.CoreValue = "testhoteles@despegar.com";
            CoreBookingFields.form.contact.Phone.area_code.CoreValue = "54";
            CoreBookingFields.form.contact.Phone.country_code.CoreValue = "11";
            CoreBookingFields.form.contact.Phone.number.CoreValue = "12341234";
            if (CoreBookingFields.form.CardInfo != null)
            {
                if (CoreBookingFields.form.CardInfo.expiration != null)
                    CoreBookingFields.form.CardInfo.expiration.CoreValue = "2018-3";
                if (CoreBookingFields.form.CardInfo.number != null)
                    CoreBookingFields.form.CardInfo.number.CoreValue = "4242424242424242";
                if (CoreBookingFields.form.CardInfo.owner_document != null)
                {
                    if (CoreBookingFields.form.CardInfo.owner_document.number != null)
                        CoreBookingFields.form.CardInfo.owner_document.number.CoreValue = "12123123";
                    if (CoreBookingFields.form.CardInfo.owner_document.type != null)
                        CoreBookingFields.form.CardInfo.owner_document.type.CoreValue = "LOCAL";
                }
                if (CoreBookingFields.form.CardInfo.owner_gender != null)
                    CoreBookingFields.form.CardInfo.owner_gender.CoreValue = "MALE";
                if (CoreBookingFields.form.CardInfo.owner_name != null)
                    CoreBookingFields.form.CardInfo.owner_name.CoreValue = "test booking";
                if (CoreBookingFields.form.CardInfo.security_code != null)
                    CoreBookingFields.form.CardInfo.security_code.CoreValue = "123";
            }

            if (CoreBookingFields.CheckoutMethodSelected.payment != null && CoreBookingFields.CheckoutMethodSelected.payment.invoice != null)
            {
                this.CoreBookingFields.CheckoutMethodSelected.payment.invoice.address.number.CoreValue = "123";
                this.CoreBookingFields.CheckoutMethodSelected.payment.invoice.address.postal_code.CoreValue = "1234";
                this.CoreBookingFields.CheckoutMethodSelected.payment.invoice.address.street.CoreValue = "falsa";
                this.CoreBookingFields.CheckoutMethodSelected.payment.invoice.fiscal_id.CoreValue = "23121231239";
                this.CoreBookingFields.CheckoutMethodSelected.payment.invoice.fiscal_status.CoreValue = "FINAL_CONSUMER";
            }

            //OnPropertyChanged();

        }

        /// <summary>
        /// Validates the booking status
        /// </summary>
        
        private void AnalizeBookingStatus(string status)
        {
            BugTracker.LeaveBreadcrumb("Flight checkout view model booking status" + status);

            switch (GetStatus(status))
            {
                case HotelBookingStatusEnum.SUCCESS:

                    Navigator.GoTo(ViewModelPages.HotelsThanks, crossParams);
                    break;

                case HotelBookingStatusEnum.BOOKING_ERROR:

                    OnViewModelError("BOOKING_FAILED", crossParams.BookingResponse.checkout_id);
                    break;

                case HotelBookingStatusEnum.FIX_CREDIT_CARD:
                    this.CoreBookingFields.form.booking_status = "FIX_CREDIT_CARD";
                    FreezeFields();
                    OnViewModelError("ONLINE_PAYMENT_ERROR_FIX_CREDIT_CARD", "CARD");
                    break;

                case HotelBookingStatusEnum.NEW_CREDIT_CARD:

                    this.CoreBookingFields.CheckoutMethodSelected.payment.card.number.CoreValue = String.Empty;
                    this.CoreBookingFields.CheckoutMethodSelected.payment.card.expiration.CoreValue = String.Empty;
                    this.CoreBookingFields.CheckoutMethodSelected.payment.card.security_code.CoreValue = String.Empty;
                    this.CoreBookingFields.form.booking_status = "NEW_CREDIT_CARD";

                    FreezeFields();
                    OnPropertyChanged("SelectedCard");
                    OnViewModelError("ONLINE_PAYMENT_ERROR_NEW_CREDIT_CARD", "CARD");
                    break;

                case HotelBookingStatusEnum.risk_evaluation_failed:
                case HotelBookingStatusEnum.RISK_REJECTED:
                    OnViewModelError("RISK_PAYMENT_FAILED", "CARD");
                    break;

                case HotelBookingStatusEnum.risk_review:
                case HotelBookingStatusEnum.RISK_QUESTIONS:
                    EventHandler RiskHandler = ShowRiskReview;
                    if (RiskHandler != null)
                        RiskHandler(this, null);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Freezes Contact and Passenger Fields
        /// </summary>
        private void FreezeFields()
        {
            // Contact
            CoreBookingFields.form.contact.IsFrozen = false;
            // Passengers
            foreach (var item in CoreBookingFields.form.passengers)
                item.IsFrozen = false;
        }

        private HotelBookingStatusEnum GetStatus(string status)
        {
            try
            {
                HotelBookingStatusEnum _status = (HotelBookingStatusEnum)Enum.Parse(typeof(HotelBookingStatusEnum), status);

                return _status;
            }
            catch (Exception)
            {
                return HotelBookingStatusEnum.BookingCustomError;
            }
        }

        private async void GetCreditCardsValidations()
        {
            try
            {
                BugTracker.LeaveBreadcrumb("Hotel checkout view model get credit cards validations");
                creditCardsValidations = await apiV1service.GetCreditCardValidations();
            }
            catch (Exception e)
            {
                logger.Log("[App:HotelsCheckout] Exception " + e.Message);
                IsLoading = false;
                OnViewModelError("CHECKOUT_INIT_FAILED");
            }
        }

        private void Fiscal_status_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CoreValue")
            {
                OnPropertyChanged("IsFiscalNameRequired");
            }
        }

        private async Task GetBookingFields(string deviceID)
        {
            BugTracker.LeaveBreadcrumb("Hotels Checkout ViewModel get booking fields init");
            CoreBookingFields = await hotelService.GetBookingFields(crossParams.BookRequest);
            CoreBookingFields.form.CountrySite = GlobalConfiguration.Site;
            BugTracker.LeaveBreadcrumb("Hotels Checkout ViewModel get booking fields complete");
        }

        private async Task LoadCountries()
        {
            Countries = (await commonServices.GetCountries()).countries;
        }

        private async Task LoadStates(string countryCode)
        {
            States = await commonServices.GetStates(countryCode);
        }

        public override void OnNavigated(object navigationParams)
        {
            BugTracker.LeaveBreadcrumb("Hotel checkout start");
            crossParams = navigationParams as HotelsCrossParameters;
        }


        private async void SendRiskAnswers()
        {
            BugTracker.LeaveBreadcrumb("Flight checkout view model Risk init");
            ResourceLoader manager = new ResourceLoader();

            if (ValidateAnswers())
            {
                this.IsLoading = true;
                List<Despegar.Core.Neo.Business.Hotels.BookingCompletePostResponse.RiskAnswer> answers = new List<Despegar.Core.Neo.Business.Hotels.BookingCompletePostResponse.RiskAnswer>();
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

                BookingResponse = await hotelService.CompleteBooking(result, CoreBookingFields.id,ItemSelected.item_id);
                this.IsLoading = false;

                EventHandler AnswerHandler = HideRiskReview;
                if (AnswerHandler != null)
                {
                    AnswerHandler(this, null);
                }

                if (BookingResponse.Error != null)
                {
                    BugTracker.LeaveBreadcrumb("Hotels checkout MAPI booking error response code: " + BookingResponse.Error.code.ToString());

                    switch (BookingResponse.Error.code)
                    {
                        case 2366:
                            OnViewModelError("DUPLICATED_BOOKING", BookingResponse.Error);
                            break;
                        default:
                            // API Error ocurred, Check CODE and inform the user
                            OnViewModelError("API_ERROR", BookingResponse.Error.code);
                            break;
                    }

                    this.IsLoading = false;
                    return;
                }

                AnalizeBookingStatus(BookingResponse.booking_status);

            }
            else
            {
                var msg = new MessageDialog(manager.GetString("Flight_Checkout_Risk_Error"));
                await msg.ShowAsync();
            }
            BugTracker.LeaveBreadcrumb("Hotel checkout view model Risk complete");

        }
        private bool ValidateAnswers()
        {
            foreach (RiskQuestion question in crossParams.BookingResponse.risk_questions)
            {
                if (question.risk_answer.text == null || question.risk_answer.text == "")
                {
                    return false;
                }
            }

            return true;
        }
    }
}