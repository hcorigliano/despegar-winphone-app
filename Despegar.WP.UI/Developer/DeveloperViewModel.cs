﻿using Despegar.Core.Business;
using Despegar.Core.Log;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Controls.Flights;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Flights;
using Despegar.WP.UI.Models.Classes;
using Despegar.WP.UI.Product.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Developer
{
    public class DeveloperViewModel : ViewModelBase
    {
        public Rect Viewport { get { return Window.Current.Bounds; } }

        #region ** Service Mocks **
        public List<IGrouping<ServiceKey, MockOption>> MockGroups { get; set; }
        #endregion

        #region ** Other Tools **
        public bool DesignGridEnabled { 
            get { return MetroGridHelper.IsVisible; }
            set
            {
                OnPropertyChanged("DesignGridEnabled");
                MetroGridHelper.IsVisible = value;
            }
        }
      
        public Color DesignGridColor
        {
            get { return MetroGridHelper.Color; }
            set
            {
                OnPropertyChanged("DesignGridColor");
                MetroGridHelper.Color = value;
            }
        }
        
        public double Opacity
        {
            get { return MetroGridHelper.Opacity; }

            set {
                MetroGridHelper.Opacity = value;
                OnPropertyChanged("Opacity");
            }
        }

        public List<double> OpacityOptions
        {
            get { return new List<double>() { .1, .2, .3, .4, .5, .6, .7, .8, .9, 1 }; }
        }
        #endregion

        public DeveloperViewModel() : base(null)
        {
            // Load Mocks list
            var mocks = Mock.AllMocks
                .Select(x => new MockOption() { MockKey = x.MockID, ServiceKey = x.ServiceID, Name = x.MockID.ToString(), Enabled = GlobalConfiguration.CoreContext.IsMockEnabled(x.MockID) })
                .ToList();

            // Add "None" Option
            foreach (ServiceKey key in Enum.GetValues(typeof(ServiceKey)).Cast<ServiceKey>())
            {
                mocks.Add(new MockNoneOption() { ServiceKey = key });
            }

            MockGroups = mocks
                .GroupBy(x => x.ServiceKey)
                .OrderBy(g => g.Key)
                .ToList();                        
        }

        private async void ShowInvalidMessage()
        {
            Logger.Log("[Developer Tools]: Can't use this functionality in this page. Go to the correct page.");
            var msg = new MessageDialog("Not available for current View.");
            await msg.ShowSafelyAsync();
        }

        public ICommand FillFlightsOneWaySearchBox_MIAMI
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var page = (Window.Current.Content as Frame).Content as FlightSearch;
                    if (page == null)
                    {
                        ShowInvalidMessage();
                        return;
                    }

                    // Fill From EZE to MIA
                    FlightSearchViewModel viewModel = page.DataContext as FlightSearchViewModel;            
                    viewModel.PassengersViewModel.GeneralAdults = 1;
                    viewModel.PassengersViewModel.GeneralMinors = 0;
                    viewModel.FromDate = new System.DateTimeOffset(2015, 2, 10, 0, 0, 0, TimeSpan.FromDays(0));

                    // Update UI
                    var pivotItem = page.FindVisualChildren<PivotItem>(page).Skip(1).First();
                    var userControl = pivotItem.FindName("OneWaySearchAirportControl") as SearchAirport;
                    userControl.UpdateAirportBoxes("EZE", "Aeropuerto Buenos Aires Ministro ¨Pistarini Ezeiza, Buenos Aires, Argentina", "MIA", "Miami, Florida, Estados Unidos");
                
                });
            }
        }

        public ICommand FillFlightsTwoWaySearchBox_MIAMI {
            get { return new RelayCommand(() => {
                var page = (Window.Current.Content as Frame).Content as FlightSearch;
                if (page == null)
                {
                    ShowInvalidMessage();
                    return;
                }

                // Fill From EZE to MIA
                FlightSearchViewModel viewModel = page.DataContext as FlightSearchViewModel;               
                viewModel.PassengersViewModel.GeneralAdults = 1;
                viewModel.PassengersViewModel.GeneralMinors = 0;
                viewModel.FromDate = new System.DateTimeOffset(2015, 1, 21, 0, 0, 0, TimeSpan.FromDays(0));
                viewModel.ToDate = new System.DateTimeOffset(2015, 1, 28, 0, 0, 0, TimeSpan.FromDays(0));

                // Update UI
                var pivotItem = page.FindVisualChildren<PivotItem>(page).First();
                var userControl = page.FindVisualChildren<SearchAirport>(pivotItem).First();
                userControl.UpdateAirportBoxes("EZE", "Aeropuerto Buenos Aires Ministro ¨Pistarini Ezeiza, Buenos Aires, Argentina","MIA" ,"Miami, Florida, Estados Unidos");

            }); }
        }

        public ICommand FillFlightsMultipleSearchBox
        {
            get
            {
                return new RelayCommand(() =>
                {
                    
                    var page = (Window.Current.Content as Frame).Content as FlightSearch;
                    if (page == null)
                    {
                        ShowInvalidMessage();
                        return;
                    }

                    // Fill From EZE to MIA
                    FlightSearchViewModel viewModel = page.DataContext as FlightSearchViewModel;
                    viewModel.PassengersViewModel.GeneralAdults = 1;
                    viewModel.PassengersViewModel.GeneralMinors = 0;
                   
                    viewModel.FromDate = new System.DateTimeOffset(2015,5, 19, 0, 0, 0, TimeSpan.FromDays(0));
                    viewModel.ToDate = new System.DateTimeOffset(2015, 6, 19, 0, 0, 0, TimeSpan.FromDays(0));

                    // Update UI
                    //var pivotItem = page.FindVisualChildren<PivotItem>(page).Skip(2).First();
                    /*var userControl = page.FindVisualChildren<SearchAirport>(pivotItem).First();
                    userControl.UpdateAirportBoxes("EZE", "Aeropuerto Buenos Aires Ministro ¨Pistarini Ezeiza, Buenos Aires, Argentina", "MIA", "Miami, Florida, Estados Unidos");
                    */

                    Core.Business.Flight.SearchBox.FlightMultipleSegment fms1 = new Core.Business.Flight.SearchBox.FlightMultipleSegment();
                    fms1.DepartureDate = new System.DateTimeOffset(2015, 1, 21, 0, 0, 0, TimeSpan.FromDays(0));
                    fms1.AirportOrigin = "EZE";
                    fms1.AirportDestination = "LIM";


                    Core.Business.Flight.SearchBox.FlightMultipleSegment fms2 = new Core.Business.Flight.SearchBox.FlightMultipleSegment();
                    fms2.DepartureDate = new System.DateTimeOffset(2015, 2, 21, 0, 0, 0, TimeSpan.FromDays(0));
                    fms2.AirportOrigin = "LIM";
                    fms2.AirportDestination = "SAO";

                    Core.Business.Flight.SearchBox.FlightMultipleSegment fms3 = new Core.Business.Flight.SearchBox.FlightMultipleSegment();
                    fms3.DepartureDate = new System.DateTimeOffset(2015, 3, 21, 0, 0, 0, TimeSpan.FromDays(0));
                    fms3.AirportOrigin = "SAO";
                    fms3.AirportDestination = "EZE";

                    List<Core.Business.Flight.SearchBox.FlightMultipleSegment> segmentList = new List<Core.Business.Flight.SearchBox.FlightMultipleSegment>();
                    segmentList.Add(fms1);
                    segmentList.Add(fms2);
                    segmentList.Add(fms3);


                    viewModel.AddMultipleSegmentsMock(segmentList);

                    //coreSearchModel.MultipleSegments.Clear();
                    //coreSearchModel.MultipleSegments.AddRange(Segments);
                });
            }
        }

    }
}
