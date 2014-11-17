using Despegar.Core.Business.Flight;
using Despegar.Core.Business.Flight.SearchBox;
using Windows.UI.Xaml.Controls;


namespace Despegar.WP.UI.Controls.Flights.Results
{
    public sealed partial class SearchMiniBoxControl : UserControl
    {
        public FlightSearchModel searchMiniboxModel { get; set; }

        public SearchMiniBoxControl()
        {
            this.InitializeComponent();
            this.DataContext = searchMiniboxModel;
        }
    }
}
