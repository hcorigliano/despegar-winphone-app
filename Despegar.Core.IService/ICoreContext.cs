using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.IService
{
    public interface ICoreContext
    {       
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
        void Configure(string x_client, string uow, string site, string language);

        /// <summary>
        /// Returns the Flight service object
        /// </summary> 
        IFlightService GetFlightService();
    }
}