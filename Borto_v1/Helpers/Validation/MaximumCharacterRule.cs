using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Borto_v1
{
    public class MaximumCharacterRule : ValidationRule
    {
        public int MaximumCharacters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            cultureInfo = CultureInfo.CurrentCulture;

            if (charString.Length > MaximumCharacters)
            {
                if (cultureInfo.Name == "en-US")
                    return new ValidationResult(false, $"The field should be less than {MaximumCharacters} characters.");
                else
                    return new ValidationResult(false, $"Поле должно содержать не более {MaximumCharacters} символов.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
