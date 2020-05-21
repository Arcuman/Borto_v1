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
    class PasswordRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            if (!Regex.Match(charString, @"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9@#!^&*.]{8,})$").Success)
            {
                return new ValidationResult(false, $"Password must contains only english letter and at least one digit and one letter.(can contain @#!^&*.)");
            }
            return ValidationResult.ValidResult;
        }
    }
}
