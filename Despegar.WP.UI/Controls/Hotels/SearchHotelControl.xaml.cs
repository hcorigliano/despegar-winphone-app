using Despegar.Core.Neo.Business.Hotels.HotelsAutocomplete;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.InversionOfControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Despegar.WP.UI.Controls.Hotels
{
    public sealed partial class SearchHotelControl : UserControl
    {
        public static readonly DependencyProperty SelectedDestinationCodeProperty = DependencyProperty.Register("SelectedDestinationCode", typeof(int), typeof(SearchHotelControl), null);
        public static readonly DependencyProperty SelectedDestinationTextProperty = DependencyProperty.Register("SelectedDestinationText", typeof(string), typeof(SearchHotelControl), null);
        public static readonly DependencyProperty InitialDestinationTextProperty = DependencyProperty.Register("InitialDestinationText", typeof(string), typeof(SearchHotelControl), null);
        public static readonly DependencyProperty SelectedDestinationTypeProperty = DependencyProperty.Register("SelectedDestinationType", typeof(string), typeof(SearchHotelControl), null);

        private IMAPIHotels hotelService;

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
        public string SelectedDestinationType
        {
            get { return (string)GetValue(SelectedDestinationTypeProperty); }
            set
            {
                SetValueAndNotify(SelectedDestinationTypeProperty, value);
            }
        }

        // Bindable Property from XAML
        public int SelectedDestinationCode
        {
            get { return (int)GetValue(SelectedDestinationCodeProperty); }
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
            this.hotelService = IoC.Resolve<IMAPIHotels>();

            (this.Content as FrameworkElement).DataContext = this;

            HotelAutocomplete GeoItem = new HotelAutocomplete()
            {
                name = "+ Cerca de mi ubicacion actual",
                id = -1,
                type = "geo"
            };

            this.DestinyInput.ItemsSource = null;
            this.DestinyInput.ItemsSource = new List<HotelAutocomplete>() { GeoItem };
        }

       private async void HotelsTextBlock_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender.Text != "" && sender.Text.Length >= 3)
            {
                try
                {
                    sender.ItemsSource = null;
                    HotelAutocomplete GeoItem = new HotelAutocomplete();
                    GeoItem.name = "+ Cerca de mi ubicacion actual";
                    GeoItem.id = -1;
                    GeoItem.type = "geo";
                    List<HotelAutocomplete> source = new List<HotelAutocomplete>();
                    source.Add(GeoItem);
                    source.AddRange(await GetHotelsAutocomplete(sender.Text));
                    sender.ItemsSource = source;
                }
                catch (Exception)
                {
                    // do nothing. Try again...
                }
            }
        }

       public async Task<HotelsAutocomplete> GetHotelsAutocomplete(string hotelString)
       {
           return await hotelService.GetHotelsAutocomplete(hotelString);
       }

       private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
       {
           var selected = (HotelAutocomplete)args.SelectedItem;
           if (selected != null)
               SetHotel(sender, selected);
       }

       private void Clear(AutoSuggestBox control)
       {
            control.Text = "";
            SelectedDestinationCode = 0;
            SelectedDestinationText = "";

       }

       private void SetHotel(AutoSuggestBox sender, HotelAutocomplete selected)
       {
           SelectedDestinationCode = selected.id;
           SelectedDestinationText = selected.name;
           SelectedDestinationType = selected.type;
          
           sender.ItemsSource = null;
           List<HotelAutocomplete> source = new List<HotelAutocomplete>();
           source.Add(selected);
           sender.ItemsSource = source;
           sender.Text = SelectedDestinationText;
       }

       public void UpdateHotelsDestiny(int destinationCode, string destinationText, string destinationType)
       {
           SetHotel(DestinyInput, new HotelAutocomplete() { id = destinationCode, name = destinationText, type = destinationType });
       }

       private void DestinationInput_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
       {
           if (e.Key == Windows.System.VirtualKey.Back && SelectedDestinationCode != 0)
           {
               Clear((AutoSuggestBox)sender);
           }
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
                    SelectedDestinationCode = city.id;
                    SelectedDestinationText = city.name;
                    SelectedDestinationType = city.type;
               }
               else
               {
                    control.Text = "";
                    SelectedDestinationCode = 0;
                    SelectedDestinationText = "";
                    SelectedDestinationType = "";
               }
           }
           else
           {
                control.Text = ""; 
                SelectedDestinationCode = 0;
                SelectedDestinationText = "";
                SelectedDestinationType = "";
           }
       }

    }
}
