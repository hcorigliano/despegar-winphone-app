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
            Configuration configuration = await configurationService.GetConfigurations();


            sitesListView.DataContext = configuration;
        }

        //private CountryItem GetCountries(Configuration p)
        //{
        //    //CountryItem ci = new CountryItem();
        //    //ci.Code = p.id;
        //    //ci.CountryName = p.description;
        //    //return ci;
        //}

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //persist data in phone
            Site countrySelected = e.ClickedItem as Site;
            var roamingSettings = ApplicationData.Current.RoamingSettings;

            roamingSettings.Values["countryCode"] = countrySelected.code;
            roamingSettings.Values["countryName"] = countrySelected.name;

            GlobalConfiguration.CoreContext.SetSite(countrySelected.code); 

            PagesManager.GoTo(typeof(Home), e);
        }   
    }
}
