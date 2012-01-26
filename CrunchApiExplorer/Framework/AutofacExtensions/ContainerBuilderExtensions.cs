using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Module = Autofac.Module;

namespace CrunchApiExplorer.Framework.AutofacExtensions
{
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Discovers all AutoFac modules in the given assembly having a default constructor, and adds them to the
        /// ContainerBuilder
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="assembly"></param>
        public static ContainerBuilder AddModulesFromAssembly(this ContainerBuilder containerBuilder, Assembly assembly)
        {
            var types = assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo<Module>() && t.GetConstructor(Type.EmptyTypes) != null && !t.IsAbstract);

            foreach (var type in types)
            {
                var module = (Module)Activator.CreateInstance(type);
                containerBuilder.RegisterModule(module);
            }

            return containerBuilder;
        }
    }
}
