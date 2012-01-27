using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CrunchApiExplorer.Framework.Extensions;

namespace CrunchApiExplorer.Framework.MVVM.Converters
{
    class TrueWhenStringNonEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            return !stringValue.IsNullOrWhiteSpace();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
