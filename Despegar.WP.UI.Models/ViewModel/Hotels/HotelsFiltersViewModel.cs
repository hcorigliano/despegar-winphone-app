using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.Business.Hotels.SearchBox;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Models.Classes;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsFiltersViewModel : ViewModelBase
    {
        public HotelSearchModel SearchModel { get; set; }
        public List<Facet> EditableFacets { get; set; }

        public HotelsFiltersViewModel(INavigator nav, IBugTracker t)
            : base(nav, t)
        {                
        }

        public override void OnNavigated(object navigationParams)
        {
            BugTracker.LeaveBreadcrumb("Flight search Filter View");
            var param = navigationParams as GenericFilterNavigationData;

            // make a copy in order to cancelation
            this.SearchModel = (HotelSearchModel)param.SearchModel;  
            this.EditableFacets = SearchModel.Facets.Select(x => Facet.Copy(x)).ToList();
            
        }

        public ICommand ApplyFilterCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //SearchModel.Facets = EditableFacets;  // Apply the filters                    
                    Navigator.GoTo(ViewModelPages.FlightsResults, new GenericFilterNavigationData() { SearchModel = SearchModel, FiltersApplied = true });
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Navigator.GoBack();
                });
            }
        }
    }
}