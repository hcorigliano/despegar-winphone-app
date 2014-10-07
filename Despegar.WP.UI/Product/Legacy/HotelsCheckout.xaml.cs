using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation; 
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Info;
using System.Windows.Data;
using System.Windows.Media.Imaging;
//using Despegar.Analytics;
using Despegar.Core.Util;
using Despegar.Core.ViewModel;
using Despegar.Core;
using Despegar.Core.Resource;
using System.Collections.ObjectModel;
using Despegar.Core.Model;
using Microsoft.Phone.Net.NetworkInformation;
using System.Collections;
using Despegar.Core.Connector.Domain.API;
using CrittercismSDK;


namespace Despegar.View
{
    public partial class HotelsCheckout : PhoneApplicationPage
    {
        public HotelsCheckoutViewModel CheckoutViewModel { get; set; }
        public Style InputErrStyle;
        public Style InputStyle;
        private bool acceptTermsAndConditions;

        public HotelsCheckout()
        {
            InitializeComponent();
            
            #if DECOLAR
            MainLogo.Source = new BitmapImage(new Uri("/Assets/Image/decolar-logo.png", UriKind.RelativeOrAbsolute));
            #endif

            if (NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                return;

            CardSelector.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 30);
            MonthPicker.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 20);
            YearPicker.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 15);
            CheckoutViewModel =  new  HotelsCheckoutViewModel();
            CheckoutViewModel.FieldsLoaded += ViewModel_FieldsLoaded;
            HotelsCheckoutView.DataContext = CheckoutViewModel;
            FillStatesListPicker();            
        }

        private void  ViewModel_FieldsLoaded(object sender, EventArgs e)
        {
            Logger.Info("[view:HotelsCheckout]: Booking fields loaded");
            if (!CheckoutViewModel.InvoiceDefinitionIsRequired)
                CheckoutForm.Items.Remove(InvoiceDefinitionPivotItem); 
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

        private  async void  FillStatesListPicker()
        {
            StatesListPicker.ItemsSource =  (IEnumerable)(await CheckoutViewModel.GetAllStatesAsync());
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Logger.Info(String.Format("[view:hotel:checkout] Hotel Checkout page navigated with params {0}", this.CheckoutViewModel.ToString() ));
            //Track.View("HotelsCheckoutPage");

            if (NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                NavigationService.Navigate(new Uri("/View/ConnectionError.xaml", UriKind.Relative));
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

            CheckoutViewModel.CardDefinition.Cards = new ObservableCollection<Core.Connector.Domain.API.HotelCreditCard>();
            CheckoutViewModel.AvailabilityInfo.paymentMethod.payments.Find(it => { return it.id == CheckoutViewModel.PaymentId; }).creditCards.ForEach(it =>
            {
                CheckoutViewModel.CardDefinition.Cards.Add(it);
            });

            CardSelector.ItemsSource = CheckoutViewModel.CardDefinition.Cards;
            Logger.Info(String.Format("[view:hotel:checkout] Selected payment:{0}", CheckoutViewModel.PaymentId));
        }

        private void Show_SubPaymentMethods_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowSubPaymentMethods.Begin();
            SubPaymentMethod.Visibility = Visibility.Visible;
            PayWithInterestRadio.IsChecked = true;

            bool anySelected = false;
            CheckoutViewModel.AvailabilityInfo.PayWithInterest.ForEach(it => {
                if (it.Selected) {
                    CheckoutViewModel.PaymentId = it.id;
                    anySelected = true;
                }
            });

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


        private void Input_Focus(object sender, RoutedEventArgs e)
        {
            //ApplicationBar.IsVisible = true;
        }

        private void Input_Blur(object sender, RoutedEventArgs e)
        {
            PhoneTextBox input = sender as PhoneTextBox;
            input.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            
            StackPanel stack = input.Parent as StackPanel;
            CheckoutViewModel.Input_Blur(stack.DataContext);

            //ApplicationBar.IsVisible = false;
        }

        private void InputRepeat_Blur(object sender, RoutedEventArgs e)
        {
            PhoneTextBox input = sender as PhoneTextBox;
            input.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            StackPanel stack = input.Parent as StackPanel;
            CheckoutViewModel.InputRepeat_Blur(stack.DataContext);
        }

        private void CardNumber_Validation(object sender, RoutedEventArgs e)
        {
            PhoneTextBox input = sender as PhoneTextBox;
            input.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            StackPanel stack = input.Parent as StackPanel;
            string cardErr = CheckoutViewModel.CardNumber_Validation(stack.DataContext, CardSelector.DataContext);

            if (cardErr != "")
            {
                InvalidCardNumberLabel.Visibility = Visibility.Visible;
                InvalidCardNumberLabel.Text = String.Format(Properties.CheckoutLabel_Error_InvalidCardNumber, cardErr);
            }
            else
                InvalidCardNumberLabel.Visibility = Visibility.Collapsed;
                
        }

        private void SecurityCode_Validation(object sender, RoutedEventArgs e)
        {
            PhoneTextBox input = sender as PhoneTextBox;
            input.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            StackPanel stack = input.Parent as StackPanel;
            if (CheckoutViewModel.SecurityCode_Validation(stack.DataContext, CardSelector.DataContext))
            {
                InvalidSecurityCodeLabel.Visibility = Visibility.Visible;
                InvalidSecurityCodeLabel.Text = Properties.CheckoutLabel_Error_InvalidSecurityCode;
            }
            else
                InvalidSecurityCodeLabel.Visibility = Visibility.Collapsed;
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
            ConfigurationModel Configuration = new ConfigurationModel();
            ApplicationConfig.Instance.ResetBrowsingPages(new Uri(Configuration.GetCurrentSecureDomain() + "book/hotels/checkout/conditions/wp"));
            NavigationService.Navigate(new Uri("/View/Browser.xaml", UriKind.RelativeOrAbsolute));
        }

        private void FiscalStatus_Changed(object sender, EventArgs e)
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

            
            if (selected == "FINAL_CONSUMER") //TODO: use enum constant class
            {
                this.razonSocial.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.razonSocial.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private async void ValidateAndBuy_Click(object sender, RoutedEventArgs e)
        {
            bool err = false;
            string errMessage = String.Empty;

            if (!acceptTermsAndConditions)
            {
                AcceptConditionsError.Visibility = Visibility.Visible;
                errMessage = Properties.CheckoutLabel_Error_YouHaveToReadAndAcceptTermsAndConditions;
                err = true;
            }
            else
            {
                errMessage = await CheckoutViewModel.ValidateAndBuy();
                err = !string.IsNullOrEmpty(errMessage);
            }
            
            if (!err)
                NavigationService.Navigate(new Uri("/View/HotelsThanks.xaml", UriKind.RelativeOrAbsolute));

            else if (errMessage == Properties.CheckoutLabel_Message_AdditionalDataNeeded)
                NavigationService.Navigate(new Uri("/View/HotelsRiskQuestions.xaml", UriKind.RelativeOrAbsolute));

            else
                MessageBox.Show(errMessage, Properties.CheckoutLabel_Message_CheckTheInformation, MessageBoxButton.OK);
        }

        private async void Cities_Changed(object sender, EventArgs e)
        {
            //States_Changed
            State selected = (State)StatesListPicker.SelectedItem;            
            if (citiesAutoComplete.Text.ToString() != "")
            {
                citiesAutoComplete.ItemsSource = (IEnumerable)(await CheckoutViewModel.GetStringCityAsync(citiesAutoComplete.Text.ToString(), (int)selected.oid));
            }
        }

        private void City_Focus_Lost(object sender, EventArgs e)
        {
            //Force complete city when focus lost
            if (citiesAutoComplete.Text.Length >= 3 && citiesAutoComplete.ItemsSource != null)
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

        private void SelectedCityChanged(object sender, EventArgs e)
        {
            //Saves ID city in InvoiceDefinition
            City selected = (City)citiesAutoComplete.SelectedItem;
            if (selected != null)
            {
                CheckoutViewModel.InvoiceDefinition.billingAddress.cityId.value = selected.id.ToString();
            }
        }
        
        private void States_Changed(object sender, EventArgs e)
        {
            citiesAutoComplete.Text = "";
            //Saves ID State in InvoiceDefinition
            if (CheckoutViewModel.InvoiceDefinition != null)
            {
                State selected = (State)StatesListPicker.SelectedItem;
                CheckoutViewModel.InvoiceDefinition.billingAddress.stateId.value = selected.oid.ToString();
            }
        }

        
    }
}