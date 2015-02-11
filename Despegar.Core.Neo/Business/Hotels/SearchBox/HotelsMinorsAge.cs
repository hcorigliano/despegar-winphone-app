using Despegar.Core.Neo.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Hotels.SearchBox
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
                return Enumerable.Range(0, 17);
            }
        }

        public HotelsMinorsAge()
        {
            this.selectedAge = 0;
        }
    }
}