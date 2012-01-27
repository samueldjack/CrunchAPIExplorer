using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using CrunchApiExplorer.Framework.MVVM;
using CrunchApiExplorer.Infrastructure.Controls;

namespace CrunchApiExplorer.Infrastructure.Services
{
    class DialogService : IDialogService
    {
        private readonly IViewLocator _viewLocator;
        private Stack<Window> _currentDialogs = new Stack<Window>();
 
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
                                 Content = view,
                                 WindowStartupLocation = WindowStartupLocation.CenterOwner,
                             };

            window.SetBinding(Window.TitleProperty, new Binding("Title") {Source = viewModel});

            viewModel.Closed += delegate
                                    {
                                        window.DialogResult = viewModel.DialogResult;
                                        window.Close();
                                    };
            
            return ShowDialog(window);
        }

        public void ShowErrorMessage(string message)
        {
            ShowMessageBox(message, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private MessageBoxResult ShowMessageBox(string message, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            return MessageBox.Show(GetCurrentTopWindow(), message, "Crunch API Explorer", messageBoxButton,
                            messageBoxImage);
        }

        public bool AskYesNoQuestion(string question)
        {
            return ShowMessageBox(question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        private bool? ShowDialog(DialogWindow window)
        {
            window.Owner = GetCurrentTopWindow();

            try
            {
                _currentDialogs.Push(window);
                return window.ShowDialog();
            }
            finally
            {
                _currentDialogs.Pop();
            }
        }

        private Window GetCurrentTopWindow()
        {
            if (_currentDialogs.Any())
            {
                return _currentDialogs.Peek();
            }
            else
            {
                return Application.Current.MainWindow;
            }
        }
    }
}
