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
    public class HotelsThanksViewModel : INotifyPropertyChanged
    {

        public HotelsThanksViewModel()
        {
            Loading = "Visible";
            Logger.Info("[vm:hotel:checkout] Hotel Thanks ViewModel initialized");
            InitializeHotelThanks();
        }

        public void InitializeHotelThanks()
        {
            LastBookResponse = LastHotelBookData.LastBookResponse;
            AvailabilityModel = LastHotelBookData.AvailabilityModel;
            AvailabilityInfo = LastHotelBookData.AvailabilityInfo;
            PassengerDefinitions = LastHotelBookData.PassengerDefinitions;
            CardDefinition = LastHotelBookData.CardDefinition;
            VoucherDefinitions = LastHotelBookData.VoucherDefinitions;

            NotifyPropertyChanged("LastBookResponse");
            NotifyPropertyChanged("AvailabilityModel");
            NotifyPropertyChanged("AvailabilityInfo");
            NotifyPropertyChanged("PassengerDefinitions");
            NotifyPropertyChanged("CardDefinition");
            NotifyPropertyChanged("VoucherDefinitions");

            //registerBookingToDPNS(LastBookResponse.data.checkoutId);

            Loading = "Collapsed";
            NotifyPropertyChanged("Loading");
        }


        //public async void registerBookingToDPNS(string checkoutId)
        //{
        //    DPNSModel PushNotifications = new DPNSModel();
        //    await PushNotifications.RegisterBooking("HOTEL", checkoutId);
        //}

        public string Loading { get; set; }

        public HotelBookingBook LastBookResponse { get; set; }
        public HotelsAvailabilityModel AvailabilityModel { get; set; }

        public HotelAvailabilityItem AvailabilityInfo { get; set; }
        public List<HotelPassengerDefinition> PassengerDefinitions { get; set; }
        public HotelCardDefinition CardDefinition { get; set; }
        public List<HotelVoucherDefinition> VoucherDefinitions { get; set; }
        
        public void NotifyPropertyChanged(string propertyName) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
