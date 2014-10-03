﻿using System;
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
using Despegar.WP.UI.Model;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Flight.BookingCompletePostResponse;
using Despegar.Core.Business.Flight.BookingCompletePost;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Despegar.WP.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private HomeModel hm = new HomeModel();

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void btnMainButton_Click(object sender, RoutedEventArgs e)
        {

            hm.test();
            //CitiesAutocomplete a = await hm.GetCities("bue");
            //FlightsItineraries b = await hm.GetItineraries("BUE", "LAX", "2014-10-10", 1, "2014-10-12", 0, 0, 0, 10, "", "", "ARS", "");

            BookingFieldPost bookingFieldPost = new BookingFieldPost();
            bookingFieldPost.inbound_choice = 1;
            bookingFieldPost.outbound_choice = 1;
            bookingFieldPost.itinerary_id = "prism_AR_0_FLIGHTS_A-1_C-0_I-0_RT-BUEMIA20141010-MIABUE20141013_xorigin-api!1!C_626893920!1,1";
                                            
                                            

            BookingFields c = await hm.GetBooking(bookingFieldPost);                

            Configurations d = await hm.GetConfigurations();
            int test = 1;

            string id =  "prism_AR_0_FLIGHTS_A-1_C-0_I-0_RT-BUEMIA20141010-MIABUE20141013_xorigin-api!0!C_1385824347!1,1";
            BookingCompletePost booking = new BookingCompletePost();
            
            



            //BookingCompletePostResponse e = await hm.GetBooking(booking,id );
        }

        private async void btnMainButton2_Click(object sender, RoutedEventArgs e)
        {            
        }
    }
}