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
    class NickNameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            if (!Regex.Match(charString, "^[a-zA-ZА-Яа-я]").Success)
            {
                return new ValidationResult(false, $"Field can only begin with a letter ");
            }
            if (!Regex.Match(charString, "^[a-zA-ZА-Яа-я][a-zA-ZА-Яа-я\\d._$#*!]*$").Success)
            {
                return new ValidationResult(false, $"Field can contain only letter , digits and ._$#*!");
            }

            return ValidationResult.ValidResult;
        }
    }
}
