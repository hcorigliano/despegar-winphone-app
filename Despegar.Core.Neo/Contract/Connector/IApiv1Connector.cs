using Despegar.Core.Neo.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Contract.Connector
{
    internal interface IApiv1Connector : IConnector
    {
        void Configure(string x_client, string uow, string site);
    }
}