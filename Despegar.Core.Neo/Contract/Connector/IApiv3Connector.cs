using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Business.Hotels.UserReviews;
using Despegar.Core.Neo.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Contract.Connector
{
    public interface IApiv3Connector : IConnector
    {
        void ConfigureSiteAndLanguage(string site, string lang);
        void ConfigureClientAndUow(string x_client, string uow, string userAgent);
    }
}