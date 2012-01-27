using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CrunchApiExplorer.Framework.MVVM.Converters
{
    public class BooleanToAnythingConverter : IValueConverter
    {
        public object ValueWhenTrue { get; set; }

        public object ValueWhenFalse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool) value;

            return boolValue ? ValueWhenTrue : ValueWhenFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
