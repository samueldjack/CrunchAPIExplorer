using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CrunchApiExplorer.Crunch;
using CrunchApiExplorer.Framework.MVVM;
using CrunchApiExplorer.Infrastructure.Services;

namespace CrunchApiExplorer.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly Func<AuthenticateDialogViewModel> _authenticateViewModelFactory;
        private readonly ICrunchFacade _crunchFacade;
        private string _requestUrl;
        private string _response;
        private bool _isBusy;

        public MainWindowViewModel(IDialogService dialogService, Func<AuthenticateDialogViewModel> authenticateViewModelFactory, ICrunchFacade crunchFacade)
        {
            _dialogService = dialogService;
            _authenticateViewModelFactory = authenticateViewModelFactory;
            _crunchFacade = crunchFacade;
        }

        public ICommand ConnectCommand
        {
            get { return Commands.GetOrCreateCommand(() => ConnectCommand, HandleConnect); }
        }

        public ICommand MakeRequestCommand
        {
            get { return Commands.GetOrCreateCommand(() => MakeRequestCommand, HandleMakeRequest); }
        }

        private void HandleMakeRequest()
        {
            IsBusy = true;

            _crunchFacade.MakeRequestAsync(RequestUrl)
                .ContinueWith(HandleRequestComplete);
        }

        private void HandleRequestComplete(Task<XElement> task)
        {
            IsBusy = false;

            if (task.IsFaulted)
            {
                Response = task.Exception.InnerException.ToString();
            }
            else
            {
                Response = task.Result.ToString();
            }
        }

        private void HandleConnect()
        {
            var viewModel = _authenticateViewModelFactory();
            _dialogService.ShowDialogForViewModel(viewModel);
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        } 

        public string RequestUrl
        {
            get { return _requestUrl; }
            set
            {
                _requestUrl = value;
                RaisePropertyChanged(() => RequestUrl);
            }
        }

        public string Response
        {
            get { return _response; }
            set
            {
                _response = value;
                RaisePropertyChanged(() => Response);
            }
        } 

    }
}
