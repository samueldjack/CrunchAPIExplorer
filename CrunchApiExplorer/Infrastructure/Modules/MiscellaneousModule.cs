using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using CrunchApiExplorer.Framework.MVVM;

namespace CrunchApiExplorer.Infrastructure.Modules
{
    class MiscellaneousModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Messenger>()
                .SingleInstance()
                .AsImplementedInterfaces();
        }
    }
}
