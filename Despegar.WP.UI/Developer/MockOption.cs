using Despegar.Core.Business;
using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Developer
{
    public class MockOption
    {
        public ServiceKey ServiceKey { get; set; }
        public MockKey MockKey { get; set; }
        public string Name { get; set; }
        public bool Enabled
        {
            get;
            set;
            //{
            //    //GlobalConfiguration.CoreContext.AddMock();
            //};
        }
    }
}
