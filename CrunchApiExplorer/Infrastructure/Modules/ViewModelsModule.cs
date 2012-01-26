using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using CrunchApiExplorer.Framework.MVVM;
using Module = Autofac.Module;

namespace CrunchApiExplorer.Infrastructure.Modules
{
    class ViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<ViewModel>()
                .InstancePerDependency();
        }
    }
}
