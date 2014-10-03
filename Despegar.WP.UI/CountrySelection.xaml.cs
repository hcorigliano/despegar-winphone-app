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
            //var element = resourceLoader.GetString("Country_Code_Argentina");

            this.Items = new ObservableCollection<CountryItem>
            {
                new CountryItem{ Code= resourceLoader.GetString("Country_Code_Argentina"), CountryName = resourceLoader.GetString("Country_Name_Argentina") },
                new CountryItem{ Code= resourceLoader.GetString("Country_Code_Brazil"), CountryName = resourceLoader.GetString("Country_Name_Brazil") },
                new CountryItem{ Code= resourceLoader.GetString("Country_Code_Mexico"), CountryName = resourceLoader.GetString("Country_Name_Mexico") },
            };


            this.DataContext = this;
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            CountryItem countrySelected = e.ClickedItem as CountryItem;
            PagesManager.GoTo(typeof(Home), e);
        }   
    }
}
