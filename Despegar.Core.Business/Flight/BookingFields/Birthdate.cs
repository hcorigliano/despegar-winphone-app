using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Birthdate : Bindable
    {
        // MAPI Fields
        public bool required { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string data_type { get; set; }

        // Custom
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
        public bool CorePostEnable { get; set; }

        

        public List<string> Errors { get; set; }

        private string currentError;
        public string CurrentError { get { return currentError; } set { currentError = value; OnPropertyChanged(); } }

        public bool IsValid
        {
            get
            {
              Validate(); 
              return CurrentError == null; 
           }
        }

        public Birthdate()
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

            if(String.IsNullOrWhiteSpace(CoreValue))
            {
                Errors.Add("REQUIRED");
                CurrentError = "REQUIRED";
                return;
            }

            DateTime valueTime = DateTime.ParseExact(CoreValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime ToTime = DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime FromTime = DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);



            if ((valueTime > ToTime || valueTime < FromTime) && required)
            {
                Errors.Add("REQUIRED");
                CurrentError = "REQUIRED";
                return;
               
            }

            //if (!required && String.IsNullOrWhiteSpace(CoreValue))
            //{                
            //    return;
            //}

            
        }
    }
}
