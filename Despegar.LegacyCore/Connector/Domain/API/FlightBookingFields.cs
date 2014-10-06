using Despegar.LegacyCore.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{

    public class FlightBookingFields : BaseResponse
    {
        public FlightBookingFieldsData data { get; set; }
    }

    public class FlightBookingFieldsData
    {
        // API Properties

        public string ticket { get; set; }
        public FlightInputDefinition flightInputDefinition { get; set; }

        public string Serialize()
        {
            string serialized = "{";
            serialized += "\"ticket\" : \"" + ticket + "\",";
            serialized += "\"flightInputDefinition\" : " + flightInputDefinition.Serialize();
            serialized += "}";
            return serialized;
        }
    }

    public class FlightInputDefinition
    {
        // API properties
        public List<FlightPassengerDefinition> passengerDefinitions { get; set; }
        public FlightPaymentDefinition paymentDefinition { get; set; }
        public FlightContactDefinition contactDefinition { get; set; }
        public FlightInvoiceDefinition invoiceDefinition { get; set; }

        //public List<FlightVoucherDefinition> voucherDefinitions { get; set; }

        // View Model properties
        public string Serialize()
        {
            List<string> passDef = new List<string>();

            foreach (var it in passengerDefinitions)
            {
                passDef.Add(it.Serialize()); 
            }
            List<string> vouchDef = new List<string>();
            //voucherDefinitions.ForEach(it => { if (it.Active) vouchDef.Add("{ \"value\" : \"" + it.Value + "\" }"); });

            string serialized = "{";

            serialized += "\"passengerDefinitions\" : [ " + String.Join(",", passDef) + " ],";
            serialized += "\"paymentDefinition\" : " + paymentDefinition.Serialize() + ",";
            //if (vouchDef.Count > 0) serialized += "\"voucherDefinitions\" : [" + String.Join(",", vouchDef) + "],";
            serialized += "\"contactDefinition\" : " + contactDefinition.Serialize();

            if (invoiceDefinition != null)
              serialized += ", \"invoiceDefinition\" : " + invoiceDefinition.Serialize();

            serialized += "}";
            return serialized;
        }

        // TODO: what is this for
        public void SetPassengerIndexes()
        {
            if (passengerDefinitions.Count == 0)
                passengerDefinitions[0].Index = "";

            else
                for (int i = 0; i < passengerDefinitions.Count; i++)
                    passengerDefinitions[i].Index = (i + 1).ToString();
        }
    }

    public class FlightPassengerDefinition
    {
        // API properties

        public String type { get; set; }

        public TextField firstName { get; set; }
        public TextField lastName { get; set; }
        public MultivalueField gender { get; set; }
        public FlightDocumentDefinition documentDefinition { get; set; }
        public NationalityField nationality { get; set; }
        public BirthdayField birthday { get; set; }


        // View Model properties

        public string Index { get; set; }
        public String Type { get { return "TYPE_" + type; } }

        public List<CustomValidation> Validations { get; set; }

        public bool Validate()
        {
            bool nationalityErr = false;
            bool birthdayErr = false;
            bool genderErr = false;
            bool documentErr = false;

            firstName.Validate();
            lastName.Validate();
            if (gender != null)
            {
                gender.Validate();
                genderErr = gender.Error;
            }
            if (nationality != null)
            {
                nationality.Validate();
                nationalityErr = nationality.Error;
            }
            if (birthday != null)
            {
                birthday.Validate();
                birthdayErr = birthday.Error;
            }

            if (documentDefinition != null)
            {
                documentErr = documentDefinition.Validate();
            }

            return documentErr || firstName.Error || lastName.Error || genderErr || nationalityErr || birthdayErr;
        }

        public string Serialize()
        {
            string serialized = "{";
            if (nationality != null) serialized += "\"nationality\": { \"value\" : \"" + nationality.Value + "\" },";
            if (documentDefinition != null) serialized += "\"documentDefinition\":" + documentDefinition.Serialize() + ",";
            if (birthday != null) serialized += "\"birthday\": { \"value\" : \"" + birthday.Value + "\" },";
            if (gender != null) serialized += "\"gender\": { \"value\" : \"" + gender.Value + "\" },";
            serialized += "\"firstName\": { \"value\" : \"" + firstName.Value + "\" },";
            serialized += "\"lastName\": { \"value\" : \"" + lastName.Value + "\" }";
            serialized += "}";
            return serialized;
        }
    }

    public class NationalityField : TextField
    {
        public List<GeoCountry> options { get { return CountriesRep.All.countries; } }

        public GeoCountry Selected
        {
            get 
            {
                if (selected != null) return selected;
                else if (!string.IsNullOrEmpty(value)) return options.First(it => { return value == it.id; });
                else if (options.Count > 0) return options[0];
                else return selected;
            }
            set { selected = value; NotifyPropertyChanged("Selected"); NotifyPropertyChanged("Value"); }
        }
        public GeoCountry selected;

        public new string Value { get { return Selected.id; } }
    }

    public class BirthdayField : TextField
    {
        public long from { get; set; }
        public long to { get; set; }


        public new DateTime value
        {
            get 
            {
                if (val != null && val.Year != 1) return val;
                else if (To != null && To.Year != 1) return To;
                else return val;
            }
            set
            {
                val = value;
            }
        }
        public DateTime val;

        public override string Value
        {
            get
            {
                return value.ToString("yyyy-MM-dd");
            }
        }

        public DateTime From
        {
            get
            {
                DateTime epochFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                epochFrom = epochFrom.AddSeconds(from / 1000).ToLocalTime();
                return epochFrom;
            }
        }

        public DateTime To
        {
            get
            {
                DateTime epochTo = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                epochTo = epochTo.AddSeconds(to / 1000).ToLocalTime();
                return epochTo;
            }
        }


        public new bool Validate()
        {
            error = false;
            int fromErr = DateTime.Compare(From, value);
            int toErr = DateTime.Compare(value, To);
            error = !(fromErr <= 0 && toErr <= 0);
            NotifyPropertyChanged("Error");
            return error;
        }
    }
    
    public class FlightDocumentDefinition
    {
        public MultivalueField type { get; set; }
        public TextField number { get; set; }

        public bool Validate()
        {
            type.Validate();
            number.Validate();

            return type.Error || number.Error;
        }

        public string Serialize()
        {
            string serialized = "{";
            serialized += "\"type\": { \"value\" : \"" + type.Value + "\" },";
            serialized += "\"number\": { \"value\" : \"" + number.Value + "\" }";
            serialized += "}";
            return serialized;
        }
    }

    public class FlightContactDefinition
    {
        // API properties

        public EmailTextField email { get; set; }
        public List<FlightPhoneDefinition> phoneDefinitions { get; set; }


        // View Model properties

        public FlightPhoneDefinition PhoneDefinition { get { return phoneDefinitions[0]; } }

        public bool Validate()
        {
            email.Validate();
            bool phoneErr = false;
            foreach (FlightPhoneDefinition it in phoneDefinitions)
            {
                phoneErr = it.Validate();
            }

            return email.Error || phoneErr;
        }

        public string Serialize()
        {
            List<string> phoneDef = new List<string>();

            foreach (var it in phoneDefinitions)
            {
                phoneDef.Add(it.Serialize());
            }

            string serialized = "{";
            serialized += "\"email\": { \"value\" : \"" + email.Value + "\" },";
            serialized += "\"phoneDefinitions\": [ " + String.Join(",", phoneDef) + "]";
            serialized += "}";
            return serialized;
        }
    }

    public class FlightInvoiceDefinition 
    {
        public TextField cardHolderName { get; set; }        
        public MultivalueField fiscalStatus { get; set; }
        public TextField fiscalId { get; set; }
        public FlighInvoiceAddressDefinition addressDefinition { get; set; }
        public TextField address { get; set; }

        //public bool ShowFiscalAddressFields { get { return fiscalStatus.Value.ToUpper() != "FINAL";  } }

        public bool Validate()
        {            
            address.Validate();
            fiscalId.Validate();
            if (fiscalStatus.Value != "FINAL")
            {
                cardHolderName.Validate();
            }
            fiscalStatus.Validate();
            bool addressDefinitionErr = addressDefinition.Validate();


            return address.Error || fiscalId.Error || cardHolderName.Error | fiscalStatus.Error  || addressDefinitionErr;
        }

        public string Serialize()
        {
            string serialized = "{";
            serialized += "\"fiscalStatus\": { \"value\" : \"" + fiscalStatus.Value + "\" },";
            serialized += "\"fiscalId\": { \"value\" : \"" + fiscalId.Value + "\" },";

            serialized += "\"cardHolderName\": { \"value\" : \"" + cardHolderName.Value + "\" },";
            serialized += "\"addressDefinition\" : " + addressDefinition.Serialize();
            serialized += ",";

            serialized += "\"address\": { \"value\" : \"" + address.Value + "\" }";

            serialized += "}";
            return serialized;
        }        
    }

    public class FlightPhoneDefinition 
    {
        public MultivalueField type { get; set; }
        public TextField countryCode { get; set; }
        public TextField areaCode { get; set; }
        public TextField number { get; set; }

        public bool Validate()
        {
            countryCode.Validate();
            areaCode.Validate();
            number.Validate();
            return countryCode.Error || areaCode.Error || number.Error;
        }

        public string Serialize()
        {
            string serialized = "{";
            serialized += "\"type\": { \"value\" : \"" + type.Value + "\" },";
            serialized += "\"countryCode\": { \"value\" : \"" + countryCode.Value + "\" },";
            serialized += "\"areaCode\": { \"value\" : \"" + areaCode.Value + "\" },";
            serialized += "\"number\": { \"value\" : \"" + number.Value + "\" }";
            serialized += "}";
            return serialized;
        }
    }

    public class FlightPaymentDefinition
    {
        public FlightInstallmentDefinition installmentDefinition { get; set; }
        public FlightCardDefinition cardDefinition { get; set; }

        public FlightBillingAddressDefinition billingAddressDefinition { get; set; }

        public string Serialize()
        {
            string serialized = "{";
            serialized += "\"installmentDefinition\": " + installmentDefinition.Serialize() + " ,";
            serialized += "\"cardDefinition\": " + cardDefinition.Serialize();
            serialized += "}";
            return serialized;
        }
    }

    public class FlightCardDefinition : AbstractDefinition
    {
        // API filled properties:

        public TextField number { get; set; }
        public DateYearMonthField expiration { get; set; }
        public TextField securityCode { get; set; }
        public TextField ownerName { get; set; }
        public MultivalueField ownerGender { get; set; }
        public TextField bank { get; set; }
        public HotelDocumentDefinition ownerDocumentDefinition { get; set; }


        // View Model Properties

        public bool Validate()
        {
            bool genderErr = false;
            bool documentErr = false;
            bool bankErr = false;

            number.Validate();
            expiration.Validate();
            securityCode.Validate();
            ownerName.Validate();
            
            if (ownerGender != null)
            {
                ownerGender.Validate();
                genderErr = ownerGender.Error;
            }

            if (ownerDocumentDefinition != null)
            {
                documentErr = ownerDocumentDefinition.Validate();
            }

            if (bank != null)
            {
                bankErr = bank.Error;
            }
            return documentErr || number.Error || expiration.Error || securityCode.Error || ownerName.Error || genderErr || bankErr;
        }

        public string Serialize()
        {
            string serialized = "{";
            serialized += "\"expiration\": { \"value\" : \"" + expiration.Value + "\" },";
            serialized += "\"securityCode\": { \"value\" : \"" + securityCode.Value + "\" },";
            serialized += "\"ownerName\": { \"value\" : \"" + ownerName.Value + "\" },";
            if (ownerGender != null) serialized += "\"ownerGender\": { \"value\" : \"" + ownerGender.Value + "\" },";
            if (bank != null) serialized += "\"bank\": { \"value\" : \"" + bank.Value + "\" },";
            if (ownerDocumentDefinition != null) serialized += "\"ownerDocumentDefinition\" : " + ownerDocumentDefinition.Serialize() + ",";
            serialized += "\"number\": { \"value\" : \"" + number.Value + "\" }";
            serialized += "}";
            return serialized;
        }
    }

    public class FlightInstallmentDefinition : AbstractDefinition
    {
        public TextField quantity { get; set; }
        public TextField completeCardCode { get; set; }
        public TextField cardType { get; set; }
        public TextField bankCode { get; set; }
        public TextField cardCode { get; set; }


        public FlightPayment Value { get { return value; } set { this.value = value; NotifyPropertyChanged("Value"); } }
        public FlightPayment value;
        public ObservableCollection<FlightPayment> Cards { get; set; }

        public FlightPayment Selected
        {
            get
            {
                if (selected != null) return selected;
                else if (Cards != null) return Cards[0];
                else return selected;
            }
            set { selected = value; }
        }

        public FlightPayment selected;


        public string Serialize()
        {
            string completeCardCode = Selected.cardCode;
            String[] parts = completeCardCode.Split('_');
            string bankCode = "";
            string cardCode = "";
            string cardType = "CREDIT";
            if (parts.Length > 0) cardCode = parts[0];
            if (parts.Length > 1) bankCode = parts[1];

            string serialized = "{";
            serialized += "\"quantity\": { \"value\" : \"" + Selected.installments.quantity + "\" },";
            serialized += "\"completeCardCode\": { \"value\" : \"" + completeCardCode + "\" },";
            serialized += "\"bankCode\": { \"value\" : \"" + bankCode + "\" },";
            serialized += "\"cardCode\": { \"value\" : \"" + cardCode + "\" },";
            serialized += "\"cardType\": { \"value\" : \"" + cardType + "\" }";
            serialized += "}";
            return serialized;
        }
    }

    public class FlightBillingAddressDefinition : AbstractDefinition
    {
        public MultivalueField country { get; set; }
        public TextField state { get; set; }
        public TextField postalCode { get; set; }
        
        public TextField street { get; set; }
        public TextField number { get; set; }
        public TextField floor { get; set; }
        public TextField department { get; set; }

        // View Model Properties
        public bool Validate()
        {
            bool countryErr = false;
            bool stateErr = false;
            bool postalCodeErr = false;
            bool streetErr = false;
            bool numberErr = false;
            bool floorErr = false;
            bool departmentErr = false;

            if (country != null) 
            {
                country.Validate();
                countryErr = country.Error;
            }

            if (state != null)
            {
                state.Validate();
                stateErr = state.Error;
            }

            if (postalCode != null)
            {
                postalCode.Validate();
                postalCodeErr = postalCode.Error;
            }
            if (street != null)
            {
                street.Validate();
                streetErr = street.Error;
            }

            if (number != null)
            {
                number.Validate();
                numberErr = number.Error;
            }

            if (floor != null)
            {
                floor.Validate();
                floorErr = floor.Error;
            }

            if (department != null)
            {
                department.Validate();
                departmentErr = department.Error;
            }
            return countryErr || stateErr || postalCodeErr || streetErr || numberErr;
        }

        public string Serialize()
        {
            string serialized = "{";
            if (country != null) serialized += "\"country\": { \"value\" : \"" + country.Value + "\" },";
            if (state != null) serialized += "\"state\": { \"value\" : \"" + state.Value + "\" },";
            if (postalCode != null) serialized += "\"postalCode\": { \"value\" : \"" + postalCode.Value + "\" },";
            if (street != null) serialized += "\"street\": { \"value\" : \"" + street.Value + "\" },";
            if (floor != null) serialized += "\"floor\": { \"value\" : \"" + floor.Value + "\" },";
            if (department != null) serialized += "\"department\": { \"value\" : \"" + department.Value + "\" },";
            serialized += "\"number\": { \"value\" : \"" + number.Value + "\" }";
            serialized += "}";
            return serialized;
        }
    }

    public class FlighInvoiceAddressDefinition : AbstractDefinition
    {
        public TextField country { get; set; } // TODO: Always be AR?
        public TextField state { get; set; }
        public TextField cityOid { get; set; }
        public TextField postalCode { get; set; }
        public TextField street { get; set; }
        public TextField number { get; set; }
        public TextField floor { get; set; }
        public TextField department { get; set; }

        // View Model Properties
        public bool Validate()
        {
            bool countryErr = false;
            bool stateErr = false;
            bool cityOidErr = false;
            bool postalCodeErr = false;
            bool streetErr = false;
            bool numberErr = false;
            bool floorErr = false;
            bool departmentErr = false;

            if (cityOid != null)
            {
                cityOid.Validate();
                cityOidErr = cityOid.Error; 
            }

            if (state != null)
            {
                state.Validate();
                stateErr = state.Error;
            }

            if (postalCode != null)
            {
                postalCode.Validate();
                postalCodeErr = postalCode.Error;
            }

            if (street != null)
            {
                street.Validate();
                streetErr = street.Error;
            }

            if (number != null)
            {
                number.Validate();
                numberErr = number.Error;
            }

            if (floor != null)
            {
                floor.Validate();
                floorErr = floor.Error;
            }

            if (department != null)
            {
                department.Validate();
                departmentErr = department.Error;
            }
            return countryErr || stateErr || postalCodeErr || streetErr || numberErr || cityOidErr;
        }

        public string Serialize()
        {
            string serialized = "{";
            if (country != null) serialized += "\"country\": { \"value\" : \"" + country.Value + "\" },";
            if (state != null) serialized += "\"state\": { \"value\" : \"" + state.Value + "\" },";
            if (cityOid != null) serialized += "\"cityOid\": { \"value\" : \"" + cityOid.Value + "\" },";
            if (postalCode != null) serialized += "\"postalCode\": { \"value\" : \"" + postalCode.Value + "\" },";
            if (street != null) serialized += "\"street\": { \"value\" : \"" + street.Value + "\" },";
            if (floor != null) serialized += "\"floor\": { \"value\" : \"" + floor.Value + "\" },";
            if (department != null) serialized += "\"department\": { \"value\" : \"" + department.Value + "\" },";
            serialized += "\"number\": { \"value\" : \"" + number.Value + "\" }";
            serialized += "}";

            return serialized;
        }
    }

    public class FlightVoucherDefinition : TextField
    {
        public bool Active { get { return active; } set { active = value; NotifyPropertyChanged("Active"); } }
        public bool active;
    }

}