using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Common;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using Despegar.WP.UI.Model.ViewModel.Hotels;
using Despegar.WP.UI.Product.Flights.Checkout.RiskQuestions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Hotels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HotelsCheckout : Page
    {
        private HotelsCheckoutViewModel ViewModel;
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
        private ModalPopup riskPopup;

        public HotelsCheckout()
        {
            this.InitializeComponent();

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

#if !DEBUG
                GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
                ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
                ga.SendView("HotelsCheckout");
#endif
        }
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            BugTracker.Instance.LeaveBreadcrumb("Hotel checkout start");
            HotelsCrossParameters crossParams = e.Parameter as HotelsCrossParameters;

            // Initialize Checkout
            ViewModel = new HotelsCheckoutViewModel(
                Navigator.Instance,                
                GlobalConfiguration.CoreContext.GetHotelService(),                
                GlobalConfiguration.CoreContext.GetCommonService(),
                GlobalConfiguration.CoreContext.GetConfigurationService(),
                GlobalConfiguration.CoreContext.GetCouponsService(),
                BugTracker.Instance,
                crossParams
              );

            ViewModel.PropertyChanged += Checkloading;
            ViewModel.ShowRiskReview += this.ShowRisk;
            ViewModel.HideRiskReview += this.HideRisk;
            ViewModel.ViewModelError += ErrorHandler;

            // Init Checkout
            await ViewModel.Init();

            // Set Defaults values and Country specifics
            ConfigureFields();

            this.DataContext = ViewModel;

            BugTracker.Instance.LeaveBreadcrumb("Hotels checkout ready");
        }

        /// <summary>
        /// View Adaptations based on selected country
        /// </summary>
        private void ConfigureFields()
        {
            if (!ViewModel.InvoiceRequired)
            {
                MainPivot.Items.RemoveAt(4);
            }
        }

        # region ** ERROR HANDLING **
        private async void ErrorHandler(object sender, ViewModelErrorArgs e)
        {
            BugTracker.Instance.LeaveBreadcrumb("Hotels checkout Error raised - " + e.ErrorCode);

            ResourceLoader manager = new ResourceLoader();
            MessageDialog dialog;
            string pageID;

            switch (e.ErrorCode)
            {
                //case "FORM_ERROR":
                //    dialog = new MessageDialog(manager.GetString("Flights_Checkout_ERROR_FORM_ERROR"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                //    await dialog.ShowSafelyAsync();

                //    // Go to Pivot with errors
                //    string sectionID = (string)e.Parameter;
                //    MainPivot.SelectedIndex = GetSectionIndex(sectionID);
                //    break;
                //case "TERMS_AND_CONDITIONS_NOT_CHECKED":
                //    dialog = new MessageDialog(manager.GetString("TermsAndConditions_ERROR"), manager.GetString("TermsAndConditions_ERROR_TITLE"));
                //    await dialog.ShowSafelyAsync();
                //    break;

                //case "BOOKING_FAILED":

                //    string ticketid = e.Parameter as string;
                //    ticketid = (ticketid != null) ? ticketid : String.Empty;
                //    string phrase = manager.GetString("Flights_Checkout_Card_Data_Card_ERROR_OP_BOOKING_FAILED");

                //    dialog = new MessageDialog(String.Format(phrase, ticketid), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                //    await dialog.ShowSafelyAsync();
                //    this.navigationHelper.GoBack();
                //    this.navigationHelper.GoBack();
                //    break;


                //case "COMPLETE_BOOKING_CONECTION_FAILED":
                //    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_FAILED"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                //    await dialog.ShowSafelyAsync();
                //    break;

                //case "CHECKOUT_INIT_FAILED":
                //    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_FAILED"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                //    await dialog.ShowSafelyAsync();
                //    this.navigationHelper.GoBack();
                //    break;
                //case "ONLINE_PAYMENT_ERROR_NEW_CREDIT_CARD":
                //    dialog = new MessageDialog(manager.GetString("Flights_Checkout_Card_Data_Card_ERROR_NEW_CREDIT_CARD"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                //    await dialog.ShowSafelyAsync();

                //    // Go to Pivot with errors
                //    pageID = (string)e.Parameter;
                //    MainPivot.SelectedIndex = GetSectionIndex(pageID);
                //    break;

                //case "ONLINE_PAYMENT_ERROR_FIX_CREDIT_CARD":

                //    dialog = new MessageDialog(manager.GetString("Flights_Checkout_Card_Data_Card_ERROR_ONLINE_PAYMENT_ERROR_FIX_CREDIT_CARD"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                //    await dialog.ShowSafelyAsync();
                //    pageID = (string)e.Parameter;
                //    MainPivot.SelectedIndex = GetSectionIndex(pageID);
                //    break;
                //case "ONLINE_PAYMENT_FAILED":
                //    {
                //        //string ticketid = e.Parameter as string;
                //        //ticketid = (ticketid != null) ? ticketid : String.Empty;
                //        string phone = GetContactPhone();
                //        string phrase2 = manager.GetString("Flights_Checkout_Card_Data_Card_ERROR_OP_PAYMENT_FAILED");
                //        dialog = new MessageDialog(String.Format(phrase2, phone), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                //        await dialog.ShowSafelyAsync();
                //        break;
                //    }
                //case "COMPLETE_BOOKING_BOOKING_FAILED":
                //    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_BOOKING_FAILED"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                //    await dialog.ShowSafelyAsync();
                //    this.navigationHelper.GoBack();
                //    this.navigationHelper.GoBack();
                //    break;
                //case "VOUCHER_VALIDITY_ERROR":
                //    dialog = new MessageDialog(manager.GetString("Voucher_ERROR_" + (string)e.Parameter), manager.GetString("Voucher_ERROR_TITLE"));
                //    await dialog.ShowSafelyAsync();
                //    break;
                //case "API_ERROR":
                //    dialog = new MessageDialog(manager.GetString("Flights_Checkout_ERROR_FORM_ERROR"), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                //    await dialog.ShowSafelyAsync();
                //    break;
                // TODO: CHECKOUT SESSION EXPIRED -> Handle that error
            }
        }
        #endregion
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

        private void ShowRisk(Object sender, EventArgs e)
        {
            riskPopup = new ModalPopup(new RiskQuestionsPopUp() { DataContext = ViewModel });
            riskPopup.Show();
        }

        private void HideRisk(Object sender, EventArgs e)
        {
            riskPopup.Hide();
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (ViewModel != null)
            {
                e.Handled = true;

                if (ViewModel.IsLoading)
                {
                    e.Handled = true;
                    return;
                }               

                ViewModel.Navigator.GoBack();
            }
        }
    }
}