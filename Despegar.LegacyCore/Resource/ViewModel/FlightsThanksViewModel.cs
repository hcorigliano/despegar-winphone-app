using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Model;
using Despegar.LegacyCore.Connector.Domain.API;

namespace Despegar.LegacyCore.ViewModel
{
    public class FlightsThanksViewModel : INotifyPropertyChanged
    {

        public FlightsThanksViewModel()
        {
            Loading = "Visible";
            Logger.Info("[vm:flight:checkout] Flight Thanks ViewModel initialized");
            InitializeFlightThanks();
        }

        public void InitializeFlightThanks()
        {
            LastBookResponse = LastFlightBookData.LastBookResponse;
            AvailabilityModel = LastFlightBookData.AvailabilityModel;
            AvailabilityInfo = LastFlightBookData.AvailabilityInfo;
            PassengerDefinitions = LastFlightBookData.PassengerDefinitions;
            CardDefinition = LastFlightBookData.CardDefinition;
            //VoucherDefinitions = LastFlightBookData.VoucherDefinitions;

            NotifyPropertyChanged("LastBookResponse");
            NotifyPropertyChanged("AvailabilityModel");
            NotifyPropertyChanged("AvailabilityInfo");
            NotifyPropertyChanged("PassengerDefinitions");
            NotifyPropertyChanged("CardDefinition");
            //NotifyPropertyChanged("VoucherDefinitions");

            registerBookingToDPNS( LastBookResponse.data.checkoutId );

            Loading = "Collapsed";
            NotifyPropertyChanged("Loading");
        }

        public async void registerBookingToDPNS(string checkoutId)
        {
            DPNSModel PushNotifications = new DPNSModel();
            await PushNotifications.RegisterBooking("FLIGHT", checkoutId);
        }


        public string Loading { get; set; }

        public FlightBookingBook LastBookResponse { get; set; }
        public FlightsAvailabilityModel AvailabilityModel { get; set; }

        public FlightAvailabilityItem AvailabilityInfo { get; set; }
        public List<FlightPassengerDefinition> PassengerDefinitions { get; set; }
        public FlightCardDefinition CardDefinition { get; set; }
        public List<FlightVoucherDefinition> VoucherDefinitions { get; set; }

        public void NotifyPropertyChanged(string propertyName) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
