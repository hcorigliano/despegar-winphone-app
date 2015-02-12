using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.Business.Hotels.SearchBox;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsSortByViewModel : ViewModelBase
    {
        public HotelSearchModel SearchModel { get; set; }
        public Sorting EditableSortingOptions { get; set; }

        public HotelsSortByViewModel(INavigator nav, IBugTracker t)
            : base(nav, t)
        {                
        }

        public override void OnNavigated(object navigationParams)
        {
            BugTracker.LeaveBreadcrumb("Hotels search Sort By View");

            var param = navigationParams as GenericResultNavigationData;

            // make a copy in order to support cancelation
            this.SearchModel = (HotelSearchModel)param.SearchModel;
            this.EditableSortingOptions = Sorting.Copy(SearchModel.Sortings);
        }

        public ICommand ApplyFilterCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SearchModel.Sortings = EditableSortingOptions;  // Apply the filters                    
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