using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Common.Checkout
{
    public class RegularOptionsField : RegularField
    {
        private List<Option> _options { get; set; }
        public List<Option> options 
        {
            get { return _options; }
            set
            {
                _options = value;
                OnPropertyChanged();
            }
        }

        private Option selectedOption;
        public Option SelectedOption
        {
            get { return selectedOption; }
            set
            {
                selectedOption = value;

                if (value != null)
                   CoreValue = value.value;

                OnPropertyChanged();
            }
        }
       
        public override void Validate()
        {
            CurrentError = null;
            Errors.Clear();
            if (this.required && this.SelectedOption == null)
            {
                Errors.Add("REQUIRED");
                CurrentError = "REQUIRED";
            }
        }

        /// <summary>
        /// Sets the API Default Value of the Field
        /// </summary>
        public override void SetDefaultValue()
        {
            if (value == null)
            {
                // Select first option available
                if (options != null && options.Count > 0)                
                    this.SelectedOption = options.FirstOrDefault();
                
            } else {
                // API default                
                this.SelectedOption = options.SingleOrDefault(x => x.value == value);
            }
        }
    }
}