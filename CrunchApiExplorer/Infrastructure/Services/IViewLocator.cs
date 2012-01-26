using System.Windows;
using CrunchApiExplorer.Framework.MVVM;

namespace CrunchApiExplorer.Infrastructure.Services
{
    internal interface IViewLocator
    {
        FrameworkElement CreateViewForViewModel(ViewModel viewModel);
    }
}