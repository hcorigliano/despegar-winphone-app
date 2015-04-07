using Despegar.Core.Neo.Contract.Log;
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
    public class HotelsThanksViewModel : ViewModelBase
    {
        public HotelsCrossParameters CrossParams { get; set; }
        public HotelsThanksViewModel(INavigator nav, IBugTracker t) : base(nav, t) { }

        public override void OnNavigated(object navigationParams)
        {
            CrossParams = (HotelsCrossParameters)navigationParams;
        }

        public ICommand NavigateToHomeCommand
        {
            get
            {
                return new RelayCommand(() => { this.NavigateToHome(); });
            }
        }
        private void NavigateToHome()
        {
            Navigator.GoTo(ViewModelPages.Home, new HomeParameters() { ClearStack = true });
        }

        
    }
}