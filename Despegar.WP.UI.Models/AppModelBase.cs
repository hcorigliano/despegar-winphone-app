using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public abstract class AppModelBase 
    {

        internal AppModelBase()
        {
            InitializeModel();
        }

        internal void Validate()
        {
            throw new NotImplementedException();
        }

        internal void InitializeModel()
        {
            throw new NotImplementedException();
        }
    }
}
