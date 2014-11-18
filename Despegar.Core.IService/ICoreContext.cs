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
        /// Enables a specified Mock
        /// </summary>
        /// <param name="mockKey">The Mock Key</param>
        void EnableMock(MockKey mockKey);

        /// <summary>
        /// Disables a specified Mock
        /// </summary>
        /// <param name="mockKey">The Mock Key</param>
        void DisableMock(MockKey mockKey);

        /// <summary>
        /// Indicates whether a Mock is Enabled or not
        /// </summary>
        /// <param name="mockKey">The Mock Key</param>
        /// <returns>A boolean indicating the Mock status</returns>
        bool IsMockEnabled(MockKey mockKey);

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
        /// returns the CommonService Object
        /// </summary>
        /// <returns></returns>
        ICommonServices GetCommonService();

        /// <summary>
        /// Re-configures the Core for the new Site
        /// </summary>
        /// <param name="Site">Example: AR,CO,MX etc. </param>
        void SetSite(string siteCode);        
    }
}