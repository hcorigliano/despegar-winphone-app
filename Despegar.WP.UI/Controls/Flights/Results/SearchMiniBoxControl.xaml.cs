using Despegar.Core.Neo.Business.Flight;
using Despegar.Core.Neo.Business.Flight.SearchBox;
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
