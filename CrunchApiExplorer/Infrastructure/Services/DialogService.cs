using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrunchApiExplorer.Framework.MVVM;
using CrunchApiExplorer.Infrastructure.Controls;

namespace CrunchApiExplorer.Infrastructure.Services
{
    class DialogService : IDialogService
    {
        private readonly IViewLocator _viewLocator;

        public DialogService(IViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
        }

        public bool? ShowDialogForViewModel(DialogViewModel viewModel)
        {
            var view = _viewLocator.CreateViewForViewModel(viewModel);
            view.DataContext = viewModel;

            var window = new DialogWindow()
                             {
                                 Content = view
                             };

            viewModel.Closed += delegate
                                    {
                                        window.DialogResult = viewModel.DialogResult;
                                        window.Close();
                                    };

            return window.ShowDialog();
        }
    }
}
