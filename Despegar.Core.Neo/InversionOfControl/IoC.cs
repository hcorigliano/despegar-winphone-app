using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.InversionOfControl
{
    public static class IoC
    {
        private static IContainer container;

        public static void LoadModules(bool isQA, IEnumerable<Module> modules)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CoreBasicModule(isQA));

            // Custom Modules
            foreach (Module mod in modules)
                builder.RegisterModule(mod);

            container = builder.Build();
        }

        /// <summary>
        /// Core exposes the resolve method as a Core Feature without exposing Autofac fmk
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T Resolve<T>() 
        {
             return container.Resolve<T>();
        }
    }
}