using CrunchApiExplorer.Framework.MVVM;

namespace CrunchApiExplorer.Infrastructure.Services
{
    internal interface IDialogService
    {
        bool? ShowDialogForViewModel(DialogViewModel viewModel);
        void ShowErrorMessage(string message);
    }
}