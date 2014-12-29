using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightThanks : Page
    {
        private NavigationHelper navigationHelper;
        private FlightThanksViewModel ViewModel;

        public FlightThanks()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            FlightsCrossParameter crossParameters = e.NavigationParameter as FlightsCrossParameter;

            ViewModel = new FlightThanksViewModel(Navigator.Instance);
            ViewModel.FlightParameters = crossParameters;

            DataContext = ViewModel;
            if (ViewModel.FlightParameters.Inbound.choice == -1)
            {
                FlightsSegmentReturnTextBlock.Visibility = Visibility.Collapsed;
                FlightsSegmentReturnSrc.Visibility = Visibility.Collapsed;
            }
            if (ViewModel.FlightParameters.MultipleRoutes == null)
                FlightsSegmentGoSrcMultiple.Visibility = Visibility.Collapsed;
            else
                FlightsSegmentGoSrc.Visibility = Visibility.Collapsed;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

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

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            ViewModel.NavigateToHomeCommand.Execute(null);        
        }
    }
}
