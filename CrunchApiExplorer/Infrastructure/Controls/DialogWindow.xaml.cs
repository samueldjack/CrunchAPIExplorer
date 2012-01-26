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
using System.Windows.Shapes;
using CrunchApiExplorer.Framework.Extensions;

namespace CrunchApiExplorer.Infrastructure.Controls
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty InitialWidthProperty =
            DependencyProperty.RegisterAttached("InitialWidth", typeof(double), typeof(DialogWindow), new UIPropertyMetadata(double.NaN));

        public static readonly DependencyProperty InitialHeightProperty =
            DependencyProperty.RegisterAttached("InitialHeight", typeof(double), typeof(DialogWindow), new UIPropertyMetadata(double.NaN));

        public static double GetInitialHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(InitialHeightProperty);
        }

        public static void SetInitialHeight(DependencyObject obj, double value)
        {
            obj.SetValue(InitialHeightProperty, value);
        }

        public static double GetInitialWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(InitialWidthProperty);
        }

        public static void SetInitialWidth(DependencyObject obj, double value)
        {
            obj.SetValue(InitialWidthProperty, value);
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            var frameworkElement = newContent as FrameworkElement;
            if (frameworkElement != null)
            {
                SetSizeMode(frameworkElement);
            }
        }

        private void SetSizeMode(FrameworkElement frameworkElement)
        {
            var initialWidth = GetInitialWidth(frameworkElement);
            var initialHeight = GetInitialHeight(frameworkElement);

            if (initialHeight.IsNaN() && initialWidth.IsNaN())
            {
                SizeToContent = SizeToContent.WidthAndHeight;
            }
            else if (initialHeight.IsNaN())
            {
                SizeToContent = SizeToContent.Height;
                Width = initialWidth;
            }
            else if (initialWidth.IsNaN())
            {
                SizeToContent = SizeToContent.Width;
                Height = initialHeight;
            }
            else
            {
                SizeToContent = SizeToContent.Manual;
                Width = initialWidth;
                Height = initialHeight;
            }
        }
    }
}
