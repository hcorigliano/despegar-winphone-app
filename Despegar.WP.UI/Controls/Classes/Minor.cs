using Despegar.WP.UI.Model.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Controls.Classes
{
    public class Minor
    {
        public List<ChildrenAgeOption> OptionsItems { get; set; }
        public ChildrenAgeOption SelectedAge { get; set; }
        public int Index { get; set; }
        public int IndexZeroBased { get { return Index - 1; } }
    }
}
