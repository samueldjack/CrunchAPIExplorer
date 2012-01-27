using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CrunchApiExplorer.Crunch;
using CrunchApiExplorer.Framework.MVVM;
using CrunchApiExplorer.Infrastructure;
using CrunchApiExplorer.Infrastructure.Services;
using CrunchApiExplorer.Framework.Extensions;
using CrunchApiExplorer.Properties;

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
        private HttpMethod _selectedHttpMethod;
        private string _request;
        private string _connectedServer;

        public MainWindowViewModel(IDialogService dialogService, Func<AuthenticateDialogViewModel> authenticateViewModelFactory, ICrunchFacade crunchFacade)
        {
            _dialogService = dialogService;
            _authenticateViewModelFactory = authenticateViewModelFactory;
            _crunchFacade = crunchFacade;

            _crunchFacade.ConnectionStatusChanged += delegate
                                                         {
                                                             // the event gets raised on a background thread, so marshal to the UI thread
                                                             Task.Factory.StartNew(UpdateConnectedServer,Schedulers.UIThread);
                                                         };
        }

        public ICommand ConnectCommand
        {
            get { return Commands.GetOrCreateCommand(() => ConnectCommand, DoConnect); }
        }

        public ICommand MakeRequestCommand
        {
            get { return Commands.GetOrCreateCommand(() => MakeRequestCommand, HandleMakeRequest); }
        }

        public IList<HttpMethod> AvailableHttpMethods
        {
            get { return new[] {HttpMethod.Get, HttpMethod.Post, HttpMethod.Put, HttpMethod.Delete}; }
        }

        public HttpMethod SelectedHttpMethod
        {
            get { return _selectedHttpMethod; }
            set
            {
                _selectedHttpMethod = value;
                RaisePropertyChanged(() => SelectedHttpMethod);
                RaisePropertyChanged(() => IsRequestVisibile);
            }
        } 

        private void HandleMakeRequest()
        {
            var requestUri = new Uri(RequestUrl, UriKind.Relative);

            XDocument requestDocument = null;
            if (SelectedHttpMethod == HttpMethod.Post && !Request.IsNullOrWhiteSpace())
            {
                requestDocument = XDocument.Parse(Request);
            }

            if (!EnsureUserHasConfirmedUpdateToLiveServer())
            {
                return;
            }

            IsBusy = true;
            Response = string.Empty;

            _crunchFacade.MakeRequestAsync(requestUri, SelectedHttpMethod, requestDocument)
                .ContinueWith(HandleRequestComplete);
        }

        private bool EnsureUserHasConfirmedUpdateToLiveServer()
        {
            if (_crunchFacade.Authority.Authority == Settings.Default.LiveServer.Authority
                && SelectedHttpMethod != HttpMethod.Get)
            {
                return _dialogService.AskYesNoQuestion("You are about to change data on the live server. Are you sure you want to continue?");
            }

            return true;
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

        private void DoConnect()
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

        public string Request
        {
            get { return _request; }
            set
            {
                _request = value;
                RaisePropertyChanged(() => Request);
            }
        } 

        public bool IsRequestVisibile
        {
            get { return SelectedHttpMethod == HttpMethod.Post || SelectedHttpMethod == HttpMethod.Put; }
        }

        public string ConnectedServer
        {
            get { return _connectedServer; }
            set
            {
                _connectedServer = value;
                RaisePropertyChanged(() => ConnectedServer);
            }
        } 

        public bool IsConnected
        {
            get { return _crunchFacade.IsConnected; }
        }

        protected override void OnViewLoaded()
        {
            SelectedHttpMethod = HttpMethod.Get;

            UpdateConnectedServer();

            if (!IsConnected)
            {
                DoConnect();
            }
        }

        private void UpdateConnectedServer()
        {
            ConnectedServer = IsConnected ? _crunchFacade.Authority.ToString() : string.Empty;
            RaisePropertyChanged(() => IsConnected);
        }
    }
}
