using Despegar.Core.Business.Hotels.HotelsAutocomplete;
using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Despegar.WP.UI.Controls.Hotels
{
    public sealed partial class SearchHotelControl : UserControl
    {
        public static readonly DependencyProperty SelectedDestinationCodeProperty = DependencyProperty.Register("SelectedDestinationCode", typeof(string), typeof(SearchHotelControl), null);

        public static readonly DependencyProperty SelectedDestinationTextProperty = DependencyProperty.Register("SelectedDestinationText", typeof(string), typeof(SearchHotelControl), null);

        public static readonly DependencyProperty InitialDestinationTextProperty = DependencyProperty.Register("InitialDestinationText", typeof(string), typeof(SearchHotelControl), null);

        #region ** BoilerPlate Code **
        public event PropertyChangedEventHandler PropertyChanged;
        private void SetValueAndNotify(DependencyProperty property, object value, [CallerMemberName] string p = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        #endregion


        // Bindable Property from XAML
        public string SelectedDestinationCode
        {
            get { return (string)GetValue(SelectedDestinationCodeProperty); }
            set
            {
                SetValueAndNotify(SelectedDestinationCodeProperty, value);
            }
        }

        // Bindable Property from XAML
        public string SelectedDestinationText
        {
            get { return (string)GetValue(SelectedDestinationTextProperty); }
            set
            {
                SetValueAndNotify(SelectedDestinationTextProperty, value);
            }
        }

        // Bindable Property from XAML (This property is OneTime binding only. It is used to set the Autosuggest Text from XAML the first time the control is loaded.)
        public string InitialDestinationText
        {
            get { return (string)GetValue(InitialDestinationTextProperty); }
            set
            {
                SetValueAndNotify(InitialDestinationTextProperty, value);
            }
        }


        public SearchHotelControl()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }

        private async void HotelsTextBlock_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender.Text != "" && sender.Text.Length >= 3)
            {
                try
                {
                    sender.ItemsSource = await GetHotelsAutocomplete(sender.Text);
                }
                catch (Exception)
                {
                    // do nothing. Try again...
                }
            }
        }


       public async Task<HotelsAutocomplete> GetHotelsAutocomplete(string hotelString)
       {
           var hotelService = GlobalConfiguration.CoreContext.GetHotelService();
           return await hotelService.GetHotelsAutocomplete(hotelString);
       }

       private async void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
       {

       }

       private void Focus_Lost(object sender, RoutedEventArgs e)
       {
           UpdateTextbox((AutoSuggestBox)sender);
       }

       private void UpdateTextbox(AutoSuggestBox control)
       {
           // Force complete city when focus lost
           if (control.Text.Length > 2 && control.ItemsSource != null)
           {
               List<HotelAutocomplete> cities = (List<HotelAutocomplete>)control.ItemsSource;
               HotelAutocomplete city = cities.FirstOrDefault();
               if (city != null)
               {
                    control.Text = city.name;
                    SelectedDestinationCode = city.code;
                    SelectedDestinationText = city.name;
               }
               else
               {
                    control.Text = "";
                    SelectedDestinationCode = "";
                    SelectedDestinationText = "";
               }
           }
           else
           {
                control.Text = ""; 
                SelectedDestinationCode = "";
                SelectedDestinationText = "";    
           }
       }


    }
}
