using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrunchApiExplorer.Framework.MVVM
{
    public class DialogViewModel : ViewModel
    {
        private string _title;
        public event EventHandler<EventArgs> Closed;

        public bool? DialogResult { get; private set; }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        } 

        protected void Close(bool? dialogResult)
        {
            DialogResult = dialogResult;
            RaiseClosed(EventArgs.Empty);
        }

        private void RaiseClosed(EventArgs e)
        {
            EventHandler<EventArgs> handler = Closed;
            if (handler != null) handler(this, e);
        }
    }
}
