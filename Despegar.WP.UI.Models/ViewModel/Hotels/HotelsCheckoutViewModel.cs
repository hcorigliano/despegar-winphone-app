using Despegar.Core.Neo.Business.Common.Checkout;
using Despegar.Core.Neo.Business.Common.State;
using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Business.Coupons;
using Despegar.Core.Neo.Business.CreditCard;
using Despegar.Core.Neo.Business.Forms;
using Despegar.Core.Neo.Business.Hotels;
using Despegar.Core.Neo.Business.Hotels.BookingFields;
using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.Core.Neo.Exceptions;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;

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
        #endregion

        #region ** Public Interface **
        //public RoomsSelected
        public HotelsCrossParameters CrossParams { get; set; }
        public HotelsBookingFields CoreBookingFields { get; set; }
        public List<CountryFields> Countries { get; set; }
        public List<State> States { get; set; }
        public bool InvoiceRequired
        {
            get
            {
                if (GlobalConfiguration.Site == "AR")
                    return CoreBookingFields != null ? CoreBookingFields.form.Invoice != null 
                        && !CoreBookingFields.form.Invoice.AllFieldsAreOptional : false;

                return false;
            }
        }

        public bool IsFiscalNameRequired
        {
            get
            {
                if (InvoiceRequired)
                {
                    return CoreBookingFields.form.Invoice.fiscal_status.required && CoreBookingFields.form.Invoice.fiscal_status.CoreValue != "FINAL";                                      
                }
                else { return false; }
            }
        }

        public ItemPrice PaymentDetails {
            get
            {
                return CoreBookingFields.items.FirstOrDefault().Value.price;
            }
        }

        public HotelItem HotelDetails
        {
            get
            {
                return CrossParams.SelectedHotel;
            }
        }

        public string CancelationPolicy
        {
            get
            {
                return CrossParams.RoomPackSelected.room_availabilities[0].cancellation_policy.penalty_short_description;
            }
        }

        public string MealPlan
        {
            get
            {
                return CrossParams.RoomPackSelected.room_availabilities[0].meal_plan.description;
            }
        }

        public string RoomName 
        {
            get 
            {
                return CrossParams.RoomPackSelected.name;
            }
        }

        public string PaymentAlertMessage { get; set; }
        public bool IsPaymentAlertRequired
        { 
            get
            {
                foreach (string choice in CrossParams.BookRequest.room_choices)
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

        private void ConfigureCountry(string currentCountry)
        {            
        }

        /// <summary>
        /// Selected "RadioButton" payment strategy
        /// </summary>
        private InstallmentOption selectedInstallment;
        public InstallmentOption SelectedInstallment
        {
            get { return selectedInstallment; }
            set
            {
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
                    payments.installment.bank_code.CoreValue = selectedCard.card.bank;
                    //payments.installment.quantity.CoreValue = selectedCard.installments.quantity.ToString();
                    payments.installment.card_code.CoreValue = selectedCard.card.code;
                    payments.installment.card_code.CoreValue = selectedCard.card.company;
                    payments.installment.card_type.CoreValue = selectedCard.card.type;
                    //payments.installment.complete_card_code.CoreValue = selectedCard.card.code;

                    if (creditCardsValidations != null)
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
                }

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

        private async Task ValidateAndBuy()
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
                    //this.IsLoading = true;
                    //object bookingData = null;

                    //bookingData = await BookingFormBuilder.BuildHotelsForm(this.CoreBookingFields);

                    //// Buy
                    //crossParams.PriceDetail = PriceDetailsFormatted;
                    //crossParams.BookingResponse = await hotelService.CompleteBooking(bookingData, CoreBookingFields.id);

                    //if (crossParams.BookingResponse.Error != null)
                    //{
                    //    BugTracker.LeaveBreadcrumb("Hotels checkout MAPI booking error response code: " + crossParams.BookingResponse.Error.code.ToString());
                    //    // API Error ocurred, Check CODE and inform the user
                    //    OnViewModelError("API_ERROR", crossParams.BookingResponse.Error.code);
                    //    this.IsLoading = false;
                    //    return;
                    //}

                    //// Booking processed, check the status of Booking request
                    //AnalizeBookingStatus(crossParams.BookingResponse.booking_status);
                }
                catch (HTTPStatusErrorException)
                {
                    OnViewModelError("COMPLETE_BOOKING_CONECTION_FAILED");
                }
                catch (Exception)
                {
                    OnViewModelError("COMPLETE_BOOKING_BOOKING_FAILED");
                }

                BugTracker.LeaveBreadcrumb("Hotels checkout view model validate and buy complete");
                this.IsLoading = false;
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
            CoreBookingFields = await hotelService.GetBookingFields(CrossParams.BookRequest);
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
            CrossParams = navigationParams as HotelsCrossParameters;
        }
    }
}