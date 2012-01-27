using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace CrunchApiExplorer.Framework.MVVM
{
    public class ErrorHolder
    {
        private List<ValidationResult> _validationResults;

        public string GetError(string propertyName)
        {
            if (_validationResults == null)
            {
                return string.Empty;
            }

            var firstError = _validationResults.FirstOrDefault(vr => vr.MemberNames.Contains(propertyName));

            return firstError != null ? firstError.ErrorMessage : string.Empty;
        }

        public bool Validate(object instance)
        {
            _validationResults = _validationResults ?? new List<ValidationResult>();
            _validationResults.Clear();

            var isValid = Validator.TryValidateObject(instance, new ValidationContext(instance, null, null),  _validationResults, true);

            return isValid;
        }
    }
}
