
using System.Globalization;
using System.Windows.Controls;

namespace Borto_v1
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            cultureInfo = CultureInfo.CurrentCulture;
            if (cultureInfo.Name == "en-US")
                return string.IsNullOrWhiteSpace((value ?? "").ToString()) ? new ValidationResult(false, "Field is required and must be not empty")
                : ValidationResult.ValidResult;
            else
                return string.IsNullOrWhiteSpace((value ?? "").ToString()) ? new ValidationResult(false, "Это обязательное поле и не может быть пустым")
                : ValidationResult.ValidResult;
        }
    }
}
