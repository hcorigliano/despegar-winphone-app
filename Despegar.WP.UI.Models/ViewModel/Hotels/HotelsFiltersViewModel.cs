using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.Business.Hotels.SearchBox;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Interfaces;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using Despegar.WP.UI.Model.ViewModel.Classes;

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
            BugTracker.LeaveBreadcrumb("Hotels search Filter View");
            var param = navigationParams as GenericResultNavigationData;

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
                    SearchModel.Facets = EditableFacets;  // Apply the filters                    
                    Navigator.GoTo(ViewModelPages.HotelsResults, new GenericResultNavigationData() { SearchModel = SearchModel, FiltersApplied = true });
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