using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Interfaces
{

    public enum ViewModelPages
    {
        Home, CountrySelecton, FlightsSearch, FlightsResults, FlightsDetails, FlightsCheckout, FlightsThanks, FlightsMultiplEdit, FlightsFilters, LegacyBrowser, HotelsDetails, HotelsSearch, HotelsResults, HotelsCheckout, PhotoPresenter, HotelsAmenities,
        FlightsOrderBy,

    }

    public interface INavigator
    {
       void GoTo(ViewModelPages page, object data);
       void ClearStack();
       void RemoveBackEntry();
       void GoBack();
    }
}