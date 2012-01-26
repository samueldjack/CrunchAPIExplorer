using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CrunchApiExplorer.Framework.MVVM;

namespace CrunchApiExplorer.Infrastructure.Services
{
    class ViewLocator : IViewLocator
    {
        public ViewLocator()
        {
            
        }

        public FrameworkElement CreateViewForViewModel(ViewModel viewModel)
        {
            var viewModelType = viewModel.GetType();
            var viewModelNamespace = viewModelType.Namespace;

            if (!viewModelNamespace.EndsWith(".ViewModels"))
            {
                throw new InvalidOperationException("Expected ViewModel to be located in a namespace ending with '.ViewModels'");
            }

            var viewsNamespace = viewModelNamespace.Substring(0, viewModelNamespace.Length - "ViewModels".Length) 
                + "Views";

            var viewModelTypeName = viewModelType.Name;
            if (!viewModelTypeName.EndsWith("ViewModel"))
            {
                throw new InvalidOperationException("Expected ViewModel name to end with 'ViewModel'");
            }

            var viewTypeName = viewModelTypeName.Substring(0, viewModelTypeName.Length - "ViewModel".Length)
                + "View";

            var expectedFullTypeName = viewsNamespace + "." + viewTypeName;

            var type = Type.GetType(expectedFullTypeName);
            if (type == null)
            {
                throw new InvalidOperationException(string.Format("No corresponding View was found for '{0}. Tried '{1}'", viewModelType.FullName, expectedFullTypeName));
            }

            var instance = Activator.CreateInstance(type) as FrameworkElement;
            if (instance == null)
            {
                throw new InvalidOperationException(string.Format("Expected '{0}' to be derived from FrameworkElement", expectedFullTypeName));
            }

            return instance;
        }
    }
}
