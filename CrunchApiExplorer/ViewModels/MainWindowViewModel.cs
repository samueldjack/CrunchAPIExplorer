﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using CrunchApiExplorer.Controls;
using CrunchApiExplorer.Crunch;
using CrunchApiExplorer.Framework.MVVM;
using CrunchApiExplorer.Infrastructure;
using CrunchApiExplorer.Infrastructure.Services;
using CrunchApiExplorer.Framework.Extensions;
using CrunchApiExplorer.Properties;

namespace CrunchApiExplorer.ViewModels
{
    class MainWindowViewModel : ViewModel, IDataErrorInfo
    {
        private readonly IDialogService _dialogService;
        private readonly Func<AuthenticateDialogViewModel> _authenticateViewModelFactory;
        private readonly ICrunchFacade _crunchFacade;
        private string _requestUrl;
        private string _responseError;
        private bool _isBusy;
        private HttpMethod _selectedHttpMethod;
        private Lazy<XDocument> _request;
        private string _connectedServer;
        private Lazy<XDocument> _responseDocument;

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
            ResponseError = string.Empty;
            ResponseDocument = null;

            var requestUri = GetRequestUri();
            if (requestUri == null)
            {
                return;
            }

            XDocument requestDocument = null;
            if (SelectedHttpMethod == HttpMethod.Post)
            {
                try
                {
                    requestDocument = _request != null ? _request.Value : null;
                }
                catch (XmlException ex)
                {
                    _dialogService.ShowErrorMessage("The xml you have entered contains an error:" + ex.Message);
                    return;
                }
            }

            if (!EnsureUserHasConfirmedUpdateToLiveServer())
            {
                return;
            }

            IsBusy = true;

            _crunchFacade.MakeRequestAsync(requestUri, SelectedHttpMethod, requestDocument)
                .ContinueWith(HandleRequestComplete);
        }

        private Uri GetRequestUri()
        {
            Uri uri;

            if (RequestUrl.IsNullOrWhiteSpace() || !Uri.TryCreate(RequestUrl, UriKind.Relative, out uri))
            {
                _dialogService.ShowErrorMessage("Please enter a URI relative to the Crunch server address.");
                return null;
            }

            return uri;
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
                ResponseError = task.Exception.InnerException.ToString();
            }
            else
            {
                var document = new XDocument(task.Result);
                ResponseDocument = new Lazy<XDocument>(() => document);
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
            private set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
                RaisePropertyChanged(() => CanEditRequest);
            }
        } 

        public bool CanEditRequest
        {
            get { return IsConnected && !IsBusy; }
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

        public string ResponseError
        {
            get { return _responseError; }
            set
            {
                _responseError = value;
                RaisePropertyChanged(() => ResponseError);
            }
        }

        public Lazy<XDocument> ResponseDocument
        {
            get { return _responseDocument; }
            set
            {
                _responseDocument = value;
                RaisePropertyChanged(() => ResponseDocument);
                RaisePropertyChanged(() => HasResponseDocument);
            }
        }

        public Lazy<XDocument> Request
        {
            get { return _request; }
            set
            {
                _request = value;
                RaisePropertyChanged(() => Request);
            }
        } 

        public bool HasResponseDocument
        {
            get { return ResponseDocument != null && ResponseDocument.Value != null; }
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
            RaisePropertyChanged(() => CanEditRequest);
        }

        public string this[string columnName]
        {
            get { return string.Empty; }
        }

        public string Error
        {
            get { return string.Empty; }
        }
    }
}
