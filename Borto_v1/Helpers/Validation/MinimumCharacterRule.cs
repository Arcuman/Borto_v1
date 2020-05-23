
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
            cultureInfo = CultureInfo.CurrentCulture;
            if (charString.Length < MinimumCharacters)
            {
                if (cultureInfo.Name == "en-US")
                    return new ValidationResult(false, $"This field must be at least {MinimumCharacters} characters.");
                else
                    return new ValidationResult(false, $"Минимальное количетво символов в поле: {MinimumCharacters} .");
            }

            return ValidationResult.ValidResult;
        }
    }
}
