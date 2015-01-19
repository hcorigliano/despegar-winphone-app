using Despegar.Core.Business.Common.Checkout;
using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Coupons;
using Despegar.Core.Business.CreditCard;
using Despegar.Core.Business.Hotels.BookingFields;
using Despegar.Core.IService;
using Despegar.Core.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsCheckoutViewModel : ViewModelBase
    {
        #region ** Private **
        private INavigator navigator;
        public IHotelService hotelService;
        private IConfigurationService configurationService;
        private ICommonServices commonServices;
        private ICouponsService couponsService;
        private IBugTracker bugTracker;
        private ValidationCreditcards creditCardsValidations;
        private HotelsCrossParameters crossParams;
        #endregion

        #region ** Public Interface **
        public HotelsBookingFields CoreBookingFields { get; set; }
        public List<CountryFields> Countries { get; set; }
        public List<Despegar.Core.Business.Common.State.State> States { get; set; }
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

        private CouponResponse voucherResult;
        public CouponResponse VoucherResult { get { return voucherResult; } set { voucherResult = value; OnPropertyChanged(); } }

        public Voucher Voucher { get; set; }
        public INavigator Navigator { get { return navigator; } }

        public bool IsTermsAndConditionsAccepted { get; set; }        
        public event EventHandler ShowRiskReview;
        public event EventHandler HideRiskReview;
        #endregion        

        public HotelsCheckoutViewModel(INavigator navigator, IHotelService hotelService, ICommonServices commonService, IConfigurationService configurationService, ICouponsService couponsService, IBugTracker t, HotelsCrossParameters crossParams)
            : base(t)
        {
            this.navigator = navigator;
            this.hotelService = hotelService;
            this.configurationService = configurationService;
            this.commonServices = commonService;
            this.couponsService = couponsService;
            this.crossParams = crossParams;
        }

        public async Task Init()
        {
            this.Tracker.LeaveBreadcrumb("Hotel checkout view model init");
            IsLoading = true;

            string currentCountry = GlobalConfiguration.Site;
            string deviceId = GlobalConfiguration.UPAId;

            try
            {
                await GetBookingFields(deviceId);
                await LoadCountries();
                await LoadStates(currentCountry);

                // Format Price details / Installments
                //FormatInstallments();
                //PriceDetailsFormatted = FormatPrice();

                // Set Known Default Values && Adapt Checkout to the country
                //ConfigureCountry(currentCountry);

                //Get validations for credit cards
                GetCreditCardsValidations();
                this.Tracker.LeaveBreadcrumb("Flight checkout view model init complete");
            }
            catch (Exception e)
            {
                Logger.Log("[App:HotelsCheckout] Exception " + e.Message);
                IsLoading = false;
                OnViewModelError("CHECKOUT_INIT_FAILED");
            }

            IsLoading = false;
        }

        private async void GetCreditCardsValidations()
        {
            try
            {
                this.Tracker.LeaveBreadcrumb("Hotel checkout view model get credit cards validations");
                creditCardsValidations = await commonServices.GetCreditCardValidations();
            }
            catch (Exception e)
            {
                Logger.Log("[App:FlightsCheckout] Exception " + e.Message);
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
            this.Tracker.LeaveBreadcrumb("Hotels checkout view model get booking fields init");

            HotelsBookingFieldsRequest bookRequest = new HotelsBookingFieldsRequest();

            // TODO
            bookRequest.token = "2a92d1c7-8a39-4574-8b9a-a92def21d36e";
            bookRequest.hotel_id = "536627";
            bookRequest.room_choices = new List<string>() { "22" };
            bookRequest.mobile_identifier = deviceID;

            CoreBookingFields = await hotelService.GetBookingFields(bookRequest);

            this.Tracker.LeaveBreadcrumb("Hotels checkout view model get booking fields complete");
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
        public async Task<List<CitiesFields>> GetCities(string countryCode, string search, string cityresult)
        {
            return await configurationService.AutoCompleteCities(countryCode, search, cityresult);
        }

    }
}