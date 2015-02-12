using Despegar.Core.Neo.Business.Flight.Itineraries;
using Despegar.Core.Neo.Business.Flight.SearchBox;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model.ViewModel.Flights
{
    public class FlightFiltersViewModel : ViewModelBase
    {
        public FlightSearchModel SearchModel { get; set; }
        public List<Facet> EditableFacets { get; set; }

        public FlightFiltersViewModel(INavigator nav, IBugTracker t) : base(nav,t)
        {                
        }

        public override void OnNavigated(object navigationParams)
        {
            BugTracker.LeaveBreadcrumb("Flight search Filter View");
            var param = navigationParams as GenericResultNavigationData;

            // make a copy in order to cancelation
            this.EditableFacets = ((FlightSearchModel)param.SearchModel).Facets.Select(x => Facet.Copy(x)).ToList();
            this.SearchModel = (FlightSearchModel)param.SearchModel;            
        }

        public ICommand ApplyFilterCommand
        {
            get
            {
                return new RelayCommand(() => 
                {
                    SearchModel.Facets = EditableFacets;  // Apply the filters                    
                    Navigator.GoTo(ViewModelPages.FlightsResults, new GenericResultNavigationData() { SearchModel = SearchModel, FiltersApplied = true });
                });
            }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(() =>
            {
                Navigator.GoBack();
            });
            }
        }

    }
}