using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CrunchApiExplorer.Framework.Extensions;

namespace CrunchApiExplorer.Framework.MVVM
{
    public abstract class ViewModel : INotifyPropertyChanged, IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(SymbolExtensions.GetPropertyName(propertyExpression)));
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        public void NotifyLoaded()
        {
            OnViewLoaded();
        }

        public void NotifyUnloaded()
        {
            OnViewUnloaded();
        }

        protected virtual void OnViewLoaded()
        {
            
        }

        protected virtual void OnViewUnloaded()
        {
            
        }
    }
}
