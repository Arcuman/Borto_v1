using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Borto_v1
{
    class LoginRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            cultureInfo = CultureInfo.CurrentCulture;
            if (char.IsDigit(charString[0]))
            {
                if (cultureInfo.Name == "en-US")
                    return new ValidationResult(false, $"Field cannot begin with a digit");
                else
                    return new ValidationResult(false, $"Поле не может начинаться с цифры");
            }
            if (!Regex.Match(charString, "^[a-zA-Z][a-zA-Z._\\d]*$").Success)
            {
                if (cultureInfo.Name == "en-US")
                    return new ValidationResult(false, $"Field can only contain only english letters and digits and ._");
                else
                    return new ValidationResult(false, $"Поле может содержать только латинские буквы и цифры и ._");

            }

            return ValidationResult.ValidResult;
        }
    }
}
