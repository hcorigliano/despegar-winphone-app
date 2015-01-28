using Despegar.WP.UI.Model.ViewModel.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Controls.Maps
{
    public class CustomMapViewModel
    {

        private ICollection<CustomPinPoint> _locations = new ObservableCollection<CustomPinPoint>();

        public ICollection<CustomPinPoint> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

    }
}
