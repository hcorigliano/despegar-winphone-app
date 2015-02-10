using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Contract.Connector
{
    public interface IApiv3Connector
    {
        void ConfigureSiteAndLanguage(string site, string lang);
    }
}