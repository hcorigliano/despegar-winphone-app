using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.InversionOfControl
{
    public class CoreModule : Module
    {
        private bool IsQA { get; set; }

        public CoreModule(bool isQA)
        {
            this.IsQA = isQA;
        }
    }
}