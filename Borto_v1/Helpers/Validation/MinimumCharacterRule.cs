
using System.Globalization;
using System.Windows.Controls;

namespace Borto_v1
{

    public class MinimumCharacterRule : ValidationRule
    {
        public int MinimumCharacters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $"This field must be at least {MinimumCharacters} characters.");

            return ValidationResult.ValidResult;
        }
    }
}
