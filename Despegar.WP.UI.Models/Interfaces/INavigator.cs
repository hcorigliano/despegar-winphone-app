using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Interfaces
{

    public enum ViewModelPages
    {
        Home, 
        CountrySelecton, 
        FlightsSearch, 
        FlightsResults, 
        FlightsDetails, 
        FlightsCheckout, 
        FlightsThanks, 
        FlightsMultiplEdit, 
        FlightsFilters, 
        HotelsDetails, 
        HotelsSearch, 
        HotelsResults, 
        HotelsCheckout, 
        PhotoPresenter, 
        HotelsAmenities,
        FlightsOrderBy,
        HotelsFilter,
        HotelsOrderBy,
        HotelsThanks,
        About
    }

    public interface INavigator
    {
       void GoTo(ViewModelPages page, object data);
       void ClearStack();
       void RemoveBackEntry();
       void GoBack();
    }
}