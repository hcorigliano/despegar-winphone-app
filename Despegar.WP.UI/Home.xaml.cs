using BugSense;
using Despegar.Core.Neo.Business.Notifications;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Common;
using Despegar.WP.UI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.Networking.PushNotifications;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI
{    
    public sealed partial class Home : Page
    {
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
        public List<Despegar.Core.Neo.Business.Configuration.Product> products;
        public HomeViewModel ViewModel { get; set; }
        private bool versionChecked = false;
        private IMAPINotifications notifications;
        private PushResponse registerResponse;        

        public Home()
        {
            this.InitializeComponent();   

#if DECOLAR
            // Remove Country Selection from BAR for DECOLAR
            //CommandBar bar = BottomAppBar as CommandBar;
            //bar.PrimaryCommands.RemoveAt(0);               
            this.BottomAppBar = null;    
#endif
         
            //Google Analytics
#if !DEBUG
            GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
            ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
            ga.SendView("Home");
#endif

            //Notifications
            InitializeNotification();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = IoC.Resolve<HomeViewModel>();
            ViewModel.PropertyChanged += Checkloading;
            ViewModel.OnNavigated(e.Parameter);

            this.DataContext = ViewModel;

            this.VersionTxt.Text = "v" + GetAppVersion();

            if(e.NavigationMode == NavigationMode.New)
                await SetupMenuItems(GlobalConfiguration.Site);

            if (!versionChecked)
            {
#if !DEBUG
                await ValidateUpdate();
#endif
                versionChecked = true;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
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
        private async Task SetupMenuItems(string country)
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
    
        private string GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        private async Task ValidateUpdate()
        {
#if DECOLAR
            string productID = "e544d4bb-be44-4db8-9882-268f0b5631a3";
#else
            string productID = "f7d63cbc-dae6-4608-b695-31a1e095c4e7";
#endif
            ResourceLoader manager = new ResourceLoader();

            bool needsUpdate =  await ViewModel.ValidateAppVersion(GetAppVersion());

            if (needsUpdate) 
            {
                MessageDialog dialog = new MessageDialog(manager.GetString("Home_Update_Error"), manager.GetString("Home_Update_Error_Title"));
                await dialog.ShowSafelyAsync();
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:navigate?appid=" + productID));
                App.Current.Exit();
            }
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            string selectedButton = (string)((e.ClickedItem as Grid).DataContext);

            switch (selectedButton)
            {
                case "Hotels":
                    ViewModel.NavigateToHotels.Execute(null);
                    break;
                case "Flights":
                    ViewModel.NavigateToFlights.Execute(null);
                    break;
                //case "MyDespegar":
                //    var f = Window.Current.Content as Frame;
                //    //f.Navigate(typeof(HotelsCheckout), null);
                //    break;
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


        #region Notification
        private async void InitializeNotification()
        {
            try
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

                PushRegistrationRequest pnr = new PushRegistrationRequest();
                pnr.upa_id = Despegar.WP.UI.Model.GlobalConfiguration.UPAId;
                pnr.token = channel.Uri;
                pnr.social_id = String.Empty;
                pnr.country_id = (Despegar.WP.UI.Model.GlobalConfiguration.Site == null) ? "es" : Despegar.WP.UI.Model.GlobalConfiguration.Site.ToLower();
                pnr.device_type = Despegar.WP.UI.Model.GlobalConfiguration.DeviceType;
                pnr.brand = Despegar.WP.UI.Model.GlobalConfiguration.Brand;

                this.notifications = IoC.Resolve<IMAPINotifications>();

                if (Despegar.WP.UI.Model.GlobalConfiguration.ChannelUri == null && channel != null)
                {
                    registerResponse = await this.notifications.RegisterOnDespegarCloud(pnr);
                }

                if (Despegar.WP.UI.Model.GlobalConfiguration.ChannelUri != null && channel != null && channel.Uri != Despegar.WP.UI.Model.GlobalConfiguration.ChannelUri)
                {

                    registerResponse = await this.notifications.RegisterOnDespegarCloud(pnr);
                }


                Despegar.WP.UI.Model.GlobalConfiguration.ChannelUri = channel.Uri;

            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
        }

        void channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            //this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    TextBlock notification = new TextBlock();
            //    string result = args.NotificationType.ToString();
            //    switch (args.NotificationType)
            //    {
            //        case PushNotificationType.Badge:
            //            result += ": " + args.BadgeNotification.Content.GetXml();
            //            break;
            //        case PushNotificationType.Raw:
            //            result += ": " + args.RawNotification.Content;
            //            break;
            //        case PushNotificationType.Tile:
            //            result += ": " + args.TileNotification.Content.GetXml();
            //            break;
            //        case PushNotificationType.TileFlyout:
            //            result += ": " + args.TileNotification.Content.GetXml();
            //            break;
            //        case PushNotificationType.Toast:
            //            result += ": " + args.ToastNotification.Content.GetXml();
            //            break;
            //        default:
            //            break;
            //    }
            //    notification.Text = result;
            //});
        }
        #endregion

     
         # region ** ERROR HANDLING **
        private async void ErrorHandler(object sender, ViewModelErrorArgs e)
        {
            ResourceLoader manager = new ResourceLoader();

            switch (e.ErrorCode)
            {
                case "VALIDATE_APP_ERROR":
                    MessageDialog dialog = new MessageDialog(manager.GetString("Page_Home_Configuration_Error"), "Error");
                    await dialog.ShowSafelyAsync();
                    Application.Current.Exit();
                break;
            }
        }
        #endregion        
   
    }
}