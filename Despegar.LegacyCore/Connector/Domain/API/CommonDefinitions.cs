using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{
    // -----------------------
    // Common Definitions:
    // -----------------------

    public class TextField : AbstractDefinition
    {

        // API properties

        public ObservableCollection<RegexValidations> regexValidations { get; set; }


        // View Model properties

        public virtual string Value { get { return value; } set { this.value = value; NotifyPropertyChanged("Value"); } }
        public string value = "";

        public bool Error
        {
            get
            {
                bool err = false;
                if (regexValidations != null)
                    err = regexValidations.Any(it => { return it.Error; });

                return error || err;
            }
        }
        public bool error;

        public void Validate()
        {
            if (regexValidations == null) return;

            foreach (RegexValidations it in regexValidations)
            {
                it.Validate(Value);
            }

            NotifyPropertyChanged("Error");
        }
    }

    public class MultivalueField : TextField
    {
        private MultivalueFieldOption selected;

        public List<MultivalueFieldOption> options { get; set; }        

        public MultivalueFieldOption Selected
        {
            get { if (selected != null) return selected; else if (options.Count > 0) return options[0]; else return selected; }
            set { selected = value; NotifyPropertyChanged("Selected"); NotifyPropertyChanged("Value"); }
        }

        public override string Value { get { return Selected.key; } }
    }

    public class MultivalueFieldOption
    {
        public string key { get; set; }
        public string description { get; set; }
    }

    public class DateYearMonthField : TextField
    {
        // API properties
        public string from { get; set; }
        public string to { get; set; }

        // Selected values
        private string valueMonth;
        private string valueYear;

        private int fromYear { get { return int.Parse(from.Substring(0, 4)); } }
        private int toYear { get { return int.Parse(to.Substring(0, 4)); } }
        private int fromMonth { get { return int.Parse(from.Substring(5, 2)); } }
        private int toMonth { get { return int.Parse(to.Substring(5, 2)); } }

        public List<string> Months { get { return new List<string>() { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" }; } }
        public List<string> Years
        {
            get
            {
                List<string> years = new List<string>();

                if (fromYear < toYear)
                    for (int i = fromYear; i < toYear; i++)
                        years.Add(i.ToString());

                return years;
            }
        }

        // viewmodel properties
        public string ValueMonth
        {
            get {
                if (valueMonth == null)
                     valueMonth = Months[0];

                return valueMonth;
            }
            set
            {
                this.valueMonth = value;

                ValidateDates();

                NotifyPropertyChanged("Error");
                NotifyPropertyChanged("ValueMonth");
                NotifyPropertyChanged("Value");
            }
        }

        public string ValueYear
        {
            get {              
                if (valueYear == null) {                    
                    valueYear = Years.Count > 0 ? Years[0] : "2016"; // returning null will break
                }

                return valueYear;
            }
            set
            {
                this.valueYear = value;

                ValidateDates();

                NotifyPropertyChanged("Error");
                NotifyPropertyChanged("ValueYear");
                NotifyPropertyChanged("Value");
            }
        }

        public override string Value { get { return ValueYear + "-" + ValueMonth; } }

        private void ValidateDates()
        {
            int month = int.Parse(valueMonth);
            int year = int.Parse(ValueYear);
            error = (month > toMonth && year== toYear) || (month < fromMonth && year == fromYear); 
        }
    }

    public class EmailTextField : TextField
    {
        public string Repeat { get { return repeat; } set { repeat = value; NotifyPropertyChanged("Repeat"); } }
        public string repeat;

        public void ValidateRepeat()
        {
            error = false;
            if (Value != Repeat) error = true;
            NotifyPropertyChanged("Error");
        }
    }

    public class RegexValidations : AbstractDefinition
    {
        // API Properties

        public string regex { get; set; }
        public string errorCode { get; set; }
        public RegexValidationsMetadata metadata { get; set; }


        // View Model Properties

        public bool Validate(string value)
        {
            Regex regex = new Regex(this.regex);
            if (!regex.IsMatch(value)) { Error = true; }
            else { Error = false; }
            return Error;
        }

        public bool Error { get { return error; } set { error = value; NotifyPropertyChanged("Error"); } }
        public bool error;
    }

    public class RegexValidationsMetadata
    {
        public string min { get; set; }
        public string max { get; set; }
        public string TYPE { get; set; }
    }

    public class CustomValidation : AbstractDefinition
    {
        // View Model Properties
        public delegate bool ExecDelegate();

        public ExecDelegate Execute { get; set; }

        public bool Error { get { return error; } set { error = value; NotifyPropertyChanged("Error"); } }
        public bool error;
    }
}
