using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsThanksViewModel : ViewModelBase
    {
        public HotelsThanksViewModel(INavigator nav, IBugTracker t) : base(nav, t) { }

        public override void OnNavigated(object navigationParams)
        {
        }
    }
}