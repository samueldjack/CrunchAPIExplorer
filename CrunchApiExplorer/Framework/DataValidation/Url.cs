using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CrunchApiExplorer.Framework.Extensions;

namespace CrunchApiExplorer.Framework.DataValidation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class Url : ValidationAttribute
    {
        public bool Absolute { get; set; }

        public override bool IsValid(object value)
        {
            var uriString = value as string;
            if (uriString.IsNullOrWhiteSpace())
            {
                return true;
            }

            Uri result;
            return Uri.TryCreate(uriString, Absolute ? UriKind.Absolute : UriKind.RelativeOrAbsolute, out result);
        }

        public override string FormatErrorMessage(string name)
        {
            return "You must supply a valid Uri.";
        }
    }
}
