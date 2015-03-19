using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Common;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Flights;
using Despegar.WP.UI.Product.Flights.Checkout;
using System;
using System.ComponentModel;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightCheckout : Page
    {
        private FlightsCheckoutViewModel ViewModel;
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
        private ModalPopup riskPopup; 

        public FlightCheckout()
        {
            this.InitializeComponent();
            #if !DEBUG
                GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
                ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
                ga.SendView("FlightCheckout");
            #endif
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;

                ViewModel = IoC.Resolve<FlightsCheckoutViewModel>();
                ViewModel.OnNavigated(e.Parameter); // Init Checkout 
                ViewModel.PropertyChanged += Checkloading;
                ViewModel.ShowRiskReview += this.ShowRisk;
                ViewModel.HideRiskReview += this.HideRisk;
                ViewModel.ViewModelError += ErrorHandler;
                await ViewModel.Init();
                this.DataContext = ViewModel;

                // Set Defaults values and Country specifics
                ConfigureFields();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                ResetPageCache();
            }
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

        }

        private void ResetPageCache()
        {
            var cacheSize = ((Frame)Parent).CacheSize;
            ((Frame)Parent).CacheSize = 0;
            ((Frame)Parent).CacheSize = cacheSize;
        }

        private void ShowRisk(Object sender, EventArgs e )
        {
            riskPopup = new ModalPopup(new RiskQuestionsPopUp() { DataContext = ViewModel });
            riskPopup.Show();
        }

        private void HideRisk(Object sender, EventArgs e)
        {
            riskPopup.Hide();
        }

        # region ** ERROR HANDLING **
        private async void ErrorHandler(object sender, ViewModelErrorArgs e)
        {
            ViewModel.BugTracker.LeaveBreadcrumb("Flight checkout Error raised - " + e.ErrorCode);

            ResourceLoader manager = new ResourceLoader();
            MessageDialog dialog;
            string pageID;

            switch (e.ErrorCode)
            {
                case "FORM_ERROR":
                    dialog = new MessageDialog(manager.GetString("Flights_Checkout_ERROR_FORM_ERROR"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    string sectionID = (string)e.Parameter;
                    MainPivot.SelectedIndex = GetSectionIndex(sectionID);
                    break;
                case "TERMS_AND_CONDITIONS_NOT_CHECKED":
                    dialog = new MessageDialog(manager.GetString("TermsAndConditions_ERROR"), manager.GetString("TermsAndConditions_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    break;
                case "BOOKING_FAILED":
                    string ticketid = e.Parameter as string;
                    ticketid = (ticketid!=null)?ticketid:String.Empty;
                    string phrase = manager.GetString("Flights_Checkout_Card_Data_Card_ERROR_OP_BOOKING_FAILED");
                    dialog = new MessageDialog(String.Format(phrase,ticketid), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    ViewModel.Navigator.RemoveBackEntry();
                    ViewModel.Navigator.GoBack();
                    break;   
               case "COMPLETE_BOOKING_CONECTION_FAILED":
                    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_FAILED"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    break;
               case "CHECKOUT_INIT_FAILED":
                    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_FAILED"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    ViewModel.Navigator.GoBack();
                    break;
               case "ONLINE_PAYMENT_ERROR_NEW_CREDIT_CARD":
                    // NEW CC
                    dialog = new MessageDialog(manager.GetString("new_creditcard"), manager.GetString("new_creditcard_title"));
                    await dialog.ShowSafelyAsync();
                    pageID = (string)e.Parameter;
                    MainPivot.SelectedIndex = GetSectionIndex(pageID);
                    break;
               case "ONLINE_PAYMENT_ERROR_FIX_CREDIT_CARD":
                    // FIX CC
                    dialog = new MessageDialog(manager.GetString("fix_creditcard"), manager.GetString("fix_creditcard_title"));
                    await dialog.ShowSafelyAsync();
                    pageID = (string)e.Parameter;
                    MainPivot.SelectedIndex = GetSectionIndex(pageID);                    
                    break;
               case "BOOKING_CANCELED":
                    dialog = new MessageDialog(manager.GetString("canceled"), manager.GetString("canceled_title"));
                    await dialog.ShowSafelyAsync();
                    ViewModel.Navigator.RemoveBackEntry();
                    ViewModel.Navigator.GoBack();
                    break;
               case "PAYMENT_FAILED":
                    dialog = new MessageDialog(manager.GetString("payment_failed"), manager.GetString("payment_failed_title"));
                    await dialog.ShowSafelyAsync();
                    ViewModel.Navigator.GoBack();
                    break;
               case "RISK_PAYMENT_FAILED":                               
                    string phone = GetContactPhone();
                    string phrase2 = manager.GetString("Flights_Checkout_Card_Data_Card_ERROR_OP_PAYMENT_FAILED");
                    dialog = new MessageDialog(String.Format(phrase2, phone), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    ViewModel.Navigator.GoBack();
                    break;                                     
                case "COMPLETE_BOOKING_BOOKING_FAILED":
                    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_BOOKING_FAILED"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    // Go back to Results
                    ViewModel.Navigator.RemoveBackEntry();
                    ViewModel.Navigator.GoBack();
                    break;
                case "VOUCHER_VALIDITY_ERROR":
                    dialog = new MessageDialog(manager.GetString("Voucher_ERROR_" + (string)e.Parameter), manager.GetString("Voucher_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();                    
                    break;
                case "API_ERROR":
                    int code = (int)e.Parameter;
                    var formErrors = new int[]
                    {
                        1007,  // INVALID_DOCUMENT_NUMBER
                        1008,  // INVALID_PASSENGER_LAST_NAME_LENGTH
                        1009,  // INVALID_PASSENGER_FIRST_NAME_LENGTH
                        1011,  // INVALID_NATIONALITY
                        1013  // INVALID_BIRTHDAY                        
                    };

                    if (formErrors.Any(x => x == code))
                    {
                        dialog = new MessageDialog(manager.GetString("Flights_Checkout_ERROR_FORM_ERROR"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                        await dialog.ShowSafelyAsync();
                        return;
                    }

                    if (code == 1010)
                    {
                        // Expired Session
                        dialog = new MessageDialog(manager.GetString("Flights_Checkout_ERROR_SESSION_EXPIRED"), String.Empty);
                        ViewModel.Navigator.GoBack();
                        ViewModel.Navigator.GoBack();
                        await dialog.ShowSafelyAsync();
                        return;
                    }

                    dialog = new MessageDialog(manager.GetString("Flights_Checkout_ERROR_FORM_ERROR"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                    ViewModel.Navigator.GoBack();
                    break;
                    // TODO: CHECKOUT SESSION EXPIRED -> Handle that error
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
            } 
            catch(Exception )
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
                MainPivot.Items.Remove(MainPivot.FindName("Pivot_INVOICE"));
            }

            if (!ViewModel.BillingAddressRequired)
            {
                MainPivot.Items.Remove(MainPivot.FindName("Pivot_BILLING_ADDRESS"));
            }
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
            e.Handled = true;

            if (ViewModel.IsLoading)
                return;

            if (ViewModel.NationalityIsOpen)
            {
                ViewModel.NationalityIsOpen = false;
                return;
            }

            ViewModel.Navigator.GoBack();
        }

    }
}