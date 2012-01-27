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
using CrunchApiExplorer.Infrastructure.Services;
using CrunchApiExplorer.Framework.Extensions;

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
            IsBusy = true;
            Response = string.Empty;

            XDocument requestDocument = null;
            if (SelectedHttpMethod == HttpMethod.Post && !Request.IsNullOrWhiteSpace())
            {
                requestDocument = XDocument.Parse(Request);
            } 

            _crunchFacade.MakeRequestAsync(RequestUrl, SelectedHttpMethod, requestDocument)
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
            get { return SelectedHttpMethod == HttpMethod.Post; }
        }

        protected override void OnViewLoaded()
        {
            SelectedHttpMethod = HttpMethod.Get;
        }
    }
}
