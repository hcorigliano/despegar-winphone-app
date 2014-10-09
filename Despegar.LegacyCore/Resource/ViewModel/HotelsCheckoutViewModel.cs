using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

using Despegar.LegacyCore.Connector.Domain.API;
using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Model;
using Despegar.LegacyCore;
using Despegar.LegacyCore.Resource;
using System.Text.RegularExpressions;


namespace Despegar.LegacyCore.ViewModel
{
    public class HotelsCheckoutViewModel : INotifyPropertyChanged
    {
        
        private HotelsBookingModel BookingModel { get; set; }
        private HotelBookingFields BookingFields { get; set; }
        private ValidationCreditcardsModel CreditCardsValidationModel { get; set; }
        private StatesModel StateModel { get; set; }
        private CitiesModel cityModel { get; set; }

        public HotelsAvailabilityModel AvailabilityModel { get; set; }
        public int PaymentId { get; set; }
        public string Loading { get; set; }
        public HotelAvailabilityItem AvailabilityInfo { get; set; }
        public List<HotelPassengerDefinition> PassengerDefinitions { get; set; }
        public HotelCardDefinition CardDefinition { get; set; }
        public List<HotelVoucherDefinition> VoucherDefinitions { get; set; }
        public HotelContactDefinition ContactDefinition { get; set; }
        public HotelInvoiceDefinition InvoiceDefinition { get; set; }
        public List<State> StatesDefinition { get; set; }
        public void NotifyPropertyChanged(string propertyName) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler FieldsLoaded;

        public HotelsCheckoutViewModel()
        {
            Loading = "Visible";
            Logger.Info("[vm:hotel:checkout] Hotel Checkout ViewModel initialized");
            InitializeHotelCheckout();
        }

        public async void InitializeHotelCheckout()
        {
            BookingModel = new HotelsBookingModel();
            AvailabilityModel = new HotelsAvailabilityModel();
            CreditCardsValidationModel = new ValidationCreditcardsModel();


            StateModel = new StatesModel();
            

            AvailabilityModel.SetParamsByUrl(ApplicationConfig.Instance.BrowsingPages.Peek());
            HotelAvailability Availability = await AvailabilityModel.GetAvailability();
            if (AvailabilityModel.Room != null) Availability.Item.SelectedRoom = AvailabilityModel.Room;

            if (Availability.Item.SessionTicket == null)
                Logger.Info("[vm:hotel:checkout] No session ticket for the current search");

            BookingFields = await BookingModel.GetBookingFields(Availability.Item.SessionTicket, ApplicationConfig.Instance.DeviceDescription);

            // prepare view model:
            AvailabilityInfo = Availability.Item;
            AvailabilityInfo.Currency = AvailabilityModel.Currency;
            AvailabilityInfo.Room.Prices.Currency = AvailabilityModel.Currency;
            AvailabilityInfo.Room.Prices.Nights = AvailabilityModel.Nights;
            AvailabilityInfo.Room.Prices.Rooms = AvailabilityModel.Rooms;
            BookingFields.data.SelectedRoom = AvailabilityInfo.SelectedRoom;
            BookingFields.data.InputDefinition.SetPassengerIndexes();
            
            Loading = "Collapsed";
            PassengerDefinitions = BookingFields.data.InputDefinition.passengerDefinitions;
            CardDefinition       = BookingFields.data.InputDefinition.paymentDefinition.cardDefinition;
            VoucherDefinitions   = BookingFields.data.InputDefinition.voucherDefinitions;
            ContactDefinition    = BookingFields.data.InputDefinition.contactDefinition;
            InvoiceDefinition = BookingFields.data.InputDefinition.paymentDefinition.InvoiceDefinition;
            cityModel = new CitiesModel();
            

            NotifyPropertyChanged("Loading");
            NotifyPropertyChanged("PassengerDefinitions");
            NotifyPropertyChanged("ContactDefinition");
            NotifyPropertyChanged("CardDefinition");
            NotifyPropertyChanged("VoucherDefinition");
            NotifyPropertyChanged("AvailabilityInfo");
            NotifyPropertyChanged("AvailabilityModel");
            NotifyPropertyChanged("InvoiceDefinition");



            await CreditCardsValidationModel.Sync();
            FieldsLoaded(this, new EventArgs());
        }


        public bool InvoiceDefinitionIsRequired { get { return InvoiceDefinition != null; } }

        public async Task<IEnumerable<State>> GetAllStatesAsync()
        {           
            return await StateModel.GetAll();             
        }

        public async Task<List<City>> GetStringCityAsync(string stringBusqueda, int idState)
        {
            return await cityModel.GetAll(stringBusqueda, idState);
        }

        public override string ToString()
        {
            return String.Format("{0}/{1}/{2}/{3}", AvailabilityModel.Hotel, AvailabilityModel.Checkin, AvailabilityModel.Checkout, AvailabilityModel.Distribution.ToString());
        }

        public void Input_Blur(object input)
        {
            TextField Input = input as TextField;
            Input.Validate();
        }

        public void InputRepeat_Blur(object input)
        {
            EmailTextField Input = input as EmailTextField;
            Input.ValidateRepeat();
        }

        public string CardNumber_Validation(object input, object creditCard)
        {
            TextField Input = input as TextField;
            Input.error = false;
            Input.Validate();
            string err = "";

            if (!Input.Error)
            {
                HotelCardDefinition card = creditCard as HotelCardDefinition;
                HotelCreditCard selected = card.Selected ?? card.Cards[0];
                Input.error = CreditCardsValidationModel.ValidateNumber(selected, Input.Value);
                Input.NotifyPropertyChanged("Error");
                if (Input.Error) err = selected.cardDescription;
            }

            return err;
        }

        public bool SecurityCode_Validation(object input, object creditCard)
        {
            TextField Input = input as TextField;
            Input.error = false;
            Input.Validate();

            if (!Input.Error)
            {
                HotelCardDefinition card = creditCard as HotelCardDefinition;
                HotelCreditCard selected = card.Selected ?? card.Cards[0];
                Input.error = CreditCardsValidationModel.ValidateCode(selected, Input.Value);
                Input.NotifyPropertyChanged("Error");
            }

            return Input.error;
        }

        public async Task<string> ValidateAndBuy()
        {
            //TODO:sacar mocked
            string errMsg = "";

            foreach (var item in PassengerDefinitions)
                item.Validate();           

            bool passErr = PassengerDefinitions.Any(it => { return it.Validate(); });
            bool contErr = ContactDefinition.Validate();
            bool cardErr = CardDefinition.Validate();
            bool fiscalDataErr = InvoiceDefinitionIsRequired ? InvoiceDefinition.Validate() : false;
            

            // other errors
            bool err = passErr || contErr || cardErr || fiscalDataErr;
            errMsg += Properties.CheckoutLabel_Message_CheckItPlease;
            errMsg += passErr ? Properties.CheckoutLabel_Message_CheckGuests : "";
            errMsg += contErr ? Properties.CheckoutLabel_Message_CheckContact : "";
            errMsg += cardErr ? Properties.CheckoutLabel_Message_CheckCardData : "";
            errMsg += Properties.CheckoutLabel_Message_BeCorrect;
            
            if (!err)
            {
                Loading = "Visible";
                NotifyPropertyChanged("Loading");
                LastHotelBookData.LastBookResponse = await BookingModel.Buy(AvailabilityInfo.SelectedRoom, PaymentId, BookingFields);


                if (LastHotelBookData.LastBookResponse.data != null)
                {
                    switch (LastHotelBookData.LastBookResponse.data.checkOutStatus)
                    {
                        case BookingResponse.SUCCESS : errMsg = null; break;

                        case BookingResponse.RECOVERABLE_FIX_CREDIT_CARD : 
                            errMsg = Properties.CheckoutLabel_Message_CheckCreditCardData; break;

                        case BookingResponse.RECOVERABLE_NEW_CREDIT_CARD :
                        case BookingResponse.RECOVERABLE_NEW_CREDIT_CARD_LOW_FOUNDS :
                            errMsg = Properties.CheckoutLabel_Message_ChangeCreditCard; break;

                        case BookingResponse.NO_RECOVERABLE_NEW_BOOKING :
                        case BookingResponse.NO_RECOVERABLE_NEW_BOOKING_EXPIRED :
                        case BookingResponse.NO_RECOVERABLE_NEW_BOOKING_NEW_PROVIDER :
                        case BookingResponse.NO_RECOVERABLE_CREDIT_CARD_ERROR :
                        case BookingResponse.NO_RECOVERABLE_CREDIT_CARD_CANCEL_ERROR :
                        case BookingResponse.NO_RECOVERABLE_RISK_REJECTED :
                        case BookingResponse.NO_RECOVERABLE_BANK_SLIP_ERROR :
                            errMsg = Properties.CheckoutLabel_Message_NoRecoverableError; break;
                    
                        case BookingResponse.C_NO_RECOVERABLE_CONSUME_COUPON_ERROR :
                            errMsg = Properties.CheckoutLabel_Message_CouponNoRecoverable; break;

                        case BookingResponse.RISK_QUESTIONS :
                            errMsg = Properties.CheckoutLabel_Message_AdditionalDataNeeded; break;

                        default: errMsg = "Unknown error"; break;
                    }
                }

                else errMsg = "Unknown error";

                if (string.IsNullOrEmpty(errMsg))
                {
                    LastHotelBookData.AvailabilityModel = AvailabilityModel;
                    LastHotelBookData.AvailabilityInfo = AvailabilityInfo;
                    LastHotelBookData.PassengerDefinitions = PassengerDefinitions;
                    LastHotelBookData.VoucherDefinitions = VoucherDefinitions;
                    LastHotelBookData.CardDefinition = CardDefinition;
                    Logger.Info("[vm:hotel:checkout] Booking completed");
                }
            }

            Loading = "Collapsed";
            NotifyPropertyChanged("Loading");
            return errMsg;
        }


        
    }
}
