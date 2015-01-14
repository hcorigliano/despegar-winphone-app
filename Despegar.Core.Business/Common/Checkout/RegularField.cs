using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Common.Checkout
{
    public class RegularField : Bindable
    {
        // MAPI fields
        public string data_type { get; set; }
        public bool required { get; set; }
        public string value { get; set; }
        public List<Validation> validations { get; set; }

        // Custom
        private string currentError;
        private string coreValue;
        public string CoreValue 
        {
            get { return coreValue; }
            set
            {
                if (coreValue != value)
                {
                    coreValue = value;
                    Validate();
                    OnPropertyChanged();
                }
            }
        }
        public List<string> Errors { get; set; }
        public string CurrentError { get { return currentError; } set { currentError = value; OnPropertyChanged(); } }

        public bool IsValid
        {
            get
            {
              Validate(); 
              return CurrentError == null; 
           }
        }

        public RegularField()
        {
            this.Errors = new List<string>();
        }

        /// <summary>
        /// Validates this field and adds the Errors
        /// </summary>
        public virtual void Validate()
        {
            CurrentError = null;
            Errors.Clear();

            if (required && String.IsNullOrWhiteSpace(CoreValue))
            {
                Errors.Add("REQUIRED");
                CurrentError = "REQUIRED";
                return;
            }

            if (!required && String.IsNullOrWhiteSpace(CoreValue))
            {                
                return;
            }

            if(validations != null)
            {
                foreach(Validation validation in validations) 
                {
                    if(!Regex.IsMatch(CoreValue, validation.regex))
                    {
                        Errors.Add(validation.error_code);
                        CurrentError = validation.error_code;
                    }   
                }
            }

            OnPropertyChanged("IsValid");
        }

        /// <summary>
        /// Sets the API Default Value of the Field
        /// </summary>
        public virtual void SetDefaultValue() 
        {
            this.CoreValue = value != null ? value : String.Empty;
        }

    }
}