using Despegar.LegacyCore.Connector.Domain.API;
using Despegar.LegacyCore.Model;
using Despegar.LegacyCore.Resource;
using Despegar.LegacyCore.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.ViewModel
{
    public class FlightsCheckoutViewModel : INotifyPropertyChanged
    {
        private FlightsBookingModel BookingModel { get; set; }
        private FlightBookingFields BookingFields { get; set; }
        private ValidationCreditcardsModel CreditCardsValidationModel { get; set; }
        public string Loading { get; set; }
        public FlightsAvailabilityModel AvailabilityModel { get; set; }
        public List<GeoCountry> Countries { get; set; }
        public FlightAvailabilityItem AvailabilityInfo { get; set; }
        public List<FlightPassengerDefinition> PassengerDefinitions { get; set; }
        public FlightCardDefinition CardDefinition { get; set; }
        public FlightInstallmentDefinition InstallmentDefinition { get; set; }
        //public List<FlightVoucherDefinition> VoucherDefinitions { get; set; }
        public FlightContactDefinition ContactDefinition { get; set; }
        public FlightInvoiceDefinition InvoiceDefinition { get; set; }
        public StatesModel StateModel { get; set; }
        private CitiesModel CityModel { get; set; }
        public void NotifyPropertyChanged(string propertyName) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler FieldsLoaded;

        public bool InvoiceDefinitionIsRequired { get { return InvoiceDefinition != null;} }

        public FlightsCheckoutViewModel()
        {
            Loading = "Visible";
            Logger.Info("[vm:flight:checkout] Flight Checkout ViewModel initialized");

            InitializeFlightCheckoutAsync();
        }
        
        public override string ToString()
        {
            return String.Format("{0}/{1}/{2}", AvailabilityModel.Ticket, AvailabilityModel.Itinerary, AvailabilityModel.Currency);
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

        public void Birthday_Value_Changed(object input)
        {
            BirthdayField picker = input as BirthdayField;
            picker.Validate();
        }

        public string CardNumber_Validation(object input, object creditCard)
        {
            TextField Input = input as TextField;
            Input.error = false;
            Input.Validate();
            string err = "";

            if (!Input.Error)
            {
                FlightInstallmentDefinition card = creditCard as FlightInstallmentDefinition;
                FlightPayment selected = card.Selected ?? card.Cards[0];
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
                FlightInstallmentDefinition card = creditCard as FlightInstallmentDefinition;
                FlightPayment selected = card.Selected ?? card.Cards[0];
                Input.error = CreditCardsValidationModel.ValidateCode(selected, Input.Value);
                Input.NotifyPropertyChanged("Error");
            }

            return Input.error;
        }

        public async Task<string> ValidateAndBuy()
        {
            // Validations
            string errMsg = String.Empty;

            foreach (var it in PassengerDefinitions)
                it.Validate();

            bool passErr = PassengerDefinitions.Any(it => { return it.Validate(); });
            bool contErr = ContactDefinition.Validate();
            bool cardErr = CardDefinition.Validate();
            bool fiscalDataErr = InvoiceDefinitionIsRequired ? InvoiceDefinition.Validate() : false;
      
            // other errors
            bool err = passErr || contErr || cardErr || fiscalDataErr;
            errMsg += Properties.CheckoutLabel_Message_CheckItPlease;
            errMsg += passErr ? Properties.CheckoutLabel_Message_CheckPassengers : "";
            errMsg += contErr ? Properties.CheckoutLabel_Message_CheckContact : "";
            errMsg += cardErr ? Properties.CheckoutLabel_Message_CheckCardData : "";
            //errMsg += fiscalDataErr ? Properties.CheckoutLabel_Message_CheckFiscalData : "";
            errMsg += Properties.CheckoutLabel_Message_BeCorrect;

            if (!err)
            {
                Loading = "Visible";
                NotifyPropertyChanged("Loading");

                // Buy Flight
                LastFlightBookData.LastBookResponse = await BookingModel.Buy(BookingFields);

                if (LastFlightBookData.LastBookResponse.data != null)
                {
                    switch (LastFlightBookData.LastBookResponse.data.checkOutStatus)
                    {
                        case BookingResponse.SUCCESS: errMsg = null; break;

                        case BookingResponse.RECOVERABLE_FIX_CREDIT_CARD:
                            errMsg = Properties.CheckoutLabel_Message_CheckCreditCardData; break;

                        case BookingResponse.RECOVERABLE_NEW_CREDIT_CARD:
                        case BookingResponse.RECOVERABLE_NEW_CREDIT_CARD_LOW_FOUNDS:
                            errMsg = Properties.CheckoutLabel_Message_ChangeCreditCard; break;

                        case BookingResponse.NO_RECOVERABLE_NEW_BOOKING:
                        case BookingResponse.NO_RECOVERABLE_NEW_BOOKING_EXPIRED:
                        case BookingResponse.NO_RECOVERABLE_NEW_BOOKING_NEW_PROVIDER:
                        case BookingResponse.NO_RECOVERABLE_CREDIT_CARD_ERROR:
                        case BookingResponse.NO_RECOVERABLE_CREDIT_CARD_CANCEL_ERROR:
                        case BookingResponse.NO_RECOVERABLE_RISK_REJECTED:
                        case BookingResponse.NO_RECOVERABLE_BANK_SLIP_ERROR:
                            errMsg = Properties.CheckoutLabel_Message_NoRecoverableError; break;

                        case BookingResponse.C_NO_RECOVERABLE_CONSUME_COUPON_ERROR:
                            errMsg = Properties.CheckoutLabel_Message_CouponNoRecoverable; break;

                        case BookingResponse.RISK_QUESTIONS:
                            errMsg = Properties.CheckoutLabel_Message_AdditionalDataNeeded; break;
                        default: errMsg = "Unknown error"; break;
                    }
                }

                else errMsg = "Unknown error";

                if (string.IsNullOrEmpty(errMsg))
                {
                    LastFlightBookData.AvailabilityModel = AvailabilityModel;
                    LastFlightBookData.AvailabilityInfo = AvailabilityInfo;
                    LastFlightBookData.PassengerDefinitions = PassengerDefinitions;
                    LastFlightBookData.CardDefinition = CardDefinition;
                    Logger.Info("[vm:flight:checkout] Booking completed");
                }
            }

            Loading = "Collapsed";
            NotifyPropertyChanged("Loading");
            return errMsg;
        }
     
        private async void InitializeFlightCheckoutAsync()
        {
            // Get Booking Fields
            BookingModel = new FlightsBookingModel();
            AvailabilityModel = new FlightsAvailabilityModel();
            CreditCardsValidationModel = new ValidationCreditcardsModel();
            CountriesModel CountriesModel = new CountriesModel();
            StateModel = new StatesModel();
            CityModel = new CitiesModel();
            AvailabilityModel.SetParamsByUrl(ApplicationConfig.Instance.BrowsingPages.Peek());
            FlightAvailability Availability = await AvailabilityModel.GetAvailability();

            BookingFields = await BookingModel.GetBookingFields(AvailabilityModel.Ticket, AvailabilityModel.Itinerary, ApplicationConfig.Instance.DeviceDescription);

            // Prepare view model:
            AvailabilityInfo = Availability.Item;
            AvailabilityInfo.priceInfo.Currency = AvailabilityModel.Currency;
            AvailabilityInfo.SetRoutesTypesAndSegmentsIndex();
            BookingFields.data.flightInputDefinition.SetPassengerIndexes();

            Loading = "Collapsed";
            PassengerDefinitions = BookingFields.data.flightInputDefinition.passengerDefinitions;
            CardDefinition = BookingFields.data.flightInputDefinition.paymentDefinition.cardDefinition;
            InstallmentDefinition = BookingFields.data.flightInputDefinition.paymentDefinition.installmentDefinition;
            //VoucherDefinitions = BookingFields.data.inputDefinition.voucherDefinitions;
            ContactDefinition = BookingFields.data.flightInputDefinition.contactDefinition;
            InvoiceDefinition = BookingFields.data.flightInputDefinition.invoiceDefinition;

            NotifyPropertyChanged("Loading");
            NotifyPropertyChanged("PassengerDefinitions");
            NotifyPropertyChanged("ContactDefinition");
            NotifyPropertyChanged("CardDefinition");
            NotifyPropertyChanged("InstallmentDefinition");
            //NotifyPropertyChanged("VoucherDefinition");
            NotifyPropertyChanged("AvailabilityInfo");
            NotifyPropertyChanged("AvailabilityModel");
            NotifyPropertyChanged("InvoiceDefinition");

            GeoCountries CountriesResp = await CountriesModel.GetAll();
            Countries = CountriesResp.countries;

            await CreditCardsValidationModel.Sync();
            // Loading booking fields finished
            FieldsLoaded(this, new EventArgs());
        }

        public async Task<IEnumerable<State>> GetAllStatesAsync()
        {
            return await StateModel.GetAll();
        }

        public async Task<List<City>> GetStringCityAsync(string searchString, int idState)
        {
            return await CityModel.GetAll(searchString, idState);
        }
    }
}