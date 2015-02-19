using Despegar.Core.Neo.Connector;

namespace Despegar.Core.Neo.Contract.Connector
{
    public interface IMapiConnector : IConnector
    {
        void ConfigureSiteAndLanguage(string site, string lang);
        void ConfigureClientAndUow(string x_client, string uow);
    }
}