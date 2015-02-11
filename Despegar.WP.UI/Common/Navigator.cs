using Despegar.Core.Neo.Log;
using Despegar.WP.UI.Controls.PhotoGallery;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Product.Flights;
using Despegar.WP.UI.Product.Hotels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Common
{
    /// <summary>
    /// Specific Implementation of the Navigator
    /// It maps the different ViewModels with their respective Views
    /// </summary>
    public class Navigator : INavigator
    {
        private static Navigator _instance;
        public static Navigator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Navigator();

                return _instance;
            }
        }

        public void GoTo(ViewModelPages page, object data)
        {
            Type view = null;

            switch (page)
            {
                case ViewModelPages.Home:
                    view = typeof(Home);
                    break;
                case ViewModelPages.CountrySelecton:
                    view = typeof(CountrySelection);
                    break;                
                 // FLIGHTS
                case ViewModelPages.FlightsSearch:
                    view = typeof(FlightSearch);
                    break;
                case ViewModelPages.FlightsMultiplEdit:
                    view = typeof(FlightMultipleEdit);
                    break;
                case ViewModelPages.FlightsResults:
                    view = typeof(FlightResults);
                    break;
                case ViewModelPages.FlightsFilters:
                    view = typeof(FlightFilters);
                    break;
                case ViewModelPages.FlightsOrderBy:
                    view = typeof(FlightSortBy);
                    break;
                case ViewModelPages.FlightsDetails:
                    view = typeof(FlightDetail);
                    break;
                case ViewModelPages.FlightsCheckout:
                    view = typeof(FlightCheckout);
                    break;
                case ViewModelPages.FlightsThanks:
                    view = typeof(FlightThanks);
                    break;
                // HOTELS
                case ViewModelPages.HotelsSearch:
                    view = typeof(HotelsSearch);
                    break;
                case ViewModelPages.HotelsResults:
                    view = typeof(HotelsResults);
                    break;
                case ViewModelPages.HotelsDetails:
                    view = typeof(HotelsDetails);
                    break;
                case ViewModelPages.HotelsFilter:
                    view = typeof(HotelsFilters);
                    break;
                case ViewModelPages.HotelsOrderBy:
                    view = typeof(HotelsSortBy);
                    break;
                case ViewModelPages.HotelsAmenities:
                    view = typeof(HotelsAmenities);
                    break;                   
                case ViewModelPages.HotelsCheckout:
                    view = typeof(HotelsCheckout);
                    break;
                case ViewModelPages.PhotoPresenter:
                    view = typeof(PhotoPresenter);
                    break;
                // PACKAGES
                // CARS
                // Add More pages-viewmodel mappings down here
            }

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null)
            {
                rootFrame.Navigate(view, data);
            }
        }

        public void ClearStack()
        {
            ((Frame)Window.Current.Content).BackStack.Clear();
        }

        public void RemoveBackEntry()
        {
            var backStack = ((Frame)Window.Current.Content).BackStack;
            backStack.RemoveAt(backStack.Count-1);
        }

        public void GoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack) rootFrame.GoBack();
        }

        // TODO: isnt' this the same as Clearing the stack?
        public void ClearPageCache()
        {
            Frame rootFrame = Window.Current.Content as Frame;

            var cacheSize = rootFrame.CacheSize;
            rootFrame.CacheSize = 0;
            rootFrame.CacheSize = cacheSize;
        }
    } 
}