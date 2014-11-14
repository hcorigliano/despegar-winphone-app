using Despegar.WP.UI.Model.Classes.Flights;
using System.Collections.Generic;

namespace Despegar.WP.UI.Models.Controls.Classes
{
    public class Minor
    {
        public List<ChildrenAgeOption> OptionsItems { get; set; }
        public ChildrenAgeOption SelectedAge { get; set; }
        public int Index { get; set; }
        public int IndexZeroBased { get { return Index - 1; } }
    }
}
