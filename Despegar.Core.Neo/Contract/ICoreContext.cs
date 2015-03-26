using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Business;
using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Connector;
using Despegar.Core.Neo.Log;

namespace Despegar.Core.Neo.Contract
{
    /// <summary>
    /// CoreContext contract.
    /// This interface is the main contract exposed by the Core library.
    /// </summary>
    public interface ICoreContext
    {
        string GetSite();

        string GetLanguage(bool isDecolar);

        string GetUOW();

        /// <summary>
        /// Enables a specified Mock
        /// </summary>
        /// <param name="mockKey">The Mock Key</param>
        void EnableMock(string mockKey);

        /// <summary>
        /// Disables a specified Mock
        /// </summary>
        /// <param name="mockKey">The Mock Key</param>
        void DisableMock(string mockKey);

        /// <summary>
        /// Indicates whether a Mock is Enabled or not
        /// </summary>
        /// <param name="mockKey">The Mock Key</param>
        /// <returns>A boolean indicating the Mock status</returns>
        bool IsMockEnabled(string mockKey);

        /// <summary>
        /// Configures the Connection parameters for MAPI
        /// </summary>
        /// <param name="x_client"></param>
        /// <param name="uow"></param>
        /// <param name="site"></param>
        /// <param name="language"></param>
        void Configure(string x_client, string uow);

        /// <summary>
        /// Re-configures the Core for the new Site
        /// </summary>
        /// <param name="Site">Example: AR,CO,MX etc. </param>
        void SetSite(string siteCode);

        /// <summary>
        /// Returns configuaration pre-load from mapi.
        /// </summary>
        /// <returns></returns>
        Configuration GetConfiguration();

        /// <summary>
        /// load the configuration from mapi
        /// </summary>
        /// <param name="value"></param>
        void SetConfiguration(Configuration value);    

        /// <summary>
        /// Gets the assigned mock for a given service. Null if there is none.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetMockedResponse(ServiceKey key);
    }
}