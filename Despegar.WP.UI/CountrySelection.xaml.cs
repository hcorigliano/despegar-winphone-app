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
using Windows.Storage;
using Despegar.WP.UI.Model;
using Despegar.Core.IService;
using Despegar.Core.Business.Configuration;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.BugSense;

namespace Despegar.WP.UI
{
    public sealed partial class CountrySelection : Page
    {
        public CountrySelectionViewModel ViewModel { get; set; }

        public CountrySelection()
        {
            this.InitializeComponent();
            ViewModel = new CountrySelectionViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetConfigurationService(), BugTracker.Instance);
            DataContext = ViewModel;
            this.CheckDeveloperTools();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            ViewModel.LoadConfigurations(); 
        }
        
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //persist data in phone
            Site countrySelected = e.ClickedItem as Site;

            // Persist selection
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["countryCode"] = countrySelected.code;
            roamingSettings.Values["countryName"] = countrySelected.name; // not used?
            roamingSettings.Values["countryLanguage"] = countrySelected.language; // not used?
                    
            ViewModel.ChangeCountry(countrySelected);
            ViewModel.NavigateToHome.Execute(null);
        }
    }
}