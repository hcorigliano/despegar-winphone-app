using Despegar.LegacyCore;
using Despegar.View;
using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Common;
using System;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Core;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

namespace Despegar.WP.UI.Product.Legacy
{
    public sealed partial class Browser : Page
    {
        private Uri _currentPage;
        private bool _firstPage;
        private static string XVERSION = "windowsphone";

        public Browser()
        {
            this.InitializeComponent();

            #if DECOLAR            
              // TODO: will not compile!
            MainLogo.Source = new BitmapImage(new Uri("ms-appx:/Product/Legacy/Assets/Image/decolar-logo.png", UriKind.Absolute));
            #endif            
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BugTracker.Instance.LeaveBreadcrumb("Hotels Legacy View");
            base.OnNavigatedTo(e);

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                OldPagesManager.GoTo(typeof(ConnectionError), null);
                return;
            }

            if (ApplicationConfig.Instance.BrowsingPages.Any())
            {
                _firstPage = true;
                // Headers configuration
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApplicationConfig.Instance.BrowsingPages.Peek());
                request.Headers.Append("X-Version", XVERSION);

                var task = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                          CoreDispatcherPriority.Normal,
                          () =>
                          {
                              EmbbededBrowser.NavigateWithHttpRequestMessage(request);
                          }
                );
            }

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void EmbbededBrowser_Navigating(object sender, WebViewNavigationStartingEventArgs e)
        {
            BugTracker.Instance.LeaveBreadcrumb("Hotel Navigating to:" + e.Uri.ToString());

            if (!_firstPage && !e.Uri.Equals(_currentPage)) 
            {
                e.Cancel = true;

                if (e.Uri.AbsoluteUri.StartsWith("javascript:void(0)") || e.Uri.AbsoluteUri.EndsWith("#"))
                    return;

                if (e.Uri.ToString().Contains("book/hotels") && !e.Uri.ToString().Contains("conditions"))
                {
                    ApplicationConfig.Instance.BrowsingPages.Push(e.Uri);
                    OldPagesManager.GoTo(typeof(HotelsCheckout), null);
                    return;
                }

                ApplicationConfig.Instance.BrowsingPages.Push(e.Uri);
                _currentPage = ApplicationConfig.Instance.BrowsingPages.Peek();

                // Open URL in Browser
                var task = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                         CoreDispatcherPriority.Normal,
                         () =>
                         {
                             EmbbededBrowser.Navigate(ApplicationConfig.Instance.BrowsingPages.Peek());
                         }
               );
            }

            if (_firstPage && !e.Uri.Equals(_currentPage)) 
            { 
                ApplicationConfig.Instance.BrowsingPages.Push(e.Uri);         
            }

            _firstPage = false;
            EmbbededBrowserHide.Begin();
        }

        private void EmbbededBrowser_Navigated(object sender, WebViewNavigationCompletedEventArgs e)
        {
            _currentPage = e.Uri;            

            string iata = getIataFromUri(_currentPage.ToString());
            ApplicationConfig.Instance.IATA = iata;
            // TileManager.SetIATATile();   TODO:  WHAT TO DO WITH THIS
        }

        private void EmbbededBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            EmbbededBrowserShow.Begin();
        }

        private void EmbbededBrowser_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            OldPagesManager.GoTo(typeof(ConnectionError), null);
        }

        private string getIataFromUri(string absoluteUri)
        {
            Regex regexCity = new Regex("city/(?<iata>[^/]*)");
            var matchCity = regexCity.Match(absoluteUri);
            if (matchCity.Success)
            {
                return matchCity.Groups["iata"].Value.ToUpper();
            }

            Regex regexFlights = new Regex("(roundtrip|oneway)/[^/]*/(?<iata>[^/]*)");
            matchCity = regexFlights.Match(absoluteUri);
            if (matchCity.Success)
            {
                return matchCity.Groups["iata"].Value.Substring(2).ToUpper();
            }
            return string.Empty;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            BugTracker.Instance.LeaveBreadcrumb("Hotel Browser - Back button pressed");
            e.Handled = true;            

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                BugTracker.Instance.LeaveBreadcrumb("Hotel Browser - No internet availabel. Closing app.");
                Application.Current.Exit();
            }
            else
            {
                ApplicationConfig.Instance.BrowsingPages.Pop();

                if (ApplicationConfig.Instance.BrowsingPages.Any())
                {
                    var task = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                                CoreDispatcherPriority.Normal,
                                    () =>
                                    {
                                        EmbbededBrowser.Navigate(ApplicationConfig.Instance.BrowsingPages.Peek());
                                    }
                    );
                }  else {
                    // No more URLs,  Navigate to home
                    OldPagesManager.GoBack();
                }      
            }
            
        }
    }
}