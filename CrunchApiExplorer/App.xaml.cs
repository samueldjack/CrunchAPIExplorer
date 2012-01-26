using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using CrunchApiExplorer.Framework.AutofacExtensions;

namespace CrunchApiExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = new ContainerBuilder()
                .AddModulesFromAssembly(Assembly.GetExecutingAssembly())
                .Build();
        }
    }
}
