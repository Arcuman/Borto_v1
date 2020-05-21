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
            if (char.IsDigit(charString[0]))
            {
                return new ValidationResult(false, $"Field cannot begin with a digit");
            }
            if (!Regex.Match(charString, "^[a-z][a-z\\d]*$").Success)
            {
                return new ValidationResult(false, $"Field can only contain only english lower case letters and digits");
            }

            return ValidationResult.ValidResult;
        }
    }
}
