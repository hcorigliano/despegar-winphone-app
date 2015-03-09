﻿using Despegar.Core.Neo.Business.Common.Checkout;
using Despegar.Core.Neo.Business.Common.State;
using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Business.Coupons;
using Despegar.Core.Neo.Business.CreditCard;
using Despegar.Core.Neo.Business.Hotels;
using Despegar.Core.Neo.Business.Hotels.BookingFields;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
        private HotelsCrossParameters crossParams;
        #endregion

        #region ** Public Interface **
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
                //ConfigureCountry(currentCountry);

                //Get validations for credit cards
                GetCreditCardsValidations();
                BugTracker.LeaveBreadcrumb("Flight checkout view model init complete");
            }
            catch (Exception e)
            {
                logger.Log("[App:HotelsCheckout] Exception " + e.Message);
                IsLoading = false;

                OnViewModelError("CHECKOUT_INIT_FAILED");
            }

            IsLoading = false;
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

            BugTracker.LeaveBreadcrumb("Flight checkout view model validate voucher complete");
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
            var item = CoreBookingFields.items.First().Value.payment;

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

        private async void GetCreditCardsValidations()
        {
            try
            {
                BugTracker.LeaveBreadcrumb("Hotel checkout view model get credit cards validations");
                creditCardsValidations = await apiV1service.GetCreditCardValidations();
            }
            catch (Exception e)
            {
                logger.Log("[App:FlightsCheckout] Exception " + e.Message);
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

            HotelsBookingFieldsRequest bookRequest = new HotelsBookingFieldsRequest();

            if (crossParams.BookRequest != null)
            {
                CoreBookingFields = await hotelService.GetBookingFields(crossParams.BookRequest);
            }
            else
            {
                // TODO
                bookRequest.token = "c66602c8-09b5-4c11-92f7-9713cc4e1552";
                bookRequest.hotel_id = "298331";
                bookRequest.room_choices = new List<string>() { "3" };
                bookRequest.mobile_identifier = deviceID;

                CoreBookingFields = await hotelService.GetBookingFields(bookRequest);
            }

            BugTracker.LeaveBreadcrumb("Hotels CheckoutVviewModel get booking fields complete");
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
    }
}