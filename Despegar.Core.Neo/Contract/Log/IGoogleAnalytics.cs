using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Contract.Log
{
    public interface IGoogleAnalytics
    {
       void SendView(string pagePath);
    }
}
