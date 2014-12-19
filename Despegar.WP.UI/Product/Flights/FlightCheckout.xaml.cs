using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Dynamics;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Common;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using Despegar.WP.UI.Product.Flights.Checkout.Passegers.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.ApplicationModel.Resources;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;

namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightCheckout : Page
    {
        private NavigationHelper navigationHelper;
        private FlightsCheckoutViewModel ViewModel;
        private ModalPopup loadingPopup = new ModalPopup(new Loading());

        public FlightCheckout()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            FlightsCrossParameter crossParameters = e.NavigationParameter as FlightsCrossParameter;

            // Initialize Checkout
            ViewModel = new FlightsCheckoutViewModel(
                Navigator.Instance, 
                GlobalConfiguration.CoreContext.GetFlightService(),
                GlobalConfiguration.CoreContext.GetCommonService(), 
                GlobalConfiguration.CoreContext.GetConfigurationService(), 
                GlobalConfiguration.CoreContext.GetCouponsService(),
                crossParameters);

            ViewModel.PropertyChanged += Checkloading;
            ViewModel.ShowRiskReview += this.ShowRisk;            

            ViewModel.ViewModelError += ErrorHandler;

            // Init Checkout
            await ViewModel.Init();

            // Set Defaults values and Country specifics
            ConfigureFields();

            this.DataContext = ViewModel;
        }

        private void ShowRisk(Object sender, EventArgs e )
        {
        }

        # region ** ERROR HANDLING **
        private void ErrorHandler(object sender, ViewModelErrorArgs e)
        {
            ResourceLoader manager = new ResourceLoader();
            MessageDialog dialog;
            string pageID;

            switch (e.ErrorCode)
            {
                case "FORM_ERROR":
                    dialog = new MessageDialog(manager.GetString("Flights_Checkout_ERROR_FORM_ERROR"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                    dialog.ShowAsync();

                    // Go to Pivot with errors
                    string sectionID = (string)e.Parameter;
                    MainPivot.SelectedIndex = GetSectionIndex(sectionID);
                    break;
                case "TERMS_AND_CONDITIONS_NOT_CHECKED":
                    dialog = new MessageDialog(manager.GetString("TermsAndConditions_ERROR"), manager.GetString("TermsAndConditions_ERROR_TITLE"));
                    dialog.ShowAsync();
                    break;

                case "BOOKING_FAILED":
                    {
                        string ticketid = e.Parameter as string;
                        ticketid = (ticketid!=null)?ticketid:String.Empty;
                        string phrase = manager.GetString("Flights_Checkout_Card_Data_Card_ERROR_OP_BOOKING_FAILED");

                        dialog = new MessageDialog(String.Format(phrase,ticketid), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                        dialog.ShowAsync();
                        this.navigationHelper.GoBack();
                        this.navigationHelper.GoBack();
                        break;
                    }

               case "COMPLETE_BOOKING_CONECTION_FAILED":
                    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_FAILED"), manager.GetString("Flights_Search_ERROR_SEARCH_FAILED_TITLE"));
                    dialog.ShowAsync();
                    break;

               case "CHECKOUT_INIT_FAILED":
                    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_FAILED"), manager.GetString("Flights_Search_ERROR_SEARCH_FAILED_TITLE"));
                    dialog.ShowAsync();
                    this.navigationHelper.GoBack();
                    break;
               case "ONLINE_PAYMENT_ERROR_NEW_CREDIT_CARD":
                    dialog = new MessageDialog(manager.GetString("Flights_Checkout_Card_Data_Card_ERROR_NEW_CREDIT_CARD"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                    dialog.ShowAsync();

                    // Go to Pivot with errors
                    pageID = (string)e.Parameter;
                    MainPivot.SelectedIndex = GetSectionIndex(pageID);
                    break;

               case "ONLINE_PAYMENT_ERROR_FIX_CREDIT_CARD":
                    {
                        dialog = new MessageDialog(manager.GetString("Flights_Checkout_Card_Data_Card_ERROR_ONLINE_PAYMENT_ERROR_FIX_CREDIT_CARD"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                        dialog.ShowAsync();
                        pageID = (string)e.Parameter;
                        MainPivot.SelectedIndex = GetSectionIndex(pageID);
                    }
                    break;

               case "ONLINE_PAYMENT_FAILED":
                    {
                        //string ticketid = e.Parameter as string;
                        //ticketid = (ticketid != null) ? ticketid : String.Empty;

                        string phone = GetContactPhone();
                        string phrase = manager.GetString("Flights_Checkout_Card_Data_Card_ERROR_OP_PAYMENT_FAILED");
                        dialog = new MessageDialog(String.Format(phrase, phone), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                        dialog.ShowAsync();
                        break;
                    }

                case "COMPLETE_BOOKING_BOOKING_FAILED":
                    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_BOOKING_FAILED"), manager.GetString("Flights_Search_ERROR_SEARCH_FAILED_TITLE"));
                    dialog.ShowAsync();
                    this.navigationHelper.GoBack();
                    this.navigationHelper.GoBack();
                    break;
                case "API_ERROR":
                    switch(((int)e.Parameter)) 
                    {
                            // TODO: check this cases:

                            //EXPIRED_SESSION("10", "^.*booking_id not found.*$"),
                            //BENEFICIARY_TYPE_INVALID_ERROR("11", "^.*Invalid value for CouponBeneficiaryIdType.*$"),
                            //INVALID_BIRTHDAY("13", ".*birthday.value:  INVALID_VALUE.*"),
                            //MISSING_BIRTHDAY("14", ".*birthday.value:  MISSING_FIELD.*"),

                            //// this field is called card_holder_name in V3 and fiscal_name in MAPI
                            //MISSING_FISCAL_NAME("15", ".*invoice.card_holder_name.value:  MISSING_FIELD.*"), 

                            //COUPON_INVALID("20", buildStandardErrorMessage("100")),
                            //COUPON_EXPIRED("22", buildStandardErrorMessage("202")),
                            //COUPON_NO_USES_REMAINING("23", buildStandardErrorMessage("203")),
                            //COUPON_WRONG_COUNTRY("24", buildStandardErrorMessage("204")),
                            //COUPON_INVALID_BENEFICIARY("26", buildStandardErrorMessage("206")),
                            //COUPON_ALREADY_USED_BY_USER("27", buildStandardErrorMessage("207")),
                            //COUPON_INVALID_DATE("43", buildStandardErrorMessage("423")),
                            //BOOKING_ITEM_NOT_FOUND("60", ".*Item .* was not found.*"); // TODO reproduce this error, got it from new relic  

                        default:
                            break;
                    }
                    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_BOOKING_FAILED"), manager.GetString("Flights_Search_ERROR_SEARCH_FAILED_TITLE"));
                    dialog.ShowAsync();                    
                    break;
            }
        }

        private string GetContactPhone()
        {
            try
            {

                Configuration conf = GlobalConfiguration.CoreContext.GetConfiguration();

                if (conf == null) return String.Empty;
                string countrySelected = GlobalConfiguration.Site;
                string phone = (conf.sites.FirstOrDefault(si => si.code == countrySelected) as Site).contact.phone;

                return phone;
            }catch(Exception ex)
            {
                //TODO add logs
                return String.Empty;
            }
        }

        private int GetSectionIndex(string sectionID)
        {
            PivotItem errorPivot = this.FindName("Pivot_" + sectionID) as PivotItem;
            return MainPivot.Items.IndexOf(errorPivot);
        }
        #endregion

        /// <summary>
        /// View Adaptations based on selected country
        /// </summary>
        private void ConfigureFields()
        {
            if (!ViewModel.InvoiceRequired)
            {
                MainPivot.Items.RemoveAt(4);
            }

            //switch(GlobalConfiguration.Site) 
            //{
            //    case "AR":
            //        if (!ViewModel.InvoiceRequired)
            //        {
            //            MainPivot.Items.RemoveAt(4);
            //        }

            //        // Passengers defaults                    
            //        //foreach (NationalitySelection control in this.FindVisualChildren<NationalitySelection>(PassengerControl))
            //        //    control.SetDisplayText("Argentina");
            //    break;

            //    default:               
            //    break;
            //}
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private void Checkloading(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoading")
            {
                if ((sender as ViewModelBase).IsLoading)
                    loadingPopup.Show();
                else
                    loadingPopup.Hide();
            }
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (ViewModel != null)
            {
                if (ViewModel.IsLoading )
                {
                    e.Handled = true;
                }
                if (ViewModel.NationalityIsOpen)
                {
                    e.Handled = true;
                    ViewModel.NationalityIsOpen = false;
                }
            }
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
     
    }
}