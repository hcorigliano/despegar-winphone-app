using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Developer
{
    public class MockNoneOption : MockOption
    {
        public new bool Enabled { get; set; }

        public MockNoneOption() {
            this.Name = "None";
        }
    }
}
