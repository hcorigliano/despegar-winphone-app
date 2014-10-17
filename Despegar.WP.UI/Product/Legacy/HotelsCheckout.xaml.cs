using Despegar.LegacyCore;
using Despegar.LegacyCore.Connector.Domain.API;
//using System.Windows.Data;
//using System.Windows.Media.Imaging;
using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.ViewModel;
using Despegar.WP.UI.Classes;
using Despegar.WP.UI.Product.Legacy;
using Despegar.WP.UI.Strings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


namespace Despegar.View
{
    public partial class HotelsCheckout : Page
    {
        public HotelsCheckoutViewModel CheckoutViewModel { get; set; }
        private bool acceptTermsAndConditions;

        public HotelsCheckout()
        {
            InitializeComponent();
            
            #if DECOLAR
            MainLogo.Source = new BitmapImage(new Uri("/Assets/Image/decolar-logo.png", UriKind.RelativeOrAbsolute));
            #endif

            if (!NetworkInterface.GetIsNetworkAvailable())
                return;

            //CardSelector.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 30);
            //MonthPicker.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 20);
            //YearPicker.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 15);

            CheckoutViewModel =  new  HotelsCheckoutViewModel();
            CheckoutViewModel.FieldsLoaded += ViewModel_FieldsLoaded;
            HotelsCheckoutView.DataContext = CheckoutViewModel;
            FillStatesListPicker();                   
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                PagesManager.GoTo(typeof(ConnectionError), null);
                return;
            }

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Application.Current.Exit();
            }
            else
            {
                ApplicationConfig.Instance.BrowsingPages.Pop();
                PagesManager.GoBack();
                PagesManager.ClearPageCache();
            }
        }

        private void ViewModel_FieldsLoaded(object sender, EventArgs e)
        {
            Logger.Info("[view:HotelsCheckout]: Booking fields loaded");
            if (!CheckoutViewModel.InvoiceDefinitionIsRequired)
                CheckoutForm.Items.Remove(InvoiceDefinitionPivotItem);
        }

        private void Input_Focus(object sender, RoutedEventArgs e)
        {
            //ApplicationBar.IsVisible = true;
        }

        private void Input_Blur(object sender, RoutedEventArgs e)
        {
            TextBox input = sender as TextBox;
            input.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            StackPanel stack = input.Parent as StackPanel;
            CheckoutViewModel.Input_Blur(stack.DataContext);

            //ApplicationBar.IsVisible = false;
        }

        private void InputRepeat_Blur(object sender, RoutedEventArgs e)
        {
            TextBox input = sender as TextBox;
            input.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            StackPanel stack = input.Parent as StackPanel;
            CheckoutViewModel.InputRepeat_Blur(stack.DataContext);
        }

        private void Image_Load_Failed(dynamic sender, ExceptionRoutedEventArgs e)
        {
            //sender.Source = new BitmapImage(new Uri(String.Format("{0}", sender.DataContext.cardCode), UriKind.Absolute));
            sender.Width = 0.0;
            Logger.Warn(String.Format("[view:hotel:checkout] {0}", sender.DataContext.cardCode));
        }

        private void PaymentMethod_Click(dynamic sender, RoutedEventArgs e)
        {
            CheckoutViewModel.PaymentId = sender.DataContext.id;
            sender.DataContext.Selected = true;

            CheckoutViewModel.CardDefinition.Cards = new ObservableCollection<Despegar.LegacyCore.Connector.Domain.API.HotelCreditCard>();
            var items = CheckoutViewModel.AvailabilityInfo.paymentMethod.payments.Find(it => { return it.id == CheckoutViewModel.PaymentId; }).creditCards;
                
                foreach (var it in items)
	            {
		             CheckoutViewModel.CardDefinition.Cards.Add(it);
	            }
                
            CardSelector.ItemsSource = CheckoutViewModel.CardDefinition.Cards;
            Logger.Info(String.Format("[view:hotel:checkout] Selected payment:{0}", CheckoutViewModel.PaymentId));
        }
        
        private void Show_SubPaymentMethods_Click(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ShowSubPaymentMethods.Begin();
            SubPaymentMethod.Visibility = Visibility.Visible;
            PayWithInterestRadio.IsChecked = true;

            bool anySelected = false;

            foreach (var it in CheckoutViewModel.AvailabilityInfo.PayWithInterest)
            {
               if (it.Selected)
                {
                    CheckoutViewModel.PaymentId = it.id;
                    anySelected = true;
                } 
            }

            if (!anySelected && CheckoutViewModel.AvailabilityInfo.PayWithInterest.Count > 0)
                CheckoutViewModel.AvailabilityInfo.PayWithInterest[0].Selected = true;

            Logger.Info(String.Format("[view:hotel:checkout] Selected payment:{0}", CheckoutViewModel.PaymentId));
        }

        private void PaymentMethod_Unchecked(dynamic sender, RoutedEventArgs e)
        {
            sender.DataContext.Selected = false;
        }

        private void Hide_SubPaymentMethods_Click(object sender, RoutedEventArgs e)
        {
            SubPaymentMethod.Visibility = Visibility.Collapsed;
        }

        private void CardNumber_Validation(object sender, RoutedEventArgs e)
        {
            TextBox input = sender as TextBox;
            input.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            StackPanel stack = input.Parent as StackPanel;
            string cardErr = CheckoutViewModel.CardNumber_Validation(stack.DataContext, CardSelector.DataContext);

            if (cardErr != "")
            {
                InvalidCardNumberLabel.Visibility = Visibility.Visible;
                InvalidCardNumberLabel.Text = String.Format(AppResources.GetLegacyString("CheckoutLabel_Error_InvalidCardNumber"), cardErr);
            }
            else
                InvalidCardNumberLabel.Visibility = Visibility.Collapsed;

        }

        private void SecurityCode_Validation(object sender, RoutedEventArgs e)
        {
            TextBox input = sender as TextBox;
            input.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            StackPanel stack = input.Parent as StackPanel;
            if (CheckoutViewModel.SecurityCode_Validation(stack.DataContext, CardSelector.DataContext))
            {
                InvalidSecurityCodeLabel.Visibility = Visibility.Visible;
                InvalidSecurityCodeLabel.Text = AppResources.GetLegacyString("CheckoutLabel_Error_InvalidSecurityCode");
            }
            else
                InvalidSecurityCodeLabel.Visibility = Visibility.Collapsed;
        }

        private void FillDataForTest(object sender, RoutedEventArgs e)
        {
            EmailConfirmTextBox.Text = EmailTextBox.Text = "testhoteles@despegar.com";
            TelephoneNumberTextBox.Text = "54156423";
            CardCreditNumberTextBox.Text = "4242424242424242";
            CreditCardDNITextBox.Text = "12123123";
            CreditCardFullNameTextBox.Text = "test booking";
            CreditCardSecurityCodeTextBox.Text = "123";           
        }

        private async void FillStatesListPicker()
        {
            StatesListPicker.ItemsSource =  (IEnumerable)(await CheckoutViewModel.GetAllStatesAsync());
        }
       
        private void AcceptConditions_Checked(object sender, RoutedEventArgs e)
        {
            acceptTermsAndConditions = true;
            AcceptConditionsError.Visibility = Visibility.Collapsed;
        }

        private void AcceptConditions_Unchecked(object sender, RoutedEventArgs e)
        {
            acceptTermsAndConditions = false;
        }

        private void AcceptConditions_Click(object sender, RoutedEventArgs e)
        {
            //ConfigurationModel Configuration = new ConfigurationModel();
            string domain = "https://secure.despegar.com.ar/book/hotels/checkout/conditions/wp";

            #if DECOLAR
                domain = "https://secure.decolar.com/book/hotels/checkout/conditions/wp";
            #endif

            ApplicationConfig.Instance.BrowsingPages.Push(new Uri(domain));
            PagesManager.GoTo(typeof(Browser),  null);
        }

        //private void SelectedCityChanged(object sender, EventArgs e)
        //{
        //    //Saves ID city in InvoiceDefinition
        //    City selected = (City)citiesAutoComplete.SelectedItem;
        //    if (selected != null)
        //    {
        //        CheckoutViewModel.InvoiceDefinition.billingAddress.cityId.value = selected.id.ToString();
        //    }
        //}

        private async void ValidateAndBuy_Click(object sender, RoutedEventArgs e)
        {
            bool err = false;
            string errMessage = String.Empty;

            if (!acceptTermsAndConditions)
            {
                AcceptConditionsError.Visibility = Visibility.Visible;
                errMessage = AppResources.GetLegacyString("CheckoutLabel_Error_YouHaveToReadAndAcceptTermsAndConditions");
                err = true;
            }
            else
            {
                errMessage = await CheckoutViewModel.ValidateAndBuy();
                err = !string.IsNullOrEmpty(errMessage);
            }

            if (!err)
               PagesManager.GoTo(typeof(HotelsThanks), null);

            //else if (errMessage == AppResources.GetLegacyString("CheckoutLabel_Message_AdditionalDataNeeded"))
            //    NavigationService.Navigate(new Uri("/View/HotelsRiskQuestions.xaml", UriKind.RelativeOrAbsolute));

            else {
                var messageDialog = new MessageDialog(errMessage, AppResources.GetLegacyString("CheckoutLabel_Message_CheckTheInformation"));
                messageDialog.Commands.Add(new UICommand("OK"));
                messageDialog.DefaultCommandIndex = 0;

                await messageDialog.ShowAsync();
            }
        }

        private void FiscalStatus_Changed(object sender, SelectionChangedEventArgs e)
        {
            string selected;
            if (FiscatStatusPicker.SelectedItem != null)
            {
                selected = ((MultivalueFieldOption)FiscatStatusPicker.SelectedItem).key;
            }
            else
            {
                selected = "FINAL_CONSUMER";
            }


            if (selected == "FINAL_CONSUMER")
            {
                this.razonSocial.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.razonSocial.Visibility = Visibility.Visible;
            }
        }

        private void States_Changed(object sender, SelectionChangedEventArgs e)
        {
            citiesAutoComplete.Text = "";

            // Saves ID State in InvoiceDefinition
            if (CheckoutViewModel.InvoiceDefinition != null)
            {
                State selected = (State)StatesListPicker.SelectedItem;
                CheckoutViewModel.InvoiceDefinition.billingAddress.stateId.value = selected.oid.ToString();
            }
        }

        private async void Cities_Changed(object sender, KeyRoutedEventArgs e)
        {
            // States_Changed
            State selected = (State)StatesListPicker.SelectedItem;
            if (!String.IsNullOrEmpty(citiesAutoComplete.Text))            
                citiesAutoComplete.ItemsSource = (IEnumerable)(await CheckoutViewModel.GetStringCityAsync(citiesAutoComplete.Text.ToString(), (int)selected.oid));            
        }

        private void City_Focus_Lost(object sender, RoutedEventArgs e)
        {
            // Force complete city when focus lost
            if (citiesAutoComplete.Text.Length > 2 && citiesAutoComplete.ItemsSource != null)
            {
                List<City> cities = (List<City>)citiesAutoComplete.ItemsSource;
                City city = cities.FirstOrDefault();
                if (city != null)
                {
                    citiesAutoComplete.Text = city.full_name;
                    CheckoutViewModel.InvoiceDefinition.billingAddress.cityId.value = city.id.ToString();
                }
                else
                {
                    citiesAutoComplete.Text = "";
                    CheckoutViewModel.InvoiceDefinition.billingAddress.cityId.value = "";
                }
            }
            else
            {
                citiesAutoComplete.Text = "";
                CheckoutViewModel.InvoiceDefinition.billingAddress.cityId.value = "";
            }
        }
       
       
    }
}