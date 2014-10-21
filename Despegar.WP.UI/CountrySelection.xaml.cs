using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources.Core;
using System.Collections.ObjectModel;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Classes;
using Windows.Storage;
using Despegar.WP.UI.Model;
using Despegar.LegacyCore.Connector;
using Despegar.LegacyCore;
using Despegar.Core.IService;
using Despegar.Core.Business.Configuration;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CountrySelection : Page
    {
        ObservableCollection<CountryItem> item;

        public ObservableCollection<CountryItem> Items
        {
            get { return item; }
            set { item = value; }
        }

        public CountrySelection()
        {
            var resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
                  
            this.DataContext = this;
            this.InitializeComponent();
        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            IConfigurationService configurationService = GlobalConfiguration.CoreContext.GetConfigurationService();            
            Configurations configuration = await configurationService.GetConfigurations();

            this.Items = new ObservableCollection<CountryItem>(configuration.configuration.Select(p => GetCountries(p)).ToList());
        }

        private CountryItem GetCountries(Configuration p)
        {
            CountryItem ci = new CountryItem();
            ci.Code = p.id;
            ci.CountryName = p.description;
            return ci;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //persist data in phone
            CountryItem ci = e.ClickedItem as CountryItem;
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            APIConnector.Instance.Channel = countrySelected.Code;  // TODO: Legacy code
            ApplicationConfig.Instance.Country = countrySelected.Code; // TODO: Legacy code
            roamingSettings.Values["countryCode"] = ci.Code;
            roamingSettings.Values["countryName"] = ci.CountryName;


            GlobalConfiguration.CoreContext.SetSite(ci.Code); 

            CountryItem countrySelected = ci;
            PagesManager.GoTo(typeof(Home), e);
        }   
    }
}
