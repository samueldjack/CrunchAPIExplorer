using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using CrunchApiExplorer.Infrastructure.Services;
using Module = Autofac.Module;

namespace CrunchApiExplorer.Infrastructure.Modules
{
    class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .InNamespaceOf<DialogService>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
