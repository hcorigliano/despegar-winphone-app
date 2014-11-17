using Despegar.Core.Business.Flight.SearchBox;
using Despegar.WP.UI.Model.Classes.Flights;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model.ViewModel.Flights
{
    public class MultipleEditionViewModel : ViewModelBase
    {
        public FlightSearchModel coreSearchModel;
        public int selectedNavigationIndex;
        private INavigator navigator;

        public MultipleEditionViewModel(INavigator navigator, EditMultiplesNavigationData navigationData) 
        {
            this.navigator = navigator;
            this.coreSearchModel = navigationData.SearchModel;
            this.selectedNavigationIndex = navigationData.SelectedSegmentIndex;
        }

        public ICommand ApplyCommand
        {
            get
            {
                return new RelayCommand(() => { navigator.GoBack(); });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(() => navigator.GoBack());
            }
        }
    }
}
