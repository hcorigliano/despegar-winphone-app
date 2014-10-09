
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Repository;
using Despegar.LegacyCore.Service;
using Despegar.LegacyCore.Connector.Domain.API;
using System.Text.RegularExpressions;

namespace Despegar.LegacyCore.Model
{
    public class ValidationCreditcardsModel
    {

        public ValidationCreditcardsModel()
        {
            Logger.Info("[model:creditcards:validations] Credit card validations Model created");
        }


        public async Task Sync()
        {
            if (ValidationCreditcardsRep.All == null)
                ValidationCreditcardsRep.All = await APIValidationCreditcards.GetAll();

            else
                Logger.Info("[model:creditcards:validations] getting all from repository");
        }

        public async Task<ValidationCreditcards> GetAll()
        {
            await this.Sync();
            return ValidationCreditcardsRep.All;
        }


        public bool ValidateNumber(HotelCreditCard card, String value)
        {
            bool err = false;
            ValidationCreditcard validation = Validation(card);

            Regex lengthRegex = new Regex(validation.lengthRegex);
            Regex numberRegex = new Regex(validation.numberRegex);
            err = !lengthRegex.IsMatch(value) || !numberRegex.IsMatch(value);

            return err;
        }


        public bool ValidateCode(HotelCreditCard card, String value)
        {
            ValidationCreditcard validation = Validation(card);
            Regex codeRegex = new Regex(validation.codeRegex);

            return !codeRegex.IsMatch(value);
        }


        private ValidationCreditcard Validation(HotelCreditCard card)
        {
            String[] parts = card.cardCode.Split('_');

            string bankCode = "*";
            string cardCode = "*";
            string cardType = "CREDIT";
            if (parts.Length > 1) bankCode = parts[1];
            if (parts.Length > 0) cardCode = parts[0];

            ValidationCreditcard validation  = default(ValidationCreditcard);
            ValidationCreditcard genericBank = default(ValidationCreditcard);
            ValidationCreditcard genericType = default(ValidationCreditcard);
            ValidationCreditcard genericTypeAndbank = default(ValidationCreditcard);
            ValidationCreditcard generic = default(ValidationCreditcard);

            if (ValidationCreditcardsRep.All != null)
            {
                foreach (var it in ValidationCreditcardsRep.All.data)
                {
                    if (it.bankCode == bankCode &&
                        it.cardCode == cardCode &&
                        it.cardType == cardType)
                        validation = it;

                    if (it.bankCode == "*" &&
                        it.cardCode == cardCode &&
                        it.cardType == "CREDIT")
                        genericBank = it;

                    if (it.bankCode == bankCode &&
                        it.cardCode == cardCode &&
                        it.cardType == "*")
                        genericType = it;

                    if (it.bankCode == "*" &&
                        it.cardCode == cardCode &&
                        it.cardType == "*")
                        genericTypeAndbank = it;

                    if (it.bankCode == "*" &&
                        it.cardCode == "*" &&
                        it.cardType == "*")
                        generic = it;
                }                
            }

            if (validation == null)
            {
                if (genericBank != null)
                    validation = genericBank;

                else if (genericType != null)
                    validation = genericType;

                else if (genericTypeAndbank != null)
                    validation = genericTypeAndbank;

                else validation = generic;
            }

            return validation;
        }
    }
}
