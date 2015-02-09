using Autofac;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Flights;
//using Despegar.WP.UI.Model.ViewModel.Hotels;

namespace Despegar.WP.UI.Model.InversionOfControl
{
    public class ViewModelModule : CoreModule
    {
        public ViewModelModule(bool isQA) : base(isQA)
        {  }

        protected override void Load(ContainerBuilder builder)
        {
            // Common
            builder.RegisterType<HomeViewModel>();
            builder.RegisterType<CountrySelectionViewModel>();

            // Flights
            builder.RegisterType<FlightSearchViewModel>();
            builder.RegisterType<MultipleEditionViewModel>();
            builder.RegisterType<FlightResultsViewModel>();
            builder.RegisterType<FlightFiltersViewModel>();
            builder.RegisterType<FlightDetailsViewModel>();
            builder.RegisterType<FlightsCheckoutViewModel>();
            builder.RegisterType<FlightThanksViewModel>();
            
            // Hotels
            //builder.RegisterType<HotelsSearchViewModel>();
            //builder.RegisterType<HotelsResultsViewModel>();
            //builder.RegisterType<HotelsDetailsViewModel>();
            //builder.RegisterType<HotelsCheckoutViewModel>();
            //builder.RegisterType<HotelsThanksViewModel>();
        }
    }
}