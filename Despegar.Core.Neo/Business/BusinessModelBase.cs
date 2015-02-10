using Despegar.Core.Neo.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public abstract class BusinessModelBase : Bindable 
    {
        public abstract bool IsValid { get; }
        public abstract bool Validate();
    }
}