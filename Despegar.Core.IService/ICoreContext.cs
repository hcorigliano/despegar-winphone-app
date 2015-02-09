using Despegar.Core.Neo.Business;
using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Log;

namespace Despegar.Core.Neo.IService
{
    /// <summary>
    /// CoreContext contract.
    /// This interface is the main contract exposed by the Core library.
    /// </summary>
    public interface ICoreContext
    {
        string GetSite();

        string GetLanguage();

        string GetUOW();

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
        /// Returns the hoteles service object
        /// </summary>
        /// <returns></returns>
        IHotelService GetHotelService();

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
        /// returns UpaService Object
        /// </summary>
        /// <returns></returns>
        IUPAService GetUpaService();

        /// <summary>
        /// returns the CommonService Object
        /// </summary>
        /// <returns></returns>
        ICommonServices GetCommonService();

        /// <summary>
        /// returns the Coupons Service Object
        /// </summary>
        /// <returns></returns>
        ICouponsService GetCouponsService();

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
        /// Sets the bug tracker instance to log core events and errors
        /// </summary>
        /// <param name="bugtracker"></param>
        void SetBugTracker(IBugTracker bugtracker);
    }
}