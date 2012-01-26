using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;
using CrunchApiExplorer.Framework.MVVM;

namespace CrunchApiExplorer.Framework.Behaviors
{
    public class NotifyViewModelOfLifeCycleEvents : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += HandleLoaded;
            AssociatedObject.Unloaded += HandleUnloaded;

            if (AssociatedObject.IsLoaded && ViewModel != null)
            {
                ViewModel.NotifyLoaded();
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= HandleLoaded;
            AssociatedObject.Unloaded -= HandleUnloaded;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.NotifyLoaded();
            }
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.NotifyUnloaded();
            }
        }

        private IViewModel ViewModel
        {
            get { return AssociatedObject.DataContext as IViewModel; }
        }
    }
}
