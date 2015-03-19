using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Common;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using Despegar.WP.UI.Model.ViewModel.Hotels;
using Despegar.WP.UI.Product.Hotels.Checkout;
//using Despegar.WP.UI.Product.Flights.Checkout;
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
    public sealed partial class HotelsCheckout : Page
    {
        private HotelsCheckoutViewModel ViewModel { get; set; }
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
        private ModalPopup riskPopup;
        private bool pivotInstallmentIsLoaded { get; set; }

        public HotelsCheckout()
        {
            this.InitializeComponent();

#if !DEBUG
                GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
                ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
                ga.SendView("HotelsCheckout");
#endif
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            // Initialize Checkout
            ViewModel = IoC.Resolve<HotelsCheckoutViewModel>();
            ViewModel.PropertyChanged += Checkloading;
            ViewModel.ShowRiskReview += this.ShowRisk;
            ViewModel.HideRiskReview += this.HideRisk;
            ViewModel.ViewModelError += ErrorHandler;
            ViewModel.OnNavigated(e.Parameter);
            // Init Checkout
            await ViewModel.Init();
            // Set Defaults values and Country specifics
            ConfigureFields();
            this.DataContext = ViewModel;

#if DEBUG
            ViewModel.FillBookingFields();
#endif 

            ViewModel.BugTracker.LeaveBreadcrumb("Hotels checkout ready");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        /// <summary>
        /// View Adaptations based on selected country
        /// </summary>
        private void ConfigureFields()
        {
            if (ViewModel.SelectedCard.card == null && ViewModel.CoreBookingFields.form.CardInfo == null && ViewModel.CoreBookingFields.form.Voucher == null)
            {
                MainPivot.Items.Remove(MainPivot.FindName("Pivot_CARD"));
            }
        }

        # region ** ERROR HANDLING **

        /// <summary>
        /// Error handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">pareter with an error code</param>
        private async void ErrorHandler(object sender, ViewModelErrorArgs e)
        {
            ViewModel.BugTracker.LeaveBreadcrumb("Hotels checkout Error raised - " + e.ErrorCode);

            ResourceLoader manager = new ResourceLoader();
            MessageDialog dialog;
            string pageID;

            switch (e.ErrorCode)
            {
                case "FORM_ERROR":
                    dialog = new MessageDialog(manager.GetString("Hotels_Checkout_ERROR_FORM_ERROR"), manager.GetString("Hotels_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();

                    // Go to Pivot with errors
                    string sectionID = (string)e.Parameter;
                    MainPivot.SelectedIndex = GetSectionIndex(sectionID);
                    break;
                case "TERMS_AND_CONDITIONS_NOT_CHECKED":
                    dialog = new MessageDialog(manager.GetString("TermsAndConditions_ERROR"), manager.GetString("TermsAndConditions_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    break;

                case "BOOKING_FAILED":

                    string ticketid = e.Parameter as string;
                    ticketid = (ticketid != null) ? ticketid : String.Empty;
                    string phrase = manager.GetString("Hotels_Checkout_Card_Data_Card_ERROR_OP_BOOKING_FAILED");

                    dialog = new MessageDialog(String.Format(phrase, ticketid), manager.GetString("Hotels_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    ViewModel.Navigator.GoBack();

                    break;


                case "COMPLETE_BOOKING_CONECTION_FAILED":
                    dialog = new MessageDialog(manager.GetString("Hotels_Search_ERROR_SEARCH_FAILED"), manager.GetString("Hotels_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    break;

                case "CHECKOUT_INIT_FAILED":
                    dialog = new MessageDialog(manager.GetString("Hotels_Search_ERROR_SEARCH_FAILED"), manager.GetString("Hotels_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    ViewModel.Navigator.GoBack();
                    break;
                case "ONLINE_PAYMENT_ERROR_NEW_CREDIT_CARD":
                    dialog = new MessageDialog(manager.GetString("Hotels_Checkout_Card_Data_Card_ERROR_NEW_CREDIT_CARD"), manager.GetString("Hotels_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();

                    // Go to Pivot with errors
                    pageID = (string)e.Parameter;
                    MainPivot.SelectedIndex = GetSectionIndex(pageID);
                    break;

                case "ONLINE_PAYMENT_ERROR_FIX_CREDIT_CARD":

                    dialog = new MessageDialog(manager.GetString("Hotels_Checkout_Card_Data_Card_ERROR_ONLINE_PAYMENT_ERROR_FIX_CREDIT_CARD"), manager.GetString("Hotels_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    pageID = (string)e.Parameter;
                    MainPivot.SelectedIndex = GetSectionIndex(pageID);
                    break;
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
                case "COMPLETE_BOOKING_BOOKING_FAILED":
                    dialog = new MessageDialog(manager.GetString("Hotels_Search_ERROR_SEARCH_FAILED"), manager.GetString("Hotels_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    ViewModel.Navigator.GoBack();
                    ViewModel.Navigator.GoBack();
                    break;
                //case "VOUCHER_VALIDITY_ERROR":
                //    dialog = new MessageDialog(manager.GetString("Voucher_ERROR_" + (string)e.Parameter), manager.GetString("Voucher_ERROR_TITLE"));
                //    await dialog.ShowSafelyAsync();
                //    break;
                case "API_ERROR":
                    dialog = new MessageDialog(manager.GetString("Hotels_Checkout_ERROR_FORM_ERROR"), manager.GetString("Hotels_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
                    break;

                case "DUPLICATED_BOOKING":
                    dialog = new MessageDialog(manager.GetString("Hotels_Duplicated_Booking_Message"), manager.GetString("Hotels_Duplicated_Booking_Title"));
                    string iAgreeText = manager.GetString("Generic_Continuar");
                    string cancelText = manager.GetString("Generic_Cancel");

                    dialog.Commands.Add(new UICommand(iAgreeText, new UICommandInvokedHandler(this.CommandInvokedHandler)));
                    dialog.Commands.Add(new UICommand(cancelText, new UICommandInvokedHandler(this.CommandCancelHandler)));

                    await dialog.ShowSafelyAsync();
                    break;
                // TODO: CHECKOUT SESSION EXPIRED -> Handle that error
            }
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            ViewModel.ValidateAndBuyNoCheckDuplicates.Execute(false);
        }

        private void CommandCancelHandler(IUICommand command)
        {
            ViewModel.Navigator.GoBack();
        }

        #endregion

        private int GetSectionIndex(string sectionID)
        {
            PivotItem errorPivot = this.FindName("Pivot_" + sectionID) as PivotItem;
            return MainPivot.Items.IndexOf(errorPivot);
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
            if (e.PropertyName == "SelectedInstallment")
            {
                //Checks for invoice
                if (ViewModel.CoreBookingFields.CheckoutMethodSelected.payment != null && ViewModel.CoreBookingFields.CheckoutMethodSelected.payment.invoice != null)
                {
                    if (!MainPivot.Items.Any(x => ((PivotItem)x).Name == "Pivot_INVOICE"))
                    {

                        //Add Invoice. The subscription is necessary 
                        if (!pivotInstallmentIsLoaded)
                            Pivot_INSTALLMENT.Loaded += Insert_Invoice;
                        else
                            Insert_Invoice(null, null);
                    }
                }
                else
                {
                    if (MainPivot.Items.Any(x => ((PivotItem)x).Name == "Pivot_INVOICE"))
                    {
                        //Eliminar Invoice
                        MainPivot.Items.Remove(MainPivot.FindName("Pivot_INVOICE"));
                    }
                }
            }
        }

        private void Insert_Invoice(object sender, RoutedEventArgs e)
        {
            PivotItem pivotItem = new PivotItem();
            pivotItem.Header = "factura fiscal";
            pivotItem.Name = "Pivot_INVOICE";
            UserControl usc = new InvoiceArgentina();
            usc.DataContext = this.DataContext;
            pivotItem.Content = usc;
            pivotItem.Margin = new Thickness(0, 3, 0, 0); 
            
            int index = FindIndexWithPivotItemName(MainPivot, "Pivot_CARD");
            MainPivot.Items.Insert(index + 1, pivotItem);

            pivotInstallmentIsLoaded = true;
            Pivot_INSTALLMENT.Loaded -= Insert_Invoice;
        }

        private void ShowRisk(Object sender, EventArgs e)
        {
            riskPopup = new ModalPopup(new Despegar.WP.UI.Product.Flights.Checkout.RiskQuestionsPopUp() { DataContext = ViewModel });
            riskPopup.Show();
        }

        private void HideRisk(Object sender, EventArgs e)
        {
            riskPopup.Hide();
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;

            if (ViewModel != null)
            {
                if (ViewModel.IsLoading)
                    return;

                ViewModel.Navigator.GoBack();
            }
        }

        private int FindIndexWithPivotItemName(Pivot mainPivot ,string name)
        {
            int i = 0;
            foreach(PivotItem pivotItem in mainPivot.Items)
            {
                if (pivotItem.Name == name)
                    return i;
                i++;
            }
            return -1;
        }
    }
}