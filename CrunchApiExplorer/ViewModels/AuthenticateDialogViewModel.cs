using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CrunchApiExplorer.Crunch;
using CrunchApiExplorer.Framework.MVVM;
using CrunchApiExplorer.Infrastructure.Services;

namespace CrunchApiExplorer.ViewModels
{
    class AuthenticateDialogViewModel : DialogViewModel
    {
        private readonly ICrunchFacade _crunchFacade;
        private readonly IDialogService _dialogService;
        private string _consumerKey;
        private string _sharedSecret;
        private string _requestTokenEndpoint;
        private string _accessTokenEndpoint;
        private string _userAuthorizationEndpoint;
        private bool _isBusy;

        public AuthenticateDialogViewModel(ICrunchFacade crunchFacade, IDialogService dialogService)
        {
            _crunchFacade = crunchFacade;
            _dialogService = dialogService;

            Title = "Connect to Crunch";
        }

        public string ConsumerKey
        {
            get { return _consumerKey; }
            set
            {
                _consumerKey = value;
                RaisePropertyChanged(() => ConsumerKey);
            }
        }

        public string SharedSecret
        {
            get { return _sharedSecret; }
            set
            {
                _sharedSecret = value;
                RaisePropertyChanged(() => SharedSecret);
            }
        }

        public string RequestTokenEndpoint
        {
            get { return _requestTokenEndpoint; }
            set
            {
                _requestTokenEndpoint = value;
                RaisePropertyChanged(() => RequestTokenEndpoint);
            }
        }

        public string AccessTokenEndpoint
        {
            get { return _accessTokenEndpoint; }
            set
            {
                _accessTokenEndpoint = value;
                RaisePropertyChanged(() => AccessTokenEndpoint);
            }
        }

        public string UserAuthorizationEndpoint
        {
            get { return _userAuthorizationEndpoint; }
            set
            {
                _userAuthorizationEndpoint = value;
                RaisePropertyChanged(() => UserAuthorizationEndpoint);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        } 

        public ICommand AuthenticateCommand
        {
            get { return Commands.GetOrCreateCommand(() => AuthenticateCommand, HandleAuthenticate); }
        }

        public ICommand CancelCommand
        {
            get { return Commands.GetOrCreateCommand(() => CancelCommand, () => Close(false)); }
        }

        private void HandleAuthenticate()
        {
            IsBusy = true;

            var task = _crunchFacade.ChangeConnectionAsync(new CrunchAuthorisationParameters(ConsumerKey, SharedSecret, RequestTokenEndpoint, AccessTokenEndpoint, UserAuthorizationEndpoint));

            task.ContinueWith(HandleChangeConnectionCompleted, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void HandleChangeConnectionCompleted(Task task)
        {
            IsBusy = false;

            if (task.IsFaulted)
            {
                _dialogService.ShowErrorMessage(task.Exception.InnerException.Message);
            }
            else
            {
                Close(true);
            }
        }

        protected override void OnViewLoaded()
        {
            var cap = _crunchFacade.GetCurrentAuthorisationParameters();

            ConsumerKey = cap.ConsumerKey;
            SharedSecret = cap.SharedSecret;
            UserAuthorizationEndpoint = cap.UserAuthorizationEndpoint;
            RequestTokenEndpoint = cap.RequestTokenEndpoint;
            AccessTokenEndpoint = cap.AccessTokenEndpoint;
        }
    }
}
