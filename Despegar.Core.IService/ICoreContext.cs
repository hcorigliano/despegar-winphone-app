using Despegar.Core.Business;
using Despegar.Core.Business.Culture;

namespace Despegar.Core.IService
{
    /// <summary>
    /// CoreContext contract.
    /// This interface is the main contract exposed by the Core library.
    /// </summary>
    public interface ICoreContext
    {
        string GetSite();

        string GetLanguage();

        /// <summary>
        /// Enables a specified Mock for a given Service
        /// </summary>
        /// <param name="serviceKey">The Service Key</param>
        /// <param name="mockKey">The Mock Key</param>
        void AddMock(ServiceKey serviceKey, MockKey mockKey);

        /// <summary>
        /// Disables a specified Mock for a given Service
        /// </summary>
        /// <param name="serviceKey">The Service Key</param>
        void RemoveMock(ServiceKey serviceKey);

        /// <summary>
        /// Configures the Connection parameters for MAPI
        /// </summary>
        /// <param name="x_client"></param>
        /// <param name="uow"></param>
        /// <param name="site"></param>
        /// <param name="language"></param>
        void Configure(string x_client, string uow);

        /// <summary>
        /// Returns the Flight service object
        /// </summary> 
        IFlightService GetFlightService();

        /// <summary>
        /// returns the ConfigurationService Object
        /// </summary>
        /// <returns></returns>
        IConfigurationService GetConfigurationService();

        /// <summary>
        /// Re-configures the Core for the new Site
        /// </summary>
        /// <param name="Site">Example: AR,CO,MX etc. </param>
        void SetSite(string siteCode);
    }
}