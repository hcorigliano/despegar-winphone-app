using Autofac;
using Despegar.Core.Neo.Contract.Log;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.Interfaces;

namespace Despegar.WP.UI.InversionOfControl
{
    public class WindowsPhoneModule : CoreModule
    {
        public WindowsPhoneModule(bool isQA) : base(isQA)
        {
        }

        protected override void Load(ContainerBuilder builder) 
        { 
            builder.RegisterType<Navigator>().As<INavigator>().SingleInstance();
            builder.RegisterType<SplunkMintBugTracker>().As<IBugTracker>().SingleInstance();
#if !DEBUG
            builder.RegisterType<GoogleAnalyticContainer>().As<IGoogleAnalytics>().SingleInstance();
#endif
        }

    }
}
