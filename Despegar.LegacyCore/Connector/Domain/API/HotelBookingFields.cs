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
    // Abstarct class definition
    public abstract class AbstractDefinition : INotifyPropertyChanged
    {
        public void NotifyPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }


    public class HotelBookingFields : BaseResponse
    {
        public HotelBookingFieldsData data { get; set; }
    }

    public class HotelBookingFieldsData
    {
        // API Properties

        public string ticket { get; set; }
        public Dictionary<string, RoomPackIdsInputDefinition> roomPackIdsInputDefinitionMap { get; set; }


        // View Model propeties

        public string SelectedRoom { get; set; }
        public HotelInputDefinition InputDefinition { get { return roomPackIdsInputDefinitionMap[SelectedRoom].inputDefinition; } }

        public string Serialize()
        {
            string serialized = "{";
            serialized += "\"ticket\" : \"" + ticket + "\",";
            serialized += "\"roomPackIdsInputDefinitionMap\" : "+ roomPackIdsInputDefinitionMap[SelectedRoom].Serialize();
            serialized += "}";
            return serialized;
        }
    }

    public class RoomPackIdsInputDefinition
    {
        public RoomPackKey roomPackKey { get; set; }
        public HotelInputDefinition inputDefinition { get; set; }
        public string roomPackId { get; set; }

        public string Serialize()
        {
            return "{ \"inputDefinition\" : "+ inputDefinition.Serialize() +" }";
        }
    }

    public class RoomPackKey
    {
        public string roomPackId { get; set; }
        public string paymentForm { get; set; }
    }


    // --------------------
    // Hotel definitions
    // --------------------

    public class HotelInputDefinition
    {
        // API properties

        public List<HotelPassengerDefinition> passengerDefinitions { get; set; }
        public HotelPaymentDefinition paymentDefinition { get; set; }
        public HotelContactDefinition contactDefinition { get; set; }
        public List<HotelVoucherDefinition> voucherDefinitions { get; set; }

        // View Model properties

        public string Serialize()
        {
            List<string> passDef = new List<string>();
            List<string> vouchDef = new List<string>();

            foreach (var it in passengerDefinitions)
            {
                passDef.Add(it.Serialize());
            }            

            if (voucherDefinitions != null)
            {
                foreach (var it in voucherDefinitions)
                {
                   if (it.Active) 
                      vouchDef.Add("{ \"value\" : \"" + it.Value + "\" }");
                }
            }

            string serialized = "{";
            serialized += "\"passengerDefinitions\" : [ " + String.Join(",", passDef) + " ],";
            if (paymentDefinition != null)
                serialized += "\"paymentDefinition\" : "+paymentDefinition.Serialize()+",";
            if (vouchDef.Count > 0) serialized += "\"voucherDefinitions\" : [" + String.Join(",", vouchDef) + "],";
            serialized += "\"contactDefinition\" : "+ contactDefinition.Serialize();
            serialized += "}";
            return serialized;
        }

        public void SetPassengerIndexes()
        {
            if (passengerDefinitions.Count == 0)
                passengerDefinitions[0].Index = "";
            
            else
                for (int i = 0; i < passengerDefinitions.Count; i++)
                    passengerDefinitions[i].Index = (i+1).ToString();
        }
    }

    public class HotelPassengerDefinition
    {
        // API properties
        
        public TextField firstName { get; set; }
        public TextField lastName { get; set; }


        // View Model properties
        
        public string Index { get; set; }

        public List<CustomValidation> Validations { get; set; }

        
        // Custom validations

        public bool Validate()
        {
            firstName.Validate();
            lastName.Validate();
            return firstName.Error || lastName.Error;
        }

        public HotelPassengerDefinition()
        {
            Validations = new List<CustomValidation>();

            // validate name length + last length
            Validations.Add(new CustomValidation() {
                Execute = () => {
                    int maxLen = 10;
                    int len = firstName.Value.Count() + lastName.Value.Count();
                    return len > maxLen;
                }
            });
        }

        public string Serialize()
        {
            string serialized = "{";
            serialized += "\"firstName\": { \"value\" : \""+ firstName.Value +"\" },";
            serialized += "\"lastName\": { \"value\" : \"" + lastName.Value  + "\" }";
            serialized += "}";
            return serialized;
        }
    }
    
    public class HotelContactDefinition
    {
        // API properties

        public EmailTextField email { get; set; }
        public List<HotelPhoneDefinition> phoneDefinitions { get; set; }


        // View Model properties

        public HotelPhoneDefinition PhoneDefinition { get { return phoneDefinitions[0]; } }

        public bool Validate()
        {
            email.Validate();
            bool phoneErr = false;
            foreach (HotelPhoneDefinition it in phoneDefinitions)
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

    public class HotelPhoneDefinition
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

    public class HotelPaymentDefinition
    {
        public HotelCardDefinition cardDefinition { get; set; }
        public HotelInvoiceDefinition InvoiceDefinition { get; set; }

        public string Serialize()
        {
            string serialized = "{";
            serialized += "\"cardDefinition\": " + cardDefinition.Serialize() + ",";

            if (InvoiceDefinition != null)
            {
                serialized += "\"invoiceDefinition\": { \"value\" : " + InvoiceDefinition.Serialize() + " }";
            }
            serialized += "}";
            return serialized;
            
        }
    }

    public class HotelCardDefinition : AbstractDefinition
    {
        // API filled properties:

        public TextField number { get; set; }
        public DateYearMonthField expiration { get; set; }
        public TextField securityCode { get; set; }
        public TextField ownerName { get; set; }
        public MultivalueField ownerGender { get; set; }
        public HotelDocumentDefinition ownerDocumentDefinition { get; set; }

        public TextField bankCode { get; set; }
        public TextField cardCode { get; set; }
        public TextField cardType { get; set; }


        // View Model Properties

        public HotelCreditCard Value { get { return value; } set { this.value = value; NotifyPropertyChanged("Value"); } }
        public HotelCreditCard value;
        public ObservableCollection<HotelCreditCard> Cards { get; set; }
        
        public HotelCreditCard Selected 
        { 
            get 
            {
                if (selected != null) return selected;
                else if (Cards != null && Cards.Count != 0) return Cards[0];
                else return selected;
            } set { selected = value; } }
        
        public HotelCreditCard selected;

        public bool Validate()
        {
            bool ownerGenderErr = false;
            bool ownerDocumentDefinitionErr= false;

            number.Validate();
            expiration.Validate();
            securityCode.Validate();
            ownerName.Validate();
            if (ownerGender != null) 
            {
                ownerGender.Validate();
                ownerGenderErr = ownerGender.Error;
            }

            if (ownerDocumentDefinition != null) 
            {
                ownerDocumentDefinitionErr = ownerDocumentDefinition.Validate();
            }

            return ownerDocumentDefinitionErr || number.Error || expiration.Error || securityCode.Error || ownerName.Error || ownerGenderErr;
        }

        public string Serialize()
        {
            String[] parts = Selected.cardCode.Split('_');
            string bankCode = "";
            string cardCode = "";
            string cardType = "CREDIT";
            if (parts.Length > 0) cardCode = parts[0];
            if (parts.Length > 1) bankCode = parts[1];

            string serialized = "{";
            serialized += "\"number\": { \"value\" : \"" + number.Value + "\" },";
            serialized += "\"expiration\": { \"value\" : \"" + expiration.Value + "\" },";
            serialized += "\"securityCode\": { \"value\" : \"" + securityCode.Value + "\" },";

            serialized += "\"bankCode\": { \"value\" : \"" + bankCode + "\" },";
            serialized += "\"cardCode\": { \"value\" : \"" + cardCode + "\" },";
            serialized += "\"cardType\": { \"value\" : \"" + cardType + "\" },";

            if (ownerGender != null) serialized += "\"ownerGender\": { \"value\" : \"" + ownerGender.Value + "\" },";
            if (ownerDocumentDefinition != null) serialized += "\"ownerDocumentDefinition\" : " + ownerDocumentDefinition.Serialize() + ",";
            serialized += "\"ownerName\": { \"value\" : \"" + ownerName.Value + "\" }";
            serialized += "}";
            return serialized;
        }
    }


    public class HotelInvoiceDefinition : AbstractDefinition
    {
        // API filled properties:

        public MultivalueField taxStatus { get; set; }
        public TextField invoiceName { get; set; }
        public TextField fiscalDocument { get; set; }
        public HotelBillingAddress billingAddress { get; set; }

        /*// View Model Properties

        public HotelCreditCard Value { get { return value; } set { this.value = value; NotifyPropertyChanged("Value"); } }
        public HotelCreditCard value;
        public ObservableCollection<HotelCreditCard> Cards { get; set; }

        public HotelCreditCard Selected
        {
            get
            {
                if (selected != null) return selected;
                else if (Cards != null) return Cards[0];
                else return selected;
            }
            set { selected = value; }
        }

        public HotelCreditCard selected;
        */
        public bool Validate()
        {
            //Validation depends of TaxStatus.
            
            bool invoiceNameErr = false;
            bool fiscalDocumentErr = false;
            bool billingAddressErr = false;

            fiscalDocument.Validate();
            billingAddressErr = billingAddress.Validate();
            if (taxStatus.Value != "FINAL_CONSUMER")
            {
                if (invoiceName != null)
                {
                    invoiceName.Validate();
                    invoiceNameErr = invoiceName.Error;
                }
            }         

                  

            return   invoiceNameErr || fiscalDocumentErr || billingAddressErr;
        }

        public string Serialize()
        {
            string serialized = "{";
            if (taxStatus != null) serialized += "\"taxStatus\": { \"value\" : \"" + taxStatus.Value + "\" },";
            if (fiscalDocument != null) serialized += "\"fiscalDocument\": { \"value\" : \"" + fiscalDocument.Value + "\" },";
            if (invoiceName != null) serialized += "\"invoiceName\" : { \"value\" : \"" + invoiceName.Value + "\" },";           
            if (billingAddress != null) serialized += "\"billingAddress\" : " + billingAddress.Serialize() ;            
            serialized += "}";
            return serialized;
        }
    }

    public class HotelBillingAddress
    {
        public TextField stateId { get; set; }
        public TextField cityId { get; set; }
        public TextField postalCode { get; set; }
        public TextField street { get; set; }
        public TextField number { get; set; }
        public TextField floor { get; set; }
        public TextField department { get; set; }

        public bool Validate()
        {
            
            stateId.Validate();
            cityId.Validate();
            postalCode.Validate();
            street.Validate();
            number.Validate();
            return stateId.Error || cityId.Error || number.Error || postalCode.Error || street.Error;
        }

        public string Serialize()
        {
            string serialized = "{";
            serialized += "\"stateId\": { \"value\" : \"" + stateId.Value + "\" },";
            serialized += "\"cityId\": { \"value\" : \"" + cityId.Value + "\" },";
            serialized += "\"postalCode\": { \"value\" : \"" + postalCode.Value + "\" },";
            serialized += "\"street\": { \"value\" : \"" + street.Value + "\" },";
            serialized += "\"number\": { \"value\" : \"" + number.Value + "\" },";
            serialized += "\"floor\": { \"value\" : \"" + floor.Value + "\" },";
            serialized += "\"department\": { \"value\" : \"" + department.Value + "\" }";
            serialized += "}";
            return serialized;
        }
    }


    public class HotelDocumentDefinition
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
            serialized += "\"number\": { \"value\" : \""+ number.Value +"\" }";
            serialized += "}";
            return serialized;
        }
    }

    public class HotelVoucherDefinition : TextField 
    {
        public bool Active { get { return active; } set { active = value; NotifyPropertyChanged("Active"); } }
        public bool active;
    }

}
