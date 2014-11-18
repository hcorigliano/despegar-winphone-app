using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls.Flights;
using Despegar.WP.UI.Model.Classes.Flights;
using Despegar.WP.UI.Model.ViewModel.Flights;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Windows.UI.Xaml;

namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightMultipleEdit : Page
    {
        private NavigationHelper navigationHelper;
        public MultipleEditionViewModel ViewModel { get; set; }

        public FlightMultipleEdit()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            ViewModel = new MultipleEditionViewModel(Navigator.Instance);
            this.DataContext = ViewModel;
            this.CheckDeveloperTools();
        }   

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            
        }

        // TODO: what to do with this
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //this.navigationHelper.OnNavigatedTo(e);
            ViewModel.Initialize((EditMultiplesNavigationData)e.Parameter);
            MainPivotControl.ItemsSource = ViewModel.Segments;
            MainPivotControl.SelectedIndex = ViewModel.SelectedNavigationIndex - 1;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //this.navigationHelper.OnNavigatedFrom(e);
        }

        //private void MainPivotControl_LayoutUpdated(object sender, object e)
        //{
        //    // Set AutosuggestBox Text properties  TODO: SET THIS TO BE THE INITIAL PROPERTY BINDING OF THE TEXT in AUTOSUGGESTBOX, in SEARCHAIROT,  Public property, not Dependency
        //    //int index = 0;
        //    //foreach (PivotItem pivotItem in this.FindVisualChildren<PivotItem>(this))
        //    //{
        //    //    SearchAirport userControl = this.FindVisualChildren<SearchAirport>(pivotItem).First();
        //    //    var segment = ViewModel.Segments[index];

        //    //    userControl.UpdateAirportBoxes(
        //    //        segment.AirportOrigin,
        //    //        segment.AirportOriginText,
        //    //        segment.AirportDestination,
        //    //        segment.AirportDestinationText);

        //    //    index++;
        //    //}
        //}
      
    }
}