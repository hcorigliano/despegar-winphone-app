using Despegar.Core.Neo.Business.Configuration;
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
#if !DEBUG
                GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
                ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
                ga.SendView("HotelCheckout");
#endif
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            // Initialize Checkout
            ViewModel = IoC.Resolve<HotelsCheckoutViewModel>();
            ViewModel.PropertyChanged += Checkloading;
            ViewModel.PropertyChanged += CheckSections;
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
            if (ViewModel.SelectedCard==null || ViewModel.CoreBookingFields == null)
            {
                return;
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

                case "RISK_PAYMENT_FAILED":
                    string phone = GetContactPhone();
                    string phrase2 = manager.GetString("Flights_Checkout_Card_Data_Card_ERROR_OP_PAYMENT_FAILED");
                    dialog = new MessageDialog(String.Format(phrase2, phone), manager.GetString("Flights_Checkout_ERROR_FORM_ERROR_TITLE"));
                    await dialog.ShowSafelyAsync();
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
            catch (Exception)
            {
                //TODO add logs
                return String.Empty;
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
            if (errorPivot == null)
                return MainPivot.SelectedIndex;
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
        }

        private void CheckSections(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedInstallment")
            {
                var currentMethod = ViewModel.CoreBookingFields.form.CheckoutMethodSelected;

                // Checks for invoice
                if (ViewModel.CoreBookingFields.form.Invoice != null)
                {
                    if (!MainPivot.Items.Any(x => ((PivotItem)x).Name == "Pivot_INVOICE"))
                    {
                        //Add Invoice. The subscription is necessary 
                        if (!pivotInstallmentIsLoaded)
                        {
                            Pivot_INSTALLMENT.Loaded -= Insert_Invoice;
                            Pivot_INSTALLMENT.Loaded += Insert_Invoice;
                        }
                        else
                            Insert_Invoice(null, null);
                    }
                }
                else if (MainPivot.Items.Any(x => ((PivotItem)x).Name == "Pivot_INVOICE"))
                {
                    //Eliminar Invoice
                    MainPivot.Items.Remove(MainPivot.FindName("Pivot_INVOICE"));
                }


                //Check billingAddress
                if (ViewModel.CoreBookingFields.form.BillingAddress != null)
                {
                    if (!MainPivot.Items.Any(x => ((PivotItem)x).Name == "Pivot_BILLING_ADDRESS"))
                    {
                        //Add billingAddress
                        if (!pivotInstallmentIsLoaded)
                        {
                            Pivot_INSTALLMENT.Loaded -= Insert_Billing_Address;
                            Pivot_INSTALLMENT.Loaded += Insert_Billing_Address;
                        }
                        else
                            Insert_Billing_Address(null, null);
                    }
                }
                else if (MainPivot.Items.Any(x => ((PivotItem)x).Name == "Pivot_BILLING_ADDRESS"))
                {
                    //Eliminar billingAddress
                    MainPivot.Items.Remove(MainPivot.FindName("Pivot_BILLING_ADDRESS"));
                }


                //Check CardData
                if (ViewModel.CoreBookingFields.form.CardInfo != null)
                {
                    if (!MainPivot.Items.Any(x => ((PivotItem)x).Name == "Pivot_CARD"))
                    {
                        //Add CardData
                        if (!pivotInstallmentIsLoaded)
                        {
                            Pivot_INSTALLMENT.Loaded -= Insert_Card_Info;
                            Pivot_INSTALLMENT.Loaded += Insert_Card_Info;
                        }
                        else
                            Insert_Card_Info(null, null);
                    }
                }
                else if (MainPivot.Items.Any(x => ((PivotItem)x).Name == "Pivot_CARD"))
                {
                    //Eliminar CardData
                    MainPivot.Items.Remove(MainPivot.FindName("Pivot_CARD"));
                }           

            }
        }

        private void Insert_Card_Info(object sender, RoutedEventArgs e)
        {
            PivotItem pivotItem = new PivotItem();

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            pivotItem.Header = loader.GetString("Hotels_Checkout_Card_Data_Header"); //Agregar a resource
            pivotItem.Name = "Pivot_CARD";
            UserControl usc = new CardData();
            usc.DataContext = this.DataContext;
            pivotItem.Content = usc;
            pivotItem.Margin = new Thickness(0, 3, 0, 0);

            int index = FindIndexWithPivotItemName(MainPivot, "Pivot_INSTALLMENT");

            MainPivot.Items.Insert(index + 1, pivotItem);

            pivotInstallmentIsLoaded = true;
            Pivot_INSTALLMENT.Loaded -= Insert_Card_Info;
        }

        private void Insert_Billing_Address(object sender, RoutedEventArgs e)
        {
            PivotItem pivotItem = new PivotItem();

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            pivotItem.Header = loader.GetString("Flight_Checkout_Billing_Address_Header"); //Agregar a resource
            pivotItem.Name = "Pivot_BILLING_ADDRESS";
            UserControl usc = new BillingAddress();
            usc.DataContext = this.DataContext;
            pivotItem.Content = usc;
            pivotItem.Margin = new Thickness(0, 3, 0, 0);


            int index = FindIndexWithPivotItemName(MainPivot, "Pivot_INVOICE");
            if (index == -1)
                index = FindIndexWithPivotItemName(MainPivot, "Pivot_CARD");


            MainPivot.Items.Insert(index + 1, pivotItem);

            pivotInstallmentIsLoaded = true;
            Pivot_INSTALLMENT.Loaded -= Insert_Billing_Address;
        }

        private void Insert_Invoice(object sender, RoutedEventArgs e)
        {
            // Add XUID, do not Harcode strings UPDATE: invoice is only for arg. & XUID its only for xaml , however we must add new resource.
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

        private async void AcceptConditions_Click(object sender, RoutedEventArgs e)
        {
            string uriToLaunch = String.Format("https://secure.despegar.com.{0}/book/hotels/checkout/conditions/wp", GlobalConfiguration.Site.ToLowerInvariant());

            if (GlobalConfiguration.Site.ToLowerInvariant() == "br")
                uriToLaunch = "https://secure.decolar.com/book/hotels/checkout/conditions/wp";

            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);     
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