using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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
using System.Xml;
using System.Xml.Linq;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Expression.Interactivity.Core;
using CrunchApiExplorer.Framework.Extensions;

namespace CrunchApiExplorer.Controls
{
    public class XmlEditor : Control
    {
        static XmlEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XmlEditor), new FrameworkPropertyMetadata(typeof(XmlEditor)));
        }

        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof (Lazy<XDocument>), typeof (XmlEditor), new UIPropertyMetadata(default(Lazy<XDocument>), HandleDocumentChanged));

        private static readonly DependencyPropertyKey DocumentErrorPropertyKey =
            DependencyProperty.RegisterReadOnly("DocumentError", typeof (string), typeof (XmlEditor),
                                                               new UIPropertyMetadata(default(string)));

        public string DocumentError
        {
            get { return (string) GetValue(DocumentErrorPropertyKey.DependencyProperty); }
            private set { SetValue(DocumentErrorPropertyKey, value); }
        }

        public XmlEditor()
        {
            TextDocument = new TextDocument();

            Observable.FromEventPattern(h => TextDocument.TextChanged += h, h => TextDocument.TextChanged -= h)
                .Throttle(TimeSpan.FromSeconds(0.25))
                .ObserveOnDispatcher()
                .Subscribe(_ => RecheckDocument());

            TextDocument.TextChanged += delegate
                                            {
                                                _localDocumentUpdate = true;
                                                SetCurrentValue(DocumentProperty, new Lazy<XDocument>(GetDocument, false));
                                                _localDocumentUpdate = false;
                                            };
        }

        private void RecheckDocument()
        {
            try
            {
                DocumentError = string.Empty;
                if (TextDocument.TextLength > 0)
                {
                    GetDocument();
                }
            }
            catch (XmlException exception)
            {
                DocumentError = exception.Message;
            }

            UpdateStates(true);
        }

        private void UpdateStates(bool useTransistions)
        {
            if (DocumentError.IsNullOrWhiteSpace())
            {
                VisualStateManager.GoToState(this, "Valid", useTransistions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Invalid", useTransistions);
            }
        }

        private static void HandleDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as XmlEditor;

            editor.HandleDocumentChanged(e);
        }

        private void HandleDocumentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_localDocumentUpdate)
            {
                return;
            }

            var document = (e.NewValue as Lazy<XDocument>).Value;
            if (document != null)
            {
                TextDocument.Text = document.ToString();
            }
        }

        private XDocument GetDocument()
        {
            if (TextDocument.TextLength > 0)
            {
                return XDocument.Parse(TextDocument.Text);
            }
            else
            {
                return new XDocument();
            }
        }

        private static readonly DependencyPropertyKey TextDocumentPropertyKey =
            DependencyProperty.RegisterReadOnly("TextDocument", typeof (TextDocument), typeof (XmlEditor), new UIPropertyMetadata(default(TextDocument)));

        private bool _localDocumentUpdate;

        public Lazy<XDocument> Document
        {
            get { return (Lazy<XDocument>) GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        public TextDocument TextDocument
        {
            get { return (TextDocument) GetValue(TextDocumentPropertyKey.DependencyProperty); }
            private set { SetValue(TextDocumentPropertyKey, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateStates(false);
        }
    }
}
