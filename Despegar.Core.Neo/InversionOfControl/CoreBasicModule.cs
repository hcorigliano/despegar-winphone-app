using Autofac;
using Despegar.Core.Neo.API.MAPI;
using Despegar.Core.Neo.API.UPA;
using Despegar.Core.Neo.API.V3;
using Despegar.Core.Neo.Connector;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Connector;
using Despegar.Core.Neo.Contract.Log;
using Despegar.Core.Neo.Log;
using Despegar.Core.Neo.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.InversionOfControl
{
    /// <summary>
    /// Configures the App Dependencies according to the Environment configuration
    /// </summary>
    public class CoreBasicModule : CoreModule
    {        
        public CoreBasicModule(bool isQA) : base(isQA)
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CoreContext>().As<ICoreContext>().SingleInstance();
            // Logging and Event/Bug Trackers
            builder.RegisterType<CoreLogger>().As<ICoreLogger>().SingleInstance();
            builder.RegisterType<EmptyBugTracker>().As<IBugTracker>().SingleInstance();
            // Connectors
            builder.RegisterType<MapiConnector>().As<IMapiConnector>().SingleInstance();
            builder.RegisterType<Apiv1Connector>().As<IApiv1Connector>().SingleInstance();
            builder.RegisterType<Apiv3Connector>().As<IApiv3Connector>().SingleInstance();
            builder.RegisterType<UPAConnector>().As<IUPAConnector>().SingleInstance();
            // MAPI
            builder.RegisterType<MAPIFlights>().As<IMAPIFlights>().SingleInstance();
            builder.RegisterType<MAPIHotels>().As<IMAPIHotels>().SingleInstance();            
            builder.RegisterType<MAPIHotels>().As<IMAPIHotels>().SingleInstance();
            builder.RegisterType<MAPICross>().As<IMAPICross>().SingleInstance();
            builder.RegisterType<MAPICoupons>().As<IMAPICoupons>().SingleInstance();
            builder.RegisterType<MAPINotifications>().As<IMAPINotifications>().SingleInstance();
            // Others APIs
            builder.RegisterType<APIv1>().As<IAPIv1>().SingleInstance();
            builder.RegisterType<APIv3>().As<IAPIv3>().SingleInstance();
            builder.RegisterType<UPAService>().As<IUPAService>().SingleInstance();
        }
     
    }
}