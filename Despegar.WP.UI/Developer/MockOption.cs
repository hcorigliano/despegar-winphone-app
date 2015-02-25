using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Business;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Developer
{
    public class MockOption : Bindable
    {
        public ServiceKey ServiceKey { get; set; }
        public string Name { get; set; }
        public bool Enabled
        {
            get { return GlobalConfiguration.CoreContext.IsMockEnabled(this.Name); }
            set {
                if (value)
                    GlobalConfiguration.CoreContext.EnableMock(this.Name);                
                else
                    GlobalConfiguration.CoreContext.DisableMock(this.Name);

                OnPropertyChanged("Enabled");
            }
        }
    }
}