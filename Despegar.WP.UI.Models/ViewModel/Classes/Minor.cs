using Despegar.Core.Business;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Classes.Flights;
using System.Collections.Generic;

namespace Despegar.WP.UI.Models.Controls.Classes
{
    public class Minor : Bindable
    {
        public List<ChildrenAgeOption> OptionsItems { get; set; }
        private ChildrenAgeOption selectedAge;
        public ChildrenAgeOption SelectedAge { get { return selectedAge; } set { selectedAge = value; OnPropertyChanged();} }
        public int Index { get; set; }
        public int IndexZeroBased { get { return Index - 1; } }
    }
}
