using Despegar.Core.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsMinorsAge : Bindable
    {
        public int Index { get; set; }
        public int IndexZeroBased { get { return Index - 1; } }
        private int selectedAge;
        public int SelectedAge
        {
            get 
            { return selectedAge; }
            set
            { selectedAge = value; OnPropertyChanged(); }
        }

        public IEnumerable<int> AgeOptions
        {
            get
            {
                IEnumerable<int> options = Enumerable.Range(0, 17);
                return options;
            }
        }

        public HotelsMinorsAge()
        {
            this.selectedAge = 0;
        }
    }
}
