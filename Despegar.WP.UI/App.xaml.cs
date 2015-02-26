using Despegar.WP.UI.Model;
using Despegar.WP.UI.Product.Flights;
using Despegar.WP.UI.Controls;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources;
using BugSense;
using BugSense.Model;
using BugSense.Core.Model;
using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.InversionOfControl;
using Despegar.Core.Neo.InversionOfControl;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Despegar.Core.Neo;
using Despegar.Core.Neo.API;

using Windows.Networking.PushNotifications;
using Windows.UI.Core;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Business.Notifications;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Despegar.WP.UI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;
        private IMAPINotifications notifications;
        private PushResponse registerResponse;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            #if !DEBUG
                // Initialize BugSense
                #if DECOLAR
                    BugSenseHandler.Instance.InitAndStartSession(new ExceptionManager(Current), "w8c0267b");
                #else
                    BugSenseHandler.Instance.InitAndStartSession(new ExceptionManager(Current), "w8c3afd3");
                #endif
            #else
                BugSenseHandler.Instance.InitAndStartSession(new ExceptionManager(Current), "w8c612a1"); //Test Crash Repo
            #endif


            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

        }

        private async static void NotifyAndClose()
        {
            MessageDialog dialog;
            ResourceLoader manager = new ResourceLoader();
            dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_FAILED"), manager.GetString("Flights_Search_ERROR_SEARCH_FAILED_TITLE")); //Error en la conexion a internet.
            await dialog.ShowSafelyAsync();
            Application.Current.Exit();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;                
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                
                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // Initialize Core
                try
                {
                    // Read and Load Mocks from XML file
                    LoadMocks();

                    await GlobalConfiguration.InitCore(new List<CoreModule>() { new WindowsPhoneModule(false) });
                }
                catch (Exception)
                {
                    NotifyAndClose();
                }
               
                // Check persist information
                var roamingSettings = ApplicationData.Current.RoamingSettings;

#if DECOLAR
                // Decolar forced to BRASIL always
                 roamingSettings.Values["countryCode"] = "BR";
#endif


                //Notifications
                InitializeNotification();


                // Load Country/Site
                 if (roamingSettings.Values["countryCode"] == null)
                 {
                     if (!rootFrame.Navigate(typeof(CountrySelection), e.Arguments))
                     {
                         throw new Exception("Failed to create initial page");
                     }
                 }
                 else
                 {
                     GlobalConfiguration.CoreContext.SetSite(roamingSettings.Values["countryCode"].ToString());
                     if (!rootFrame.Navigate(typeof(Home), e.Arguments))
                     {
                         throw new Exception("Failed to create Home page");
                     }
                 }
            }

            

            // Ensure the current window is active
            Window.Current.Activate();                    
        }

        private void LoadMocks()
        {
            string mocksPath = "mocks.xml";
            XDocument loadedData = XDocument.Load(mocksPath);

            foreach (XElement mock in loadedData.Descendants("Mock"))
            {
                Mock.AddMockToRepo(new Mock()
                {
                    MockName = mock.Attribute("name").Value,
                    ServiceID = (ServiceKey)Enum.Parse(typeof(ServiceKey), mock.Attribute("serviceKey").Value),
                    Content = mock.Value.Trim()
                });
            }
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }


        #region Notification
        private async void InitializeNotification()
        {
            try
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync("App");

                PushRegistrationRequest pnr = new PushRegistrationRequest();
                pnr.upa_id = Despegar.WP.UI.Model.GlobalConfiguration.UPAId;
                pnr.token = channel.Uri;
                pnr.social_id = String.Empty;
                pnr.country_id = (Despegar.WP.UI.Model.GlobalConfiguration.Site == null) ? "es" : Despegar.WP.UI.Model.GlobalConfiguration.Site;
                pnr.device_type = Despegar.WP.UI.Model.GlobalConfiguration.DeviceType;
                pnr.brand = Despegar.WP.UI.Model.GlobalConfiguration.Brand;

                this.notifications = IoC.Resolve<IMAPINotifications>();

                if (Despegar.WP.UI.Model.GlobalConfiguration.Channel == null && channel != null)
                {
                    registerResponse = await this.notifications.RegisterOnDespegarCloud(pnr);
                }

                if (Despegar.WP.UI.Model.GlobalConfiguration.Channel != null && channel != null && channel.Uri != Despegar.WP.UI.Model.GlobalConfiguration.Channel.Uri)
                {
                    
                    registerResponse = await this.notifications.RegisterOnDespegarCloud(pnr);
                }


                Despegar.WP.UI.Model.GlobalConfiguration.Channel = channel;

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

    }
}