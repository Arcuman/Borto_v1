using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Borto_v1
{
    class NotContainSpaceRuse : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value.ToString().Contains(" ")
                ? new ValidationResult(false, "Spaces are not allowed in this field")
                : ValidationResult.ValidResult;
        }
    }
}
