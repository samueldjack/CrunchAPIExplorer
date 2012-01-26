using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CrunchApiExplorer.Framework.MVVM;
using CrunchApiExplorer.Infrastructure.Services;

namespace CrunchApiExplorer.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly Func<AuthenticateDialogViewModel> _authenticateViewModelFactory;

        public MainWindowViewModel(IDialogService dialogService, Func<AuthenticateDialogViewModel> authenticateViewModelFactory)
        {
            _dialogService = dialogService;
            _authenticateViewModelFactory = authenticateViewModelFactory;
        }

        public ICommand ConnectCommand
        {
            get { return Commands.GetOrCreateCommand(() => ConnectCommand, HandleConnect); }
        }

        private void HandleConnect()
        {
            var viewModel = _authenticateViewModelFactory();
            _dialogService.ShowDialogForViewModel(viewModel);
        }
    }
}
