using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CrunchApiExplorer.ViewModels;

namespace CrunchApiExplorer.Views
{
    /// <summary>
    /// Interaction logic for VerifyCrunchAccessTokenDialogView.xaml
    /// </summary>
    public partial class VerifyCrunchAccessTokenDialogView : UserControl
    {
        public VerifyCrunchAccessTokenDialogView()
        {
            InitializeComponent();
            Loaded += HandleLoaded;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            WebBrowser.Navigate(ViewModel.AuthorisationUrl);
        }

        private void HandleWebBrowserPageLoadCompleted(object sender, NavigationEventArgs e)
        {
            dynamic htmlDocument = WebBrowser.Document;

            var element = htmlDocument.GetElementById("oauthVerifier");
            if (element == null)
            {
                return;
            }

            ViewModel.Verifier = element.InnerText;
        }

        private VerifyCrunchAccessTokenDialogViewModel ViewModel
        {
            get { return DataContext as VerifyCrunchAccessTokenDialogViewModel; }
        }
    }
}
