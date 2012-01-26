using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrunchApiExplorer.Framework.MVVM;

namespace CrunchApiExplorer.ViewModels
{
    public class VerifyCrunchAccessTokenDialogViewModel : DialogViewModel
    {
        private string _verifier;

        public VerifyCrunchAccessTokenDialogViewModel()
        {
            Title = "Verify with Crunch";
        }

        public Uri AuthorisationUrl { get; set; }
        
        public string Verifier
        {
            get { return _verifier; }
            set
            {
                _verifier = value;
                Close(true);
            }
        }
    }
}
