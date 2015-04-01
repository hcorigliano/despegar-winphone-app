using Despegar.WP.UI.Model.ViewModel.Hotels;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Common;


namespace Despegar.WP.UI.Product.Hotels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HotelThanks : Page
    {
        private HotelsCheckoutViewModel ViewModel;

        public HotelThanks()
        {
            this.InitializeComponent();

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if !DEBUG
                GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
                ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
                ga.SendView("HotelThanks");
#endif
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            ViewModel = Despegar.Core.Neo.InversionOfControl.IoC.Resolve<HotelsCheckoutViewModel>();
            ViewModel.OnNavigated(e.Parameter);
            this.DataContext = ViewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            ViewModel.Navigator.GoTo(ViewModelPages.Home, new HomeParameters() { ClearStack = true });
        }
    }
}
