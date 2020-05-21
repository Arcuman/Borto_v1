
using System.Globalization;
using System.Windows.Controls;

namespace Borto_v1
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required and must be not empty")
                : ValidationResult.ValidResult;
        }
    }
}
