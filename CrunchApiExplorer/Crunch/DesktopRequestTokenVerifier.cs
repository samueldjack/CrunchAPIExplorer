using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CrunchApiExplorer.Infrastructure;
using CrunchApiExplorer.Infrastructure.Services;
using CrunchApiExplorer.ViewModels;

namespace CrunchApiExplorer.Crunch
{
    class DesktopRequestTokenVerifier : IVerifyUserRequestToken
    {
        private readonly IDialogService _dialogService;
        private readonly Func<VerifyCrunchAccessTokenDialogViewModel> _viewModelFactory;

        public DesktopRequestTokenVerifier(IDialogService dialogService, Func<VerifyCrunchAccessTokenDialogViewModel> viewModelFactory)
        {
            _dialogService = dialogService;
            _viewModelFactory = viewModelFactory;
        }

        public Task<string> Verify(Uri userAuthorisationUri)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        var viewModel = _viewModelFactory();
                        viewModel.AuthorisationUrl = userAuthorisationUri;

                        _dialogService.ShowDialogForViewModel(viewModel);

                        return viewModel.Verifier;
                    },
                CancellationToken.None, TaskCreationOptions.None, Schedulers.UIThread);
        }
    }
}
