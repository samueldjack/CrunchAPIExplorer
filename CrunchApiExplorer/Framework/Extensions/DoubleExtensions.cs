using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrunchApiExplorer.Framework.Extensions
{
    public static class DoubleExtensions
    {
        public static bool IsNaN(this double value)
        {
            return double.IsNaN(value);
        }
    }
}
