using Despegar.Core.Business;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Developer
{
    public class MockOption : BindableBase
    {
        public ServiceKey ServiceKey { get; set; }
        public MockKey MockKey { get; set; }
        public string Name { get; set; }
        public bool Enabled
        {
            get { return GlobalConfiguration.CoreContext.IsMockEnabled(this.MockKey); }
            set {
                if (value)
                    GlobalConfiguration.CoreContext.EnableMock(this.MockKey);                
                else
                    GlobalConfiguration.CoreContext.DisableMock(this.MockKey);

                OnPropertyChanged("Enabled");
            }
        }
    }
}
