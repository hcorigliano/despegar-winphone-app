using Despegar.Core.Business.Configuration;
using Despegar.Core.IService;
using Despegar.Core.Service;
using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI
{    
    public sealed partial class Home : Page
    {
        private IConfigurationService configurationService;
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
        public List<Despegar.Core.Business.Configuration.Product> products;
        public Despegar.WP.UI.Model.HomeViewModel ViewModel { get; set; }

        public Home()
        {
            this.InitializeComponent();   

            // Developer Tools
            this.CheckDeveloperTools();  

#if DECOLAR
            // Remove Country Selection from BAR for DECOLAR
            CommandBar bar = BottomAppBar as CommandBar;
            bar.PrimaryCommands.RemoveAt(0);           

    #if !DEBUG
            this.BottomAppBar = null;
    #endif
#endif
         
            //Google Analytics
            #if !DEBUG
            GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
            ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
            ga.SendView("Home");
            #endif
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

        // TODO: Refactor. Use DATABINDINGS!!
        private async void SetupMenuItems(string country)
        {
            products = await ViewModel.GetProducts(country);

            // Failed to get configurations
            if (products == null) 
            {
                ResourceLoader manager = new ResourceLoader();
                MessageDialog dialog = new MessageDialog(manager.GetString("Page_Home_Configuration_Error"), "Error");
                await dialog.ShowSafelyAsync();
                Application.Current.Exit();
                return;
            }

            if (products.Exists(x => x.name == "hotels" && x.status == "ENABLED") )
                Hotels.Visibility = Visibility.Visible;
            else
                Hotels.Visibility = Visibility.Collapsed;

            if (products.Exists(x => x.name == "flights" && x.status == "ENABLED"))
                Flights.Visibility = Visibility.Visible;
            else
                Flights.Visibility = Visibility.Collapsed;

        }
    
        public static string GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        private async Task ValidateUpdate()
        {
            bool error = false;
#if DECOLAR
            string productID = "e544d4bb-be44-4db8-9882-268f0b5631a3";
#else
            string productID = "f7d63cbc-dae6-4608-b695-31a1e095c4e7";
#endif
            BugTracker.Instance.LeaveBreadcrumb("Validate App Version");
            ResourceLoader manager = new ResourceLoader();
            configurationService = GlobalConfiguration.CoreContext.GetConfigurationService();

            try
            {
                UpdateFields data = await configurationService.CheckUpdate(GetAppVersion(), "8.1", "X", "X");
                
                if (data.force_update)
                {                    
                    MessageDialog dialog = new MessageDialog(manager.GetString("Home_Update_Error"), manager.GetString("Home_Update_Error_Title"));
                    await dialog.ShowSafelyAsync();
                    await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:navigate?appid=" + productID));
                    App.Current.Exit();
                }

                BugTracker.Instance.LeaveBreadcrumb("Update Service call succesful");
            }
            catch (Exception ex)
            {
                BugTracker.Instance.LogException(ex);
                error = true;
            }

            if (error) 
            {
                MessageDialog errorDialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_FAILED"), manager.GetString("Flights_Search_ERROR_SEARCH_FAILED_TITLE"));
                await errorDialog.ShowSafelyAsync();
                App.Current.Exit();
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            BugTracker.Instance.LeaveBreadcrumb("Home View");

            var parameter = e.Parameter as HomeParameters;

            ViewModel = new Despegar.WP.UI.Model.HomeViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetConfigurationService(), parameter, BugTracker.Instance);
            ViewModel.PropertyChanged += Checkloading;
            SetupMenuItems(GlobalConfiguration.Site);

            this.DataContext = ViewModel;

            await ValidateUpdate(); 
        }   

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            string selectedButton = (string)((e.ClickedItem as Grid).DataContext);

            switch (selectedButton)
            {
                case "Hotels":
                    ViewModel.NavigateToHotelsLegacy.Execute(AppResources.GetLegacyString("HomeProductHotelsUrl"));
                    break;
                case "Flights":
                    ViewModel.NavigateToFlights.Execute(null);
                    break;
                //case "MyDespegar":
                    //var f= Window.Current.Content as Frame;
                    //f.Navigate(typeof(FlightCheckout), null);
                    //break;
                default:

#if DECOLAR
                    var dialog = new MessageDialog("Esta funcionalidade estará disponível em breve.", "Em Breve");
#else
                    var dialog = new MessageDialog("Proximamente estará disponible esta funcionalidad.", "Proximamente");
#endif
                                        
                    await dialog.ShowSafelyAsync();
                    
                    break;
            }
        }
        
    }
}